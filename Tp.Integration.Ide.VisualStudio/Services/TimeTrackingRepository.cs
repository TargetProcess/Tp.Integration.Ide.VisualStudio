using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Tp.Integration.Ide.VisualStudio.Utils;

namespace Tp.Integration.Ide.VisualStudio.Services {
	/// <summary>
	/// Carries data about time tracking repository change.
	/// </summary>
	public class RepositoryUpdatedEventArgs : EventArgs {
		private readonly DateTime _lastUpdated;

		/// <summary>
		/// Creates new instance of this class.
		/// </summary>
		/// <param name="lastUpdated">
		/// Time when time tracking eepository was changed.
		/// </param>
		public RepositoryUpdatedEventArgs(DateTime lastUpdated) {
			_lastUpdated = lastUpdated;
		}

		/// <summary>
		/// Time when time tracking repository was changed.
		/// </summary>
		public DateTime LastUpdated {
			get { return _lastUpdated; }
		}
	}

	/// <summary>
	/// Stores stopwatch repository on disk and reads it back on demand.
	/// </summary>
	public interface ITimeTrackingRepository : IDisposable {
		/// <summary>
		/// Stores the specified time tracking repository to disk.
		/// </summary>
		/// <param name="timeTracking">Time tracking repository to store.</param>
		/// <exception cref="IOException">If I/O error occurs.</exception>
		void Store(TimeTracking timeTracking);

		/// <summary>
		/// Reads previously stored data from disk into the specified time tracking repository.
		/// </summary>
		/// <param name="timeTracking">Time tracking repository read from disk.</param>
		/// <exception cref="IOException">If I/O error occurs.</exception>
		void Read(out TimeTracking timeTracking);

		/// <summary>
		/// Gets timestamp of the last update.
		/// </summary>
		DateTime LastUpdated { get; }

		/// <summary>
		/// Notifies when repository is updated externally, probably by another addin instance.
		/// </summary>
		event EventHandler<RepositoryUpdatedEventArgs> OnUpdated;
	}

	public sealed class FileSystemTimeTrackingRepository : ITimeTrackingRepository {
		private readonly string _path;
		private readonly FileSystemWatcher _fileSystemWatcher;

		public FileSystemTimeTrackingRepository(string file) {
			_path = file;
			_fileSystemWatcher = new FileSystemWatcher {
				Path = Path.GetDirectoryName(_path),
				Filter = Path.GetFileName(_path),
				NotifyFilter = NotifyFilters.LastWrite,
			};
			_fileSystemWatcher.Created += OnFileChanged;
			_fileSystemWatcher.Changed += OnFileChanged;
			_fileSystemWatcher.Deleted += OnFileChanged;
			_fileSystemWatcher.EnableRaisingEvents = true;
		}

		private void OnFileChanged(object sender, FileSystemEventArgs e) {
			if (OnUpdated != null) {
				OnUpdated(this, new RepositoryUpdatedEventArgs(LastUpdated));
			}
		}

		#region ITimeTrackingRepository Members

		public void Store(TimeTracking timeTracking) {
			_fileSystemWatcher.EnableRaisingEvents = false;
			var w = new StringWriter();
			Serializer.Serialize(timeTracking, w);
			File.WriteAllText(_path, w.ToString());
			_fileSystemWatcher.EnableRaisingEvents = true;
		}

		public void Read(out TimeTracking timeTracking) {
			timeTracking = File.Exists(_path) ? Serializer.Deserialize<TimeTracking>(File.ReadAllText(_path)) : new TimeTracking();
		}

		public DateTime LastUpdated {
			get { return File.Exists(_path) ? File.GetLastWriteTime(_path) : DateTime.MinValue; }
		}

		public event EventHandler<RepositoryUpdatedEventArgs> OnUpdated;

		#endregion

		#region IDisposable Members

		public void Dispose() {
			_fileSystemWatcher.Dispose();
		}

		#endregion
	}
}