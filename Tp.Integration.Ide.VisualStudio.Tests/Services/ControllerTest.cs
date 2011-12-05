// 
// Copyright (c) 2005-2011 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 

using System;
using System.Diagnostics;
using System.Windows.Forms;
using NUnit.Framework;
using Tp.AssignableServiceProxy;
using Tp.MyAssignmentsServiceProxy;

namespace Tp.Integration.Ide.VisualStudio.Services
{
	[TestFixture]
	public class ControllerTest
	{
		private Settings _settings;

		[SetUp]
		public void SetUp()
		{
			_settings = new Settings();
		}

		[TearDown]
		public void TearDown()
		{
			_settings.Save();
		}

		[Test]
		public void TestOptions()
		{
			var env = new DummyControllerEnvironment();
			using (var ctl = new Controller(new DummyTimeTrackingRepository(), new DummyWebServicesFactory(), env))
			{
				env.DialogResult = DialogResult.OK;
				ctl.Options();
				env.DialogResult = DialogResult.Cancel;
				ctl.Options();
			}
		}

		[Test]
		public void TestConnectDisconnect()
		{
			var settings = new Settings {Uri = new Uri("http://localhost"), Login = "admin", DecryptedPassword = "admin",};
			settings.Save();
			var env = new DummyControllerEnvironment();
			Controller ctl;
			using (ctl = new Controller(new DummyTimeTrackingRepository(), new DummyWebServicesFactory(), env))
			{
				Assert.IsFalse(ctl.Connected);
				env.DialogResult = DialogResult.OK;
				ctl.Connect(true);
				Assert.IsTrue(ctl.Connected);
				ctl.Disconnect();
				Assert.IsFalse(ctl.Connected);
				env.DialogResult = DialogResult.Cancel;
				ctl.Connect(true);
				Assert.IsFalse(ctl.Connected);
				env.DialogResult = DialogResult.OK;
				ctl.Connect(true);
				Assert.IsTrue(ctl.Connected);
			}
			Assert.IsFalse(ctl.Connected);
		}

		[Test]
		[ExpectedException(typeof (InvalidOperationException))]
		public void TestConnect()
		{
			var env = new DummyControllerEnvironment();
			var ctl = new Controller(new DummyTimeTrackingRepository(), new DummyWebServicesFactory(), env);
			Assert.IsFalse(ctl.Connected);
			ctl.Disconnect();
		}

		[Test]
		[ExpectedException(typeof (InvalidOperationException))]
		public void TestDisconnect()
		{
			var env = new DummyControllerEnvironment();
			var ctl = new Controller(new DummyTimeTrackingRepository(), new DummyWebServicesFactory(), env);
			Assert.IsFalse(ctl.Connected);
			env.DialogResult = DialogResult.OK;
			ctl.Connect(true);
			Assert.IsTrue(ctl.Connected);
			ctl.Connect(true);
		}

		private class DummyTimeTrackingRepository : ITimeTrackingRepository
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

		private class DummyWebServicesFactory : IWebServicesFactory
		{
			#region IWebServicesFactory Members

			public IWebServices CreateWebServices(Settings settings)
			{
				return new DummyWebServices(settings);
			}

			#endregion
		}

		private class DummyWebServices : IWebServices
		{
			public DummyWebServices(Settings settings)
			{
			}

			#region IDisposable Members

			public void Dispose()
			{
				//
			}

			#endregion

			#region IWebServices Members

			public void Authenticate()
			{
				//
			}

			public MyAssignments GetMyAssigments()
			{
				var assigments = new MyAssignments
				                 	{
				                 		Assignables = new[]
				                 		              	{
				                 		              		new AssignableSimpleDTO
				                 		              			{
				                 		              				AssignableID = 1,
				                 		              				Name = "Task 1",
				                 		              			}
				                 		              	}
				                 	};
				return assigments;
			}

			public void ChangeState(AssignableSimpleDTO assignable, EntityStateDTO entityState)
			{
				//
			}

			public SubmitTimeError[] SubmitTime(TimeSimpleDTO[] times)
			{
				return new SubmitTimeError[] {};
			}

			public AssignableDTO[] GetAssignables(params int[] id)
			{
				return new[]
				       	{
				       		new AssignableDTO
				       			{
				       				AssignableID = 1,
				       				Name = "Asignable 1",
				       			}
				       	};
			}

			#endregion
		}

		private class DummyControllerEnvironment : IControllerEnvironment
		{
			private static readonly ConsoleTraceListener _listener = new ConsoleTraceListener();
			private DialogResult _dialogResult = DialogResult.OK;

			public DialogResult DialogResult
			{
				set { _dialogResult = value; }
			}

			#region IControllerEnvironment Members

			public DialogResult ShowDialog(Form form)
			{
				return _dialogResult;
			}

			public DialogResult ShowMessageBox(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
			{
				return DialogResult.OK;
			}

			public TraceListener Trace
			{
				get { return _listener; }
			}

			#endregion
		}
	}
}