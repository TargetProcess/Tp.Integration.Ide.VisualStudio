using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Collections.Generic;
using System.Timers;

namespace Tp.Integration.Ide.VisualStudio.Services {
	/// <summary>
	/// Specifies that a user is idle or busy.
	/// </summary>
	internal sealed class IdleTrackerEventArgs : EventArgs {
		private readonly bool _idle;

		private readonly TimeSpan _span;

		public IdleTrackerEventArgs(bool idle, TimeSpan span) {
			_idle = idle;
			_span = span;
		}

		public bool Idle {
			get { return _idle; }
		}

		public bool Busy {
			get { return !_idle; }
		}

		public TimeSpan Span {
			get { return _span; }
		}
	}

	/// <summary>
	/// Notifies when a user becomes idle or busy.
	/// </summary>
	internal delegate void IdleTrackerEventHandler(IdleTrackerEventArgs idleTrackerEventArgs);

	/// <summary>
	/// Monitors when a user is idle, i.e he/she does not use computer, 
	/// i.e. when screen saver is running, or when desktop is locked, 
	/// or when keyboard or mouse is not used for some time.
	/// </summary>
	internal sealed class IdleTracker : IDisposable {
		private readonly int _pollInterval;

		private readonly int _notifyInterval;

		private readonly Timer _timer;

		private DateTime _idleStart = DateTime.MinValue;

		private bool _disposed;

		/// <summary>
		/// Subscribe to this event to be notified when a user becomes idle or busy.
		/// </summary>
		public event IdleTrackerEventHandler OnIdle;

		/// <summary>
		/// Creates new <see cref="IdleTracker"/> instance.
		/// </summary>
		/// <param name="pollInterval">Poll interval in seconds.</param>
		/// <param name="notifyInterval">User idle time interval in seconds.</param>
		/// <exception cref="ArgumentException">If poll interval is greater than notify interval.</exception>
		public IdleTracker(int pollInterval, int notifyInterval) {
			if (pollInterval < 1) {
				throw new ArgumentOutOfRangeException("pollInterval", "Interval cannot be negative");
			}
			if (notifyInterval < 1) {
				throw new ArgumentOutOfRangeException("notifyInterval", "Interval cannot be negative");
			}
			if (pollInterval > notifyInterval) {
				throw new ArgumentException("Poll interval cannot be grater than notify interval");
			}
			_pollInterval = pollInterval;
			_notifyInterval = notifyInterval;
			_timer = new Timer();
			_timer.Elapsed += TimerElapsed;
			_timer.Enabled = true;
		}

		#region IDisposable Members

		~IdleTracker() {
			Dispose(false);
		}

		public void Dispose() {
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public void Dispose(bool disposing) {
			if (_disposed) {
				return;
			}
			_disposed = true;
			_timer.Dispose();
		}

		#endregion

		private void TimerElapsed(object sender, ElapsedEventArgs ev) {
			uint idleTime = NativeMethods.GetIdleTime();
			if (_idleStart != DateTime.MinValue) {
				// is idle
				if (idleTime < _pollInterval) {
					// switch to busy
					if (OnIdle != null) {
						OnIdle(new IdleTrackerEventArgs(false, DateTime.Now - _idleStart));
					}
					_idleStart = DateTime.MinValue;
				}
			}
			else {
				// is busy
				if (idleTime > _notifyInterval) {
					// switch to idle
					_idleStart = DateTime.Now;
					if (OnIdle != null) {
						OnIdle(new IdleTrackerEventArgs(true, new TimeSpan(idleTime*1000)));
					}
				}
			}
		}
	}

	internal static class NativeMethods {
		[DllImport("Kernel32")]
		private static extern uint GetTickCount();

		[DllImport("User32.dll")]
		private static extern bool GetLastInputInfo([In, Out] ref LASTINPUTINFO plii);

		[DllImport("Kernel32.dll")]
		private static extern uint GetLastError();

		public static uint GetIdleTime() {
			var lastInPut = new LASTINPUTINFO();
			lastInPut.cbSize = (uint) Marshal.SizeOf(lastInPut);
			GetLastInputInfo(ref lastInPut);
			return ((uint) Environment.TickCount - lastInPut.dwTime);
		}

		public static uint GetLastInputTime() {
			LASTINPUTINFO lastInPut = new LASTINPUTINFO();
			lastInPut.cbSize = (uint) Marshal.SizeOf(lastInPut);
			if (!GetLastInputInfo(ref lastInPut)) {
				throw new SystemException(string.Format("Error executing GetLastInputInfo: {0}", GetLastError()));
			}
			return lastInPut.dwTime;
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct LASTINPUTINFO {
			[MarshalAs(UnmanagedType.U4)]
			public uint cbSize;

			[MarshalAs(UnmanagedType.U4)]
			public uint dwTime;
		}
	}
}