using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Tp.Integration.Ide.VisualStudio.Services {
	[TestFixture]
	public class IdleTrackerTest {
		[Test]
		public void TestConstructor() {
			using (var tracker = new IdleTracker(1, 1)) {
				tracker.OnIdle += idleTrackerEventArgs => { };
			}
		}
	}
}