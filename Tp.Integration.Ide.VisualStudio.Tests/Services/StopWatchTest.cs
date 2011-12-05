// 
// Copyright (c) 2005-2011 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 

using System;
using System.IO;
using NUnit.Framework;

namespace Tp.Integration.Ide.VisualStudio.Services
{
	[TestFixture]
	public class StopWatchTest
	{
		[Test]
		public void StartStop()
		{
			var stopWatch = new StopWatch(new DummyRepository());

			Assert.AreEqual(0, stopWatch.GetLog().Length);
			stopWatch.Start(1);
			Assert.AreEqual(0, stopWatch.GetLog().Length);
			stopWatch.Start(1);
			Assert.AreEqual(1, stopWatch.GetLog().Length);
			stopWatch.Start(2);
			Assert.AreEqual(2, stopWatch.GetLog().Length);
			stopWatch.Stop("Done");
			Assert.AreEqual(3, stopWatch.GetLog().Length);

			stopWatch.Delete(stopWatch.GetLog()[0]);
			Assert.AreEqual(2, stopWatch.GetLog().Length);
			stopWatch.Delete(stopWatch.GetLog()[0]);
			Assert.AreEqual(1, stopWatch.GetLog().Length);
			stopWatch.Delete(stopWatch.GetLog()[0]);
			Assert.AreEqual(0, stopWatch.GetLog().Length);
		}

		[Test(Description = "Time records survive between time tracker restarts")]
		public void IntegrationTest()
		{
			var fileName = Path.Combine(Path.GetTempPath(), string.Format("tmp{0}.xml", Environment.TickCount));
			using (var stopWatch = new StopWatch(new FileSystemTimeTrackingRepository(fileName)))
			{
				Assert.AreEqual(0, stopWatch.GetLog().Length);
				Assert.AreEqual(null, stopWatch.GetCurrent());
				stopWatch.Start(1);
				Assert.AreEqual(0, stopWatch.GetLog().Length);
				Assert.AreEqual(1, stopWatch.GetCurrent().AssignableID);
			}
			using (var stopWatch = new StopWatch(new FileSystemTimeTrackingRepository(fileName)))
			{
				Assert.AreEqual(0, stopWatch.GetLog().Length);
				Assert.AreEqual(1, stopWatch.GetCurrent().AssignableID);
				stopWatch.Stop("Done");
				Assert.AreEqual(1, stopWatch.GetLog().Length);
				Assert.AreEqual(null, stopWatch.GetCurrent());
			}
			using (var stopWatch = new StopWatch(new FileSystemTimeTrackingRepository(fileName)))
			{
				Assert.AreEqual(1, stopWatch.GetLog().Length);
				Assert.AreEqual(null, stopWatch.GetCurrent());
				stopWatch.Start(2);
				Assert.AreEqual(1, stopWatch.GetLog().Length);
				Assert.AreEqual(2, stopWatch.GetCurrent().AssignableID);
			}
			using (var stopWatch = new StopWatch(new FileSystemTimeTrackingRepository(fileName)))
			{
				Assert.AreEqual(1, stopWatch.GetLog().Length);
				Assert.AreEqual(2, stopWatch.GetCurrent().AssignableID);
				stopWatch.Stop("Done");
				Assert.AreEqual(2, stopWatch.GetLog().Length);
				Assert.AreEqual(null, stopWatch.GetCurrent());
			}
			File.Delete(fileName);
		}

		private class DummyRepository : ITimeTrackingRepository
		{
			private DateTime _lastUpdate = DateTime.Now;

			private TimeTracking _timeTracking;

			#region ITimeTrackingRepository Members

			public void Store(TimeTracking timeTracking)
			{
				_timeTracking = timeTracking;
				_lastUpdate = DateTime.Now;
			}

			public void Read(out TimeTracking timeTracking)
			{
				timeTracking = _timeTracking ?? new TimeTracking();
			}

			public DateTime LastUpdated
			{
				get { return _lastUpdate; }
			}

			public event EventHandler<RepositoryUpdatedEventArgs> OnUpdated
			{
				add { }
				remove { }
			}

			#endregion

			#region IDisposable Members

			public void Dispose()
			{
				//
			}

			#endregion
		}
	}
}