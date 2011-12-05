// 
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Forms;
using Tp.AssignableServiceProxy;
using Tp.Integration.Ide.VisualStudio.UI;
using Tp.MyAssignmentsServiceProxy;

namespace Tp.Integration.Ide.VisualStudio.Services
{

	#region Events

	public sealed class ConnectEventArgs : EventArgs
	{
		private readonly bool _connected;

		public ConnectEventArgs(bool connected)
		{
			_connected = connected;
		}

		public bool Connected
		{
			get { return _connected; }
		}
	}

	public sealed class AssignmentEventArgs : EventArgs
	{
		private readonly AssignableSimpleDTO _assignable;

		public AssignmentEventArgs(AssignableSimpleDTO assignable)
		{
			_assignable = assignable;
		}

		public AssignableSimpleDTO Assignable
		{
			get { return _assignable; }
		}
	}

	public sealed class AssignmentTimeEventArgs : EventArgs
	{
		private readonly int _assignableID;
		private readonly DateTime _started;
		private readonly DateTime? _stopped;

		public AssignmentTimeEventArgs(int assignableID, DateTime started)
		{
			_assignableID = assignableID;
			_started = started;
			_stopped = null;
		}

		public AssignmentTimeEventArgs(int assignableID, DateTime started, DateTime? stopped)
		{
			_assignableID = assignableID;
			_started = started;
			_stopped = stopped;
		}

		public int AssignableID
		{
			get { return _assignableID; }
		}

		public DateTime Started
		{
			get { return _started; }
		}

		public DateTime? Stopped
		{
			get { return _stopped; }
		}
	}

	public sealed class ConnectionStateEventArgs : EventArgs
	{
		private readonly bool _connected;
		private readonly string _uri;

		public ConnectionStateEventArgs(bool connected)
		{
			_connected = connected;
			_uri = string.Empty;
		}

		public ConnectionStateEventArgs(bool connected, string uri)
		{
			_connected = connected;
			_uri = uri;
		}

		public bool Connected
		{
			get { return _connected; }
		}

		public string Uri
		{
			get { return _uri; }
		}
	}

	#endregion

	/// <summary>
	/// Performs user interface related tasks.
	/// </summary>
	internal interface IControllerEnvironment
	{
		/// <summary>
		/// Shows the specified form as a modal dialog box.
		/// </summary>
		/// <param name="form">The form to show as a modal dialog box.</param>
		/// <returns>One of the <see cref="System.Windows.Forms.DialogResult"/> values.</returns>
		DialogResult ShowDialog(Form form);

		/// <summary>
		/// Displays a message box in front of the specified object and with the specified text, caption, buttons, and icon.
		/// </summary>
		/// <param name="text">The text to display in the message box.</param>
		/// <param name="caption">The text to display in the title bar of the message box.</param>
		/// <param name="buttons">One of the <see cref="System.Windows.Forms.MessageBoxButtons"/> values that specifies which buttons to display in the message box.</param>
		/// <param name="icon">One of the <see cref="System.Windows.Forms.MessageBoxIcon"/> values that specifies which icon to display in the message box.</param>
		/// <returns></returns>
		DialogResult ShowMessageBox(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon);

		/// <summary>
		/// Gets listener to log controller activities.
		/// </summary>
		TraceListener Trace { get; }
	}

	/// <summary>
	/// The default environment implementation.
	/// </summary>
	internal class ControllerEnvironment : IControllerEnvironment
	{
		private readonly IWin32Window _mainWindow;
		private readonly TraceListener _trace;

		/// <summary>
		/// Creates new instance of this class.
		/// </summary>
		/// <param name="mainWindow">An implementation of <see cref="System.Windows.Forms.IWin32Window"/> that will own the modal dialog boxes.</param>
		/// <param name="listener">An arbitrary <see cref="TraceListener"/> instance.</param>
		public ControllerEnvironment(IWin32Window mainWindow, TraceListener listener)
		{
			if (mainWindow == null)
			{
				throw new ArgumentNullException("mainWindow");
			}
			if (listener == null)
			{
				throw new ArgumentNullException("listener");
			}
			_trace = listener;
			_mainWindow = mainWindow;
		}

		#region IControllerEnvironment Members

		/// <summary>
		/// Calls <see cref="Form.ShowDialog()"/> method on the specified instance and returns its return value.
		/// </summary>
		public DialogResult ShowDialog(Form form)
		{
			return form.ShowDialog(_mainWindow);
		}

		/// <summary>
		/// Calls <see cref="MessageBox.Show(string, string, MessageBoxButtons, MessageBoxIcon)"/> method with the specified parameters and returns its return value.
		/// </summary>
		public DialogResult ShowMessageBox(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
		{
			return MessageBox.Show(_mainWindow, text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		/// <summary>
		/// Gets instance specified in the constructor parameter.
		/// </summary>
		public TraceListener Trace
		{
			get { return _trace; }
		}

		#endregion
	}

	/// <summary>
	/// Arranges web services calls and the underlying <see cref="StopWatch"/> component.
	/// </summary>
	internal sealed class Controller : IDisposable
	{
		private readonly ITimeTrackingRepository _timeTrackingRepository;
		private readonly IWebServicesFactory _webServicesFactory;
		private readonly IControllerEnvironment _env;
		private readonly StopWatch _stopWatch;
		private readonly Dictionary<int, string> _names = new Dictionary<int, string>();
		private IWebServices _webServices;
		private bool _connected;
		private MyAssignments _assignments = new MyAssignments();

		public Controller(ITimeTrackingRepository timeTrackingRepository,
		                  IWebServicesFactory webServicesFactory,
		                  IControllerEnvironment env)
		{
			_timeTrackingRepository = timeTrackingRepository;
			_webServicesFactory = webServicesFactory;
			_env = env;
			_stopWatch = new StopWatch(_timeTrackingRepository);
			_stopWatch.OnStart += StopWatch_OnStart;
			_stopWatch.OnStop += StopWatch_OnStop;
		}

		private void StopWatch_OnStart(StopWatch stopWatch, StopWatchEventArgs eventArgs)
		{
			if (OnAssignmentTimeStarted != null)
			{
				try
				{
					OnAssignmentTimeStarted(
						this, new AssignmentTimeEventArgs(
						      	eventArgs.TimeRecord.AssignableID, eventArgs.TimeRecord.Started));
				}
				catch (Exception ex)
				{
					_env.Trace.WriteLine(ex);
				}
			}
		}

		private void StopWatch_OnStop(StopWatch stopWatch, StopWatchEventArgs eventArgs)
		{
			if (OnAssignmentTimeStopped != null)
			{
				try
				{
					OnAssignmentTimeStopped(
						this, new AssignmentTimeEventArgs(
						      	eventArgs.TimeRecord.AssignableID, eventArgs.TimeRecord.Started, eventArgs.TimeRecord.Ended));
				}
				catch (Exception ex)
				{
					_env.Trace.WriteLine(ex);
				}
			}
		}

		public void Options()
		{
			var settings = new Settings();
			var form = new OptionsForm(settings);
			if (_env.ShowDialog(form) == DialogResult.OK)
			{
				settings.Save();
				_env.Trace.WriteLine("Options updated");
			}
		}

		public void Connect(bool prompt)
		{
			if (_connected)
			{
				throw new InvalidOperationException("Already connected");
			}
			var settings = Settings.Default;
			if (prompt || settings.HasCredentials == false)
			{
				var form = new LoginForm(settings);
				if (_env.ShowDialog(form) == DialogResult.OK)
				{
					if (ServicePointManager.ServerCertificateValidationCallback == null)
						ServicePointManager.ServerCertificateValidationCallback +=
							delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
								{
									var webRequest = sender as HttpWebRequest;
									if (webRequest != null)
									{
										return webRequest.Address.Host == settings.Uri.Host;
									}
									return false;
								};
					_webServices = _webServicesFactory.CreateWebServices(settings);
					if (!settings.SaveLogin)
					{
						settings.Login = null;
						settings.DecryptedPassword = null;
					}
					settings.Save();
					CompleteConnect(settings);
				}
			}
			else
			{
				_webServices = _webServicesFactory.CreateWebServices(settings);
				CompleteConnect(settings);
			}
		}

		private void CompleteConnect(Settings settings)
		{
			if (Authenticate(_webServices))
			{
				_connected = true;
				_env.Trace.WriteLine(string.Format("Connected to {0}", settings.Uri));
				if (OnConnect != null)
				{
					OnConnect(this, new ConnectEventArgs(true));
				}
				if (OnConnectionStateChange != null)
				{
					OnConnectionStateChange(this, new ConnectionStateEventArgs(_connected, settings.Uri.AbsoluteUri));
				}
			}
		}

		public void Disconnect()
		{
			if (!_connected)
			{
				throw new InvalidOperationException("Not connected");
			}
			_connected = false;
			_env.Trace.WriteLine(string.Format("Disconnected"));
			if (OnDisconnect != null)
			{
				OnDisconnect(this, new ConnectEventArgs(false));
			}
			if (OnConnectionStateChange != null)
			{
				OnConnectionStateChange(this, new ConnectionStateEventArgs(false));
			}
		}

		public bool Connected
		{
			get { return _connected; }
		}

		private bool Authenticate(IWebServices webServices)
		{
			try
			{
				webServices.Authenticate();
				return true;
			}
			catch (Exception ex)
			{
				_env.Trace.WriteLine(ex);
				_env.ShowMessageBox("Authentication failed", "Could not login to TargetProcess", MessageBoxButtons.OK,
				                    MessageBoxIcon.Error);
				return false;
			}
		}

		public void Refresh()
		{
			if (!_connected)
			{
				throw new InvalidOperationException("Not connected");
			}
			MyAssignments assignments;
			try
			{
				assignments = _webServices.GetMyAssigments();
			}
			catch (Exception ex)
			{
				Disconnect();
				_env.Trace.WriteLine(ex);
				_env.ShowMessageBox(ex.Message, "Could not get To Do list from TargetProcess", MessageBoxButtons.OK,
				                    MessageBoxIcon.Error);
				return;
			}
			HandleToDoListChanges(assignments);
			_assignments = assignments;
			if (OnListRefresh != null)
			{
				OnListRefresh(this, EventArgs.Empty);
			}
			_env.Trace.WriteLine("To Do list refreshed");
			GetNames();
		}

		private void HandleToDoListChanges(MyAssignments assignments)
		{
			var current = _stopWatch.GetCurrent();
			if (current != null)
			{
				foreach (var assignable in assignments.Assignables)
				{
					if (assignable.ID == current.AssignableID)
					{
						return;
					}
				}
				_stopWatch.Stop("Removed from list");
			}
		}

		public MyAssignments MyAssignments
		{
			get
			{
				if (!_connected)
				{
					throw new InvalidOperationException("Not connected");
				}
				return _assignments;
			}
		}

		public TimeRecord GetCurrent()
		{
			return _stopWatch.GetCurrent();
		}

		/// <summary>
		/// Gets a value indicating whether the Controller is started.
		/// </summary>
		public bool IsStarted
		{
			get { return _stopWatch.IsRunning; }
		}

		public TimeRecord[] GetLog()
		{
			return _stopWatch.GetLog();
		}

		public void Start(int assignableID)
		{
			if (!_connected)
			{
				throw new InvalidOperationException("Not connected");
			}
			_stopWatch.Start(assignableID);
		}

		public void Stop()
		{
			if (!_connected)
			{
				throw new InvalidOperationException("Not connected");
			}
			_stopWatch.Stop();
		}

		public void Stop(string description)
		{
			if (!_connected)
			{
				throw new InvalidOperationException("Not connected");
			}
			_stopWatch.Stop(description);
		}

		public void Resume()
		{
			if (!_connected)
			{
				throw new InvalidOperationException("Not connected");
			}
			_stopWatch.Resume();
		}

		/// <summary>
		/// Stops time interval measurement and resets the elapsed time to zero.
		/// </summary>
		public void Reset()
		{
			_stopWatch.Reset();
		}

		public void ChangeState(AssignableSimpleDTO assignable, EntityStateDTO entityState)
		{
			if (!_connected)
			{
				throw new InvalidOperationException("Not connected");
			}
			try
			{
				_webServices.ChangeState(assignable, entityState);
			}
			catch (Exception ex)
			{
				_env.Trace.WriteLine(ex);
				_env.ShowMessageBox(ex.Message, "Could not change state", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Disconnect();
				return;
			}
			_env.Trace.WriteLine(string.Format("State changed to {0}", entityState.Name));
			Refresh();
		}

		/// <summary>
		/// Gets total time recorded so far.
		/// </summary>
		public TimeSpan TotalTime
		{
			get
			{
				var log = _stopWatch.GetLog();
				var totalTime = new TimeSpan(0);
				foreach (var timeRecord in log)
				{
					totalTime += (timeRecord.Ended - timeRecord.Started).Value;
				}
				return totalTime;
			}
		}

		/// <summary>
		/// Gets time spent on the current task, if any.
		/// </summary>
		public TimeSpan? CurrentTime
		{
			get
			{
				var current = _stopWatch.GetCurrent();
				return current != null ? _stopWatch.Elapsed : (TimeSpan?) null;
			}
		}

		/// <summary>
		/// Posts all recorded time to serever, clears spent time log if succesfull.
		/// </summary>
		public void SubmitTime()
		{
			if (!_connected)
			{
				throw new InvalidOperationException("Not connected");
			}
			if (_stopWatch.GetCurrent() != null)
			{
				throw new InvalidOperationException("Stop all tasks before submitting time");
			}
			var log = _stopWatch.GetLog();
			var times = new List<TimeSimpleDTO>(log.Length);
			foreach (var timeRecord in log)
			{
				var spent = timeRecord.Ended.Value - timeRecord.Started;
				if ((int) spent.TotalMinutes > 0)
				{
					times.Add(new TimeSimpleDTO
					          	{
					          		AssignableID = timeRecord.AssignableID,
					          		Started = timeRecord.Started,
					          		Ended = timeRecord.Ended.Value,
					          		Description = timeRecord.Description,
					          	});
				}
			}
			SubmitTimeError[] errors;
			try
			{
				errors = _webServices.SubmitTime(times.ToArray());
			}
			catch (Exception ex)
			{
				_env.Trace.WriteLine(ex);
				_env.ShowMessageBox(ex.Message, "Could not submit time to TargetProcess", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Disconnect();
				return;
			}
			_stopWatch.ClearLog();
			_env.Trace.WriteLine("Time submitted to server");
			if (errors != null && errors.Length != 0)
			{
				var s = new StringBuilder();
				s.Append("Could not submit time:");
				foreach (var error in errors)
				{
					s.Append("\n");
					s.Append(error.Message);
				}
				_env.Trace.WriteLine(s.ToString());
				_env.ShowMessageBox(s.ToString(), "Time submission errors", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}

		/// <summary>
		/// Updated internal dictionary with To Do list item names.
		/// </summary>
		private void GetNames()
		{
			if (!_connected)
			{
				throw new InvalidOperationException("Not connected");
			}
			// 1. Save names of the items from the To Do list. Later, when these tasks become closed and are removed from the list, we still remember their names.
			foreach (var assignment in _assignments.Assignables)
			{
				_names[assignment.ID.Value] = assignment.Name;
			}
			// 2. Get names of the items from the log using web service, only if these tasks are not currently in the To Do list.
			var ids = new List<int>();
			var log = _stopWatch.GetLog();
			foreach (var timeRecord in log)
			{
				if (!_names.ContainsKey(timeRecord.AssignableID) && !ids.Contains(timeRecord.AssignableID))
				{
					ids.Add(timeRecord.AssignableID);
				}
			}
			AssignableDTO[] assignables;
			try
			{
				assignables = _webServices.GetAssignables(ids.ToArray());
			}
			catch (Exception ex)
			{
				_env.Trace.WriteLine(ex);
				_env.ShowMessageBox(ex.Message, "Could not get task names from TargetProcess", MessageBoxButtons.OK,
				                    MessageBoxIcon.Error);
				return;
			}
			foreach (var assignable in assignables)
			{
				_names[assignable.AssignableID.Value] = assignable.Name;
			}
		}

		/// <summary>
		/// Gets dictionary with To Do list item names.
		/// </summary>
		public Dictionary<int, string> Names
		{
			get { return new Dictionary<int, string>(_names); }
		}

		#region IDisposable Members

		public void Dispose()
		{
			if (_connected)
			{
				var settings = new Settings();
				if (_stopWatch.IsRunning && settings.StopTrackingOnVisualStudioClose)
				{
					_stopWatch.Stop("Visual Studio closed, no description provided");
				}
				Disconnect();
			}
			_stopWatch.Dispose();
		}

		#endregion

		/// <summary>
		/// Notifies that controller has connected to the server.
		/// </summary>
		public event EventHandler<ConnectEventArgs> OnConnect;

		/// <summary>
		/// Notifies that controller has disconnected from the server.
		/// </summary>
		public event EventHandler<ConnectEventArgs> OnDisconnect;

		/// <summary>
		/// Notifies that controller has just refreshed its To Do list.
		/// </summary>
		public event EventHandler<EventArgs> OnListRefresh;

		/// <summary>
		/// Notifies that assignment time tracking started.
		/// </summary>
		public event EventHandler<AssignmentTimeEventArgs> OnAssignmentTimeStarted;

		/// <summary>
		/// Notifies that assignment time tracking stopped.
		/// </summary>
		public event EventHandler<AssignmentTimeEventArgs> OnAssignmentTimeStopped;

		/// <summary>
		/// Notifies that connection to the server was lost.
		/// </summary>
		public event EventHandler<ConnectionStateEventArgs> OnConnectionStateChange;
	}
}