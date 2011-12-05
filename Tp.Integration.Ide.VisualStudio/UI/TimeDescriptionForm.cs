using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Tp.Integration.Ide.VisualStudio.UI {
	internal sealed partial class TimeDescriptionForm : Form {
		public TimeDescriptionForm() {
			InitializeComponent();
		}

		public string Description {
			get {
				string s = textBox1.Text.Trim();
				return s == "" ? null : s;
			}
		}
	}
}