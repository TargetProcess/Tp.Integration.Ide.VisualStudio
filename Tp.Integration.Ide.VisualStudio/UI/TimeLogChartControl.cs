using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using Tp.Integration.Ide.VisualStudio.Services;

namespace Tp.Integration.Ide.VisualStudio.UI {
	public class TimeLogChartControl : ScrollableControl {
		private const int HeaderHeight = 30;
		private const int HeaderWidth = 100;
		private const int RowHeight = 25;
		private const int CellWidth = 90;

		private readonly Point _offset = new Point(HeaderWidth, HeaderHeight);

		private readonly Font _font;

		private readonly ToolTip _toolTip;

		private Dictionary<int, string> _names = new Dictionary<int, string>();

		private Table _table;

		private Cell _cell;

		public TimeLogChartControl() {
			DoubleBuffered = true;
			BackColor = Color.White; // SystemColors.Window;
			ForeColor = Color.Black; // SystemColors.WindowText;
			AutoScroll = true;
			MouseMove += OnMouseMove;
			_font = new Font(Font.Name, 8, FontStyle.Regular, GraphicsUnit.Point);
			_toolTip = new ToolTip {OwnerDraw = true};
			_toolTip.Popup += ToolTip_Popup;
			_toolTip.Draw += ToolTip_Draw;
		}

		/// <summary>
		/// Sets dictionary of assignable item names.
		/// </summary>
		public Dictionary<int, string> Names {
			set { _names = value ?? new Dictionary<int, string>(); }
		}

		/// <summary>
		/// Sets array of time records.
		/// </summary>
		public IEnumerable<TimeRecord> TimeRecords {
			set {
				if (value != null) {
					_table = new Table(value);
					_table.Layout(_offset);
					AutoScrollMinSize = _table.Size;
				}
				else {
					_table = null;
					AutoScrollMinSize = Size.Empty;
				}
			}
		}

		private void OnMouseMove(object sender, MouseEventArgs e) {
			if (_table != null) {
				Cell cell = _table.GetCell(new Point(e.X - AutoScrollPosition.X, e.Y - AutoScrollPosition.Y));
				if (cell != _cell) {
					_cell = cell;
					if (_cell != null && !string.IsNullOrEmpty(_cell.Description)) {
						_toolTip.Show(_cell.Description, this, AutoScrollPosition.X + _cell.Left, AutoScrollPosition.Y + _cell.Row.Top);
					}
					else {
						_toolTip.Hide(this);
					}
				}
			}
		}

		private void ToolTip_Popup(object sender, PopupEventArgs e) {
			using (Graphics g = Graphics.FromHwnd(Handle)) {
				int width = (int) Math.Ceiling(g.MeasureString(_cell.Description, _font).Width) + 1;
				e.ToolTipSize = new Size(Math.Max(width, _cell.Width), RowHeight);
			}
		}

		private void ToolTip_Draw(object sender, DrawToolTipEventArgs e) {
			using (var brush = new SolidBrush(Color.Orange)) {
				e.Graphics.FillRectangle(brush, e.Bounds);
			}
			using (var brush = new SolidBrush(ForeColor)) {
				e.Graphics.DrawString(e.ToolTipText, _font, brush, 0, 0);
			}
		}

		protected override void OnPaint(PaintEventArgs e) {
			base.OnPaint(e);
			if (_table != null) {
				e.Graphics.TranslateTransform(AutoScrollPosition.X, AutoScrollPosition.Y);
				DrawItems(e.Graphics);
				DrawGrid(e.Graphics);
				DrawLabels(e.Graphics);
				e.Graphics.ResetTransform();
			}
		}

		private void DrawGrid(Graphics g) {
			// Fill header background.
			using (var brush = new SolidBrush(Color.LightGoldenrodYellow)) {
				var rect = new Rectangle(-AutoScrollPosition.X, 0, Width, HeaderHeight);
				g.FillRectangle(brush, rect);
			}

			// Draw vertical and horizontal grid lines.
			using (var pen = new Pen(Color.Silver)) {
				pen.DashStyle = DashStyle.Dash;
				// horizontal
				for (var n = 0; n < _table.Rows.Length + 1; n++) {
					var top = _table.GetRowOffset(n);
					var pt1 = new Point(-AutoScrollPosition.X, top);
					var pt2 = new Point(-AutoScrollPosition.X + Width, top);
					g.DrawLine(pen, pt1, pt2);
				}
				// vertical
				for (var n = 0; n < _table.End.Hour - _table.Start.Hour + 1; n++) {
					var left = _table.GetColOffset(n);
					var pt1 = new Point(left, -AutoScrollPosition.Y);
					var pt2 = new Point(left, -AutoScrollPosition.Y + Height);
					g.DrawLine(pen, pt1, pt2);
				}
			}

			// Draw time scale labels.
			using (var brush = new SolidBrush(ForeColor)) {
				using (var font = new Font(Font.Name, 14, FontStyle.Regular, GraphicsUnit.Point)) {
					var top = (HeaderHeight - font.Height)/2;
					for (var n = 0; n < _table.End.Hour - _table.Start.Hour; n++) {
						var left = _table.GetColOffset(n);
						var text = string.Format("{0:00}:00", n + _table.Start.Hour);
						g.DrawString(text, font, brush, left, top);
					}
				}
			}
		}

		private void DrawItems(Graphics g) {
			using (var brush = new SolidBrush(Color.LightGreen)) {
				foreach (var row in _table.Rows) {
					foreach (var cell in row.Cells) {
						var rect = new Rectangle(cell.Left, row.Top, cell.Width, row.Height);
						g.FillRectangle(brush, rect);
					}
				}
			}
		}

		private void DrawLabels(Graphics g) {
			using (var brush = new SolidBrush(ForeColor)) {
				foreach (var row in _table.Rows) {
					{
						var name = "";
						if (_names.TryGetValue(row.AssignableID, out name) == false) {
							name = "Unknown";
						}
						var rect = new Rectangle(0, row.Top, HeaderWidth, _font.Height);
						var text = string.Format("#{0} - {1}", row.AssignableID, name);
						var format = new StringFormat { Trimming = StringTrimming.EllipsisCharacter };
						g.DrawString(text, _font, brush, rect, format);
					}
					foreach (var cell in row.Cells) {
						var rect = new Rectangle(cell.Left, row.Top, cell.Width, _font.Height);
						if (rect.Width > 10 && !string.IsNullOrEmpty(cell.Description)) {
							var text = cell.Description;
							var format = new StringFormat {Trimming = StringTrimming.EllipsisCharacter};
							g.DrawString(text, _font, brush, rect, format);
						}
					}
				}
			}
		}

		/// <summary>
		/// Position rows and cells on the stage accordint to start and end time.
		/// </summary>
		private class Table {
			private readonly List<Row> _rows = new List<Row>();
			private Point _offset;

			public void AddRow(Row row) {
				_rows.Add(row);
				row.Table = this;
			}

			public Row[] Rows {
				get { return _rows.ToArray(); }
			}

			/// <summary>
			/// Convert time records to rows and cells.
			/// </summary>
			/// <param name="timeRecords">Time records to parse.</param>
			/// <returns>Table.</returns>
			public Table(IEnumerable<TimeRecord> timeRecords) {
				var today = DateTime.Now.Date;
				var rows = new Dictionary<int, Row>();
				foreach (var timeRecord in timeRecords) {
					Row row;
					if (rows.ContainsKey(timeRecord.AssignableID)) {
						row = rows[timeRecord.AssignableID];
					}
					else {
						rows.Add(timeRecord.AssignableID, row = new Row(timeRecord.AssignableID));
					}
					if (timeRecord.Started.Date == today && timeRecord.Ended.Value.Date == today) {
						row.AddCell(new Cell(timeRecord.Started, timeRecord.Ended.Value, timeRecord.Description));
					}
				}
				foreach (var item in rows) {
					AddRow(item.Value);
				}
			}

			/// <summary>
			/// Position rows and cells on the stage according to times.
			/// </summary>
			/// <param name="offset">Offset to top left corner.</param>
			public void Layout(Point offset) {
				_offset = offset;

				var start = DateTime.MaxValue;
				foreach (var row in _rows) {
					foreach (var cell in row.Cells) {
						if (cell.Started < start) {
							start = cell.Started;
						}
					}
				}
				Start = new DateTime(start.Year, start.Month, start.Day, start.Hour, 0, 0);

				var end = DateTime.MinValue;
				foreach (var row in _rows) {
					foreach (var cell in row.Cells) {
						if (cell.Ended > end) {
							end = cell.Ended;
						}
					}
				}
				End = new DateTime(end.Year, end.Month, end.Day, end.Hour + 1, 0, 0);

				var n = 0;
				foreach (var row in Rows) {
					row.Top = offset.Y + n*RowHeight;
					row.Height = RowHeight;
					foreach (var cell in row.Cells) {
						var left = GetTimeOffset(cell.Started);
						var width = GetTimeOffset(cell.Ended) - left;
						cell.Left = offset.X + left;
						cell.Width = Math.Max(width, 1);
					}
					n++;
				}
			}

			private int GetTimeOffset(DateTime dateTime) {
				return (int) ((dateTime - Start).TotalMinutes/60.0*CellWidth);
			}

			public int GetRowOffset(int n) {
				return _offset.Y + n*RowHeight;
			}

			public int GetColOffset(int n) {
				return _offset.X + n*CellWidth;
			}

			public Size Size {
				get {
					return new Size(
						_offset.X + (End.Hour - Start.Hour)*CellWidth,
						_offset.Y + Rows.Length*RowHeight);
				}
			}

			/// <summary>
			/// Finds cell under cursor position.
			/// </summary>
			/// <param name="p">Cursor position under which to find cell.</param>
			/// <returns>A cell whose rectangle contains the specified cursor position, or <c>null</c> in none.</returns>
			public Cell GetCell(Point p) {
				foreach (var row in Rows) {
					if (p.Y >= row.Top && p.Y <= row.Top + row.Height) {
						foreach (var cell in row.Cells) {
							if (p.X >= cell.Left && p.X <= cell.Left + cell.Width) {
								return cell;
							}
						}
					}
				}
				return null;
			}

			/// <summary>
			/// Gets earliest time.
			/// </summary>
			public DateTime Start { get; private set; }

			/// <summary>
			/// Gets latest time.
			/// </summary>
			public DateTime End { get; private set; }
		}

		private class Row {
			private readonly int _assignableID;
			private readonly List<Cell> _cells = new List<Cell>();

			public Row(int assignableID) {
				_assignableID = assignableID;
			}

			public Table Table { get; set; }

			public int AssignableID {
				get { return _assignableID; }
			}

			public void AddCell(Cell cell) {
				_cells.Add(cell);
				cell.Row = this;
			}

			public Cell[] Cells {
				get { return _cells.ToArray(); }
			}

			public int Top { get; set; }
			public int Height { get; set; }
		}

		private class Cell {
			private readonly DateTime _started;
			private readonly DateTime _ended;
			private readonly string _description;

			public Cell(DateTime started, DateTime ended, string description) {
				_started = started;
				_ended = ended;
				_description = description;
			}

			public Row Row { get; set; }

			public DateTime Started {
				get { return _started; }
			}

			public DateTime Ended {
				get { return _ended; }
			}

			public string Description {
				get { return _description; }
			}

			public int Left { get; set; }
			public int Width { get; set; }
		}
	}
}