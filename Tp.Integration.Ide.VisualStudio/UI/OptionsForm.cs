using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Tp.Integration.Ide.VisualStudio.UI {
	internal sealed partial class OptionsForm : Form {
		public OptionsForm(Settings settings) {
			InitializeComponent();

			_cbAutoLogin.DataBindings.Add(new Binding("Checked", settings, "AutoLogin"));
			_cbAutoRefresh.DataBindings.Add(new Binding("Checked", settings, "AutoRefresh"));
			_tbAutoRefreshInterval.DataBindings.Add(new Binding("Value", settings, "AutoRefreshInterval"));
			_cbStopTrackingOnVisualStudioClose.DataBindings.Add(new Binding("Checked", settings, "StopTrackingOnVisualStudioClose"));
			_cbStopTrackingOnUserIdle.DataBindings.Add(new Binding("Checked", settings, "StopTrackingOnUserIdle"));

			_tbAutoRefreshInterval.DataBindings.Add(new Binding("Enabled", _cbAutoRefresh, "Checked"));

			_cbStopTrackingOnUserIdle.Visible = false; // Hide temporarily
		}
	}
}