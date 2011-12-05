using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NUnit.Framework;
using Tp.Integration.Ide.VisualStudio.Services;

namespace Tp.Integration.Ide.VisualStudio.UI {
	[TestFixture]
	[Explicit]
	public class TimeLogChartControlTest {
		[Test]
		public void NullLog() {
			var form = new Form {
				Text = "Test",
				Width = 600,
				Height = 300,
				Font = new Font("Times New Roman", 12, FontStyle.Regular, GraphicsUnit.Point),
			};
			var ctl = new TimeLogChartControl {
				Dock = DockStyle.Fill,
				TimeRecords = null,
			};
			form.Controls.Add(ctl);
			form.Closed += (sender, e) => { Application.Exit(); };
			Application.Run(form);
		}

		[Test]
		public void EmptyLog() {
			var form = new Form {
				Text = "Test",
				Width = 600,
				Height = 300,
				Font = new Font("Times New Roman", 12, FontStyle.Regular, GraphicsUnit.Point),
			};
			var timeTracking = new TimeTracking();
			var ctl = new TimeLogChartControl {
				Dock = DockStyle.Fill,
				TimeRecords = timeTracking.Records,
			};
			form.Controls.Add(ctl);
			form.Closed += (sender, e) => { Application.Exit(); };
			Application.Run(form);
		}

		[Test]
		public void ShortLog() {
			var form = new Form {
				Text = "Test",
				Width = 600,
				Height = 300,
				Font = new Font("Times New Roman", 12, FontStyle.Regular, GraphicsUnit.Point),
			};
			var timeTracking = new TimeTracking();
			timeTracking.Records.Add(new TimeRecord {
				AssignableID = 5,
				Started = Time(11, 8),
				Ended = Time(11, 9),
				Description = "First task completed"
			});
			var ctl = new TimeLogChartControl {
				Dock = DockStyle.Fill,
				TimeRecords = timeTracking.Records,
			};
			form.Controls.Add(ctl);
			form.Closed += (sender, e) => { Application.Exit(); };
			Application.Run(form);
		}

		[Test]
		public void NormalLog() {
			var form = new Form {
				Text = "Test",
				Width = 600,
				Height = 300,
				Font = new Font("Arial", 12, FontStyle.Regular, GraphicsUnit.Point),
			};
			var timeTracking = new TimeTracking();
			timeTracking.Records.Add(new TimeRecord {
				AssignableID = 5,
				Started = Time(10, 8),
				Ended = Time(11, 23),
				Description = "First task completed"
			});
			timeTracking.Records.Add(new TimeRecord {
				AssignableID = 3,
				Started = Time(11, 49),
				Ended = Time(12, 23),
				Description = "First task completed"
			});
			timeTracking.Records.Add(new TimeRecord {
				AssignableID = 4,
				Started = Time(13, 1),
				Ended = Time(13, 23),
				Description = "First task completed"
			});
			timeTracking.Records.Add(new TimeRecord {
				AssignableID = 5,
				Started = Time(13, 23),
				Ended = Time(15, 59),
				Description = "First task completed"
			});
			timeTracking.Records.Add(new TimeRecord {
				AssignableID = 6,
				Started = Time(16, 0),
				Ended = Time(17, 28),
				Description = "First task completed"
			});
			var ctl = new TimeLogChartControl {
				Dock = DockStyle.Fill,
				Names = new Dictionary<int, string> {
					{3, "Simple task"},
					{4, "Moderate task"},
					{5, "Complex task with a very long name that does not fit to a header cell and needs to be wrapped or a tool tip needs to be displayed for it."},
				},
				TimeRecords = timeTracking.Records,
			};
			form.Controls.Add(ctl);
			form.Closed += (sender, e) => { Application.Exit(); };
			Application.Run(form);
		}

		private static DateTime Time(int hours, int minutes) {
			return new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hours, minutes, 0);
		}
	}
}