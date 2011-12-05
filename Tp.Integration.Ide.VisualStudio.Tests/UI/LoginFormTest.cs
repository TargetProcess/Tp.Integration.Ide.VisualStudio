using System.Windows.Forms;
using NUnit.Framework;

namespace Tp.Integration.Ide.VisualStudio.UI {
	[TestFixture]
	[Explicit]
	public class LoginFormTest {
		[Test]
		public void ShowForm() {
			Application.EnableVisualStyles();
			var form = new LoginForm(new Settings());
			form.ShowDialog();
		}
	}
}