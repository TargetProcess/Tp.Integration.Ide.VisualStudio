using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Tp.Integration.Ide.VisualStudio.Services
{
	public sealed class StopWatchEventArgs : EventArgs
	{
		private readonly TimeRecord _timeRecord;

		public StopWatchEventArgs(TimeRecord timeRecord)
		{
			_timeRecord = timeRecord;
		}

		public TimeRecord TimeRecord
		{
			get { return _timeRecord; }
		}
	}

	public delegate void StopWatchEventHandler(StopWatch stopWatch, StopWatchEventArgs eventArgs);

	/// <summary>
	/// Records when a user begins or ends working on an assignable entity, such as user story, task or bug.
	/// </summary>
	public sealed class StopWatch : IDisposable
	{
		private readonly ITimeTrackingRepository _repository;

		private TimeTracking _timeTracking;

		private DateTime _lastUpdated;

		private readonly Stopwatch _stopWatch;

		public event StopWatchEventHandler OnStart;

		public event StopWatchEventHandler OnStop;

		public StopWatch(ITimeTrackingRepository repository)
		{
			if (repository == null)
			{
				throw new ArgumentNullException("repository");
			}
			_repository = repository;
			_stopWatch = new Stopwatch();
		}

		/// <summary>
		/// Gets current time record, if any.
		/// </summary>
		public TimeRecord GetCurrent()
		{
			TimeTracking timeTracking;
			lock (_repository)
			{
				timeTracking = ReadTimeTracking();
			}
			return timeTracking.CurrentRecord;
		}

		/// <summary>
		/// Gets the Elapsed time tracked for a record so far.
		/// </summary>
		public TimeSpan Elapsed
		{
			get { return _stopWatch.Elapsed; }
		}

		/// <summary>
		/// Gets a value indicating whether the StopWatch timer is running.
		/// </summary>
		public bool IsRunning
		{
			get { return _stopWatch.IsRunning; }
		}

		/// <summary>
		/// Gets array of time tracking entries recorded so far.
		/// </summary>
		public TimeRecord[] GetLog()
		{
			TimeTracking timeTracking;
			lock (_repository)
			{
				timeTracking = ReadTimeTracking();
			}
			return timeTracking.Records.ToArray();
		}

		/// <summary>
		/// Pauses the current task, starts the specified one.
		/// </summary>
		/// <param name="entityID">Identifier of the task that is a user starts working on.</param>
		public void Start(int entityID)
		{
			lock (_repository)
			{
				TimeTracking timeTracking = ReadTimeTracking();
				if (timeTracking.CurrentRecord != null)
				{
					_stopWatch.Stop();
					timeTracking.CurrentRecord.Ended = timeTracking.CurrentRecord.Started + _stopWatch.Elapsed;
					timeTracking.Records.Add(timeTracking.CurrentRecord);
					if (OnStop != null)
					{
						OnStop(this, new StopWatchEventArgs(timeTracking.CurrentRecord));
					}
				}
				_stopWatch.Start();
				timeTracking.CurrentRecord = new TimeRecord(entityID, DateTime.Now);
				if (OnStart != null)
				{
					OnStart(this, new StopWatchEventArgs(timeTracking.CurrentRecord));
				}
				StoreTimeTracking(timeTracking);
			}
		}

		public void Resume()
		{
			lock (_repository)
			{
				TimeTracking timeTracking = ReadTimeTracking();
				if (timeTracking.CurrentRecord != null)
				{
					_stopWatch.Start();
				}
				if (OnStart != null)
				{
					OnStart(this, new StopWatchEventArgs(timeTracking.CurrentRecord));
				}
			}
		}

		/// <summary>
		/// Stops time interval measurement and resets the elapsed time to zero.
		/// </summary>
		public void Reset()
		{
			_stopWatch.Reset();
		}

		public void Stop()
		{
			TimeTracking timeTracking = ReadTimeTracking();
			if (timeTracking.CurrentRecord != null)
			{
				_stopWatch.Stop();
				timeTracking.CurrentRecord.Ended = timeTracking.CurrentRecord.Started + _stopWatch.Elapsed;
			}
		}

		/// <summary>
		/// Stops recording time for the current item.
		/// </summary>
		public void Stop(string description)
		{
			lock (_repository)
			{
				TimeTracking timeTracking = ReadTimeTracking();
				if (timeTracking.CurrentRecord != null)
				{
					_stopWatch.Stop();
					timeTracking.CurrentRecord.Ended = timeTracking.CurrentRecord.Started + _stopWatch.Elapsed;
					timeTracking.CurrentRecord.Description = description;
					timeTracking.Records.Add(timeTracking.CurrentRecord);
					if (OnStop != null)
					{
						OnStop(this, new StopWatchEventArgs(timeTracking.CurrentRecord));
					}
					timeTracking.CurrentRecord = null;
					_stopWatch.Reset();
				}
				StoreTimeTracking(timeTracking);
			}
		}

		public void Delete(TimeRecord timeRecord)
		{
			lock (_repository)
			{
				TimeTracking timeTracking = ReadTimeTracking();
				timeTracking.Records.Remove(timeRecord);
				StoreTimeTracking(timeTracking);
			}
		}

		public void ClearLog()
		{
			lock (_repository)
			{
				StoreTimeTracking(new TimeTracking());
			}
		}

		private TimeTracking ReadTimeTracking()
		{
			if (_timeTracking == null || _repository.LastUpdated > _lastUpdated)
			{
				_lastUpdated = _repository.LastUpdated;
				_repository.Read(out _timeTracking);
			}
			return _timeTracking;
		}

		private void StoreTimeTracking(TimeTracking timeTracking)
		{
			_repository.Store(timeTracking);
			_timeTracking = timeTracking;
			_lastUpdated = DateTime.Now;
		}

		#region IDisposable Members

		public void Dispose()
		{
			_repository.Dispose();
		}

		#endregion
	}

	[Serializable]
	public class TimeTracking
	{
		private List<TimeRecord> _records = new List<TimeRecord>();

		public TimeRecord CurrentRecord { get; set; }

		public List<TimeRecord> Records
		{
			get { return _records; }
			set { _records = value; }
		}

		#region Equals and GetHashCode

		public override string ToString()
		{
			var s = new StringBuilder();
			ToString(s);
			return s.ToString();
		}

		public void ToString(StringBuilder s)
		{
			if (CurrentRecord != null)
			{
				s.Append("current={");
				CurrentRecord.ToString(s);
				s.Append("}");
			}
			int n = 0;
			foreach (TimeRecord record in Records)
			{
				if (n > 0)
				{
					s.Append("; ");
				}
				s.Append("{");
				record.ToString(s);
				s.Append("}");
				n++;
			}
		}

		#endregion
	}

	[Serializable]
	public class TimeRecord : IEquatable<TimeRecord>
	{
		public TimeRecord() { }

		public TimeRecord(int entityId, DateTime started)
		{
			AssignableID = entityId;
			Started = started;
		}

		public TimeRecord(int entityID, DateTime started, DateTime? ended)
		{
			AssignableID = entityID;
			Started = started;
			Ended = ended;
		}

		/// <summary>
		/// Gets assignable entity ID.
		/// </summary>
		public int AssignableID { get; set; }

		/// <summary>
		/// Gets time when starged working on the assignable.
		/// </summary>
		public DateTime Started { get; set; }

		/// <summary>
		/// Gets time when ended working on the assignable.
		/// </summary>
		public DateTime? Ended { get; set; }

		/// <summary>
		/// Gets time interval description.
		/// </summary>
		public string Description { get; set; }

		#region Object Methods

		public bool Equals(TimeRecord that)
		{
			return that != null && AssignableID == that.AssignableID && Started == that.Started && Ended == that.Ended;
		}

		public override bool Equals(object that)
		{
			return ReferenceEquals(this, that) || Equals(that as TimeRecord);
		}

		public override int GetHashCode()
		{
			int result = AssignableID;
			result = 29 * result + Started.GetHashCode();
			result = 29 * result + Ended.GetHashCode();
			return result;
		}

		public static bool operator ==(TimeRecord a, TimeRecord b)
		{
			return Equals(a, b);
		}

		public static bool operator !=(TimeRecord a, TimeRecord b)
		{
			return !Equals(a, b);
		}

		#endregion

		#region ToString

		public override string ToString()
		{
			var s = new StringBuilder();
			ToString(s);
			return s.ToString();
		}

		public void ToString(StringBuilder s)
		{
			s.AppendFormat("ID={0}", AssignableID);
			s.AppendFormat("; started={0}", Started);
			if (Ended != null)
			{
				s.AppendFormat("; ended={0}", Ended);
			}
		}

		#endregion
	}
}