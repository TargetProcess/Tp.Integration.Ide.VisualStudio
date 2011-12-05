using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Tp.Integration.Ide.VisualStudio.Services {
	[TestFixture]
	[Explicit]
	public class WebServicesIntegrationTest {
		[Test]
		public void TestServices() {
			var settings = new Settings { Uri = new Uri("http://localhost/tp2"), Login = "admin", DecryptedPassword = "admin" };
			var webServices = new WebServices(settings);
			webServices.Authenticate();

			var assigments = webServices.GetMyAssigments();
			Assert.IsNotNull(assigments);
			foreach (var assignable in assigments.Assignables) {
				Console.WriteLine("assignable={0}", assignable);
			}

			{
				var assignables = webServices.GetAssignables();
				Assert.IsNotNull(assignables);
				foreach (var assignable in assignables) {
					Console.WriteLine("assignable={0}", assignable);
				}
			}
			{
				var assignables = webServices.GetAssignables(1, 2, 3, 4, 5);
				Assert.IsNotNull(assignables);
				foreach (var assignable in assignables) {
					Console.WriteLine("assignable={0}", assignable);
				}
			}
		}
	}
}