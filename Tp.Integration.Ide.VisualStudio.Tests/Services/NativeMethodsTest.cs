using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Tp.Integration.Ide.VisualStudio.Services {
	[TestFixture]
	public class NativeMethodsTest {
		[Test]
		public void TestNativeMethods() {
			Console.WriteLine("last input time={0}", NativeMethods.GetLastInputTime()); // should not fail
			Console.WriteLine("idle time={0}", NativeMethods.GetIdleTime()); // should not fail
		}
	}
}