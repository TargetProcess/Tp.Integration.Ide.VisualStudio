using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Tp.Integration.Ide.VisualStudio.Services;

namespace Tp.Integration.Ide.VisualStudio.UI {
	public partial class TimeLogForm : Form {
		public TimeLogForm(IEnumerable<TimeRecord> log, Dictionary<int, string> names) {
			InitializeComponent();

			var control = new TimeLogChartControl {
				Dock = DockStyle.Fill,
				Names = names,
				TimeRecords = log,
			};

			Controls.Add(control);

			control.DoubleClick += (sender, e) => { Close(); };
			control.KeyPress += (sender, e) => {
				if (e.KeyChar == 27) {
					Close();
				}
			};
		}
	}
}