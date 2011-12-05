// 
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using EnvDTE80;
using Tp.Integration.Ide.VisualStudio.Services;
using Tp.Integration.Ide.VisualStudio.Utils;
using Tp.MyAssignmentsServiceProxy;

namespace Tp.Integration.Ide.VisualStudio.UI
{
	/// <summary>
	/// Summary description for MyControl.
	/// </summary>
	public sealed partial class ToolWindowControl : ToolWindowControlBase
	{
		private readonly ToDoListSorter _sorter = new ToDoListSorter();

		private DTE2 _application;

		private Controller _controller;

		private ControllerEnvironment _env;

		private Timer _timerClock;

		private Timer _timerRefresh;

		public ToolWindowControl()
		{
			InitializeComponent();

			_lvToDoList.ListViewItemSorter = _sorter;

			Enabled = false;

			_timerClock = new Timer(components) {Interval = 1000, Enabled = false};
			_timerClock.Tick += ClockTimer_Tick;

			_timerRefresh = new Timer(components) {Interval = 1000*60, Enabled = false};
			_timerRefresh.Tick += RefreshTimer_Tick;
		}

		internal void Init(DTE2 applicationObject, Controller controller, ControllerEnvironment env)
		{
			if (applicationObject == null)
			{
				throw new ArgumentNullException("applicationObject");
			}
			if (controller == null)
			{
				throw new ArgumentNullException("controller");
			}
			if (env == null)
			{
				throw new ArgumentNullException("env");
			}
			if (_application != null)
			{
				throw new InvalidOperationException("Already initialized");
			}

			_application = applicationObject;
			_controller = controller;
			_env = env;

			_controller.OnConnect += Controller_OnConnect;
			_controller.OnDisconnect += Controller_OnDisconnect;

			_controller.OnAssignmentTimeStarted += Controller_OnCurrentAssignmentChanged;
			_controller.OnAssignmentTimeStopped += Controller_OnCurrentAssignmentChanged;

			_controller.OnListRefresh += Controller_OnListRefresh;
		}

		private void Controller_OnConnect(object sender, ConnectEventArgs e)
		{
			_controller.Refresh();
		}

		private void Controller_OnDisconnect(object sender, ConnectEventArgs e)
		{
			Enabled = false;
			_timerClock.Enabled = false;
			_timerRefresh.Enabled = false; // stop auto refresh timer
			_lvToDoList.Items.Clear(); // clear To Do list
			UpdateIndicators();
			UpdateCommands();
		}

		private void Controller_OnCurrentAssignmentChanged(object sender, EventArgs e)
		{
			RefreshToDoList();
			UpdateIndicators();
			UpdateCommands();
		}

		private void Controller_OnListRefresh(object sender, EventArgs e)
		{
			Enabled = true;
			_timerClock.Enabled = true;
			StartAutoRefrestTimer();
			RefreshToDoList();
			UpdateIndicators();
			UpdateCommands();
		}

		private void StartAutoRefrestTimer()
		{
			_timerRefresh.Enabled = false; // stop timer
			var settings = new Settings();
			if (settings.AutoRefresh)
			{
				_timerRefresh.Interval = 1000*60*settings.AutoRefreshInterval;
				_timerRefresh.Enabled = true; // if required, start timer again, this time with new interval
			}
		}

		private void RefreshToDoList()
		{
			AssignableSimpleDTO selectedAssignable = null;
			if (_lvToDoList.SelectedItems.Count == 1)
			{
				var item = _lvToDoList.SelectedItems[0];
				selectedAssignable = item.Tag as AssignableSimpleDTO;
			}
			_lvToDoList.BeginUpdate();
			_lvToDoList.Items.Clear();
			foreach (var assignableSimpleDto in _controller.MyAssignments.Assignables)
			{
				var item = new ListViewItem(Convert.ToString(assignableSimpleDto.ID), assignableSimpleDto.ID.GetValueOrDefault())
				           	{Tag = assignableSimpleDto};
				switch (assignableSimpleDto.EntityTypeAbbreviation)
				{
					case "UserStory":
						item.ImageIndex = 0;
						break;
					case "Task":
						item.ImageIndex = 1;
						break;
					case "Bug":
						item.ImageIndex = 2;
						break;
				}
				item.SubItems.Add(assignableSimpleDto.EntityTypeAbbreviation);
				item.SubItems.Add(assignableSimpleDto.Name);
				item.SubItems.Add(assignableSimpleDto.ProjectName);
				if (assignableSimpleDto.Rank > 0)
				{
					var lvRankItem = new ListViewSubItemEx(Convert.ToString(assignableSimpleDto.Rank));
					var linkLabel = new RankLabel(assignableSimpleDto.Rank, assignableSimpleDto.MaxRank);
					lvRankItem.Image = linkLabel.GetRankImage();
					item.SubItems.Add(lvRankItem);
				}
				else
					item.SubItems.Add(string.Empty);
				item.SubItems.Add(assignableSimpleDto.SeverityName);
				item.SubItems.Add(assignableSimpleDto.EntityStateName);
				item.SubItems.Add(String.Format("{0:F2} h", assignableSimpleDto.TimeSpent ?? 0));
				item.SubItems.Add(String.Format("{0:F2} h", assignableSimpleDto.TimeRemain ?? 0));
				_lvToDoList.Items.Add(item);
				var timeRecord = _controller.GetCurrent();
				if (timeRecord != null && timeRecord.AssignableID == assignableSimpleDto.ID)
				{
					item.Font = new Font(item.Font, FontStyle.Bold);
				}
				if (selectedAssignable != null && assignableSimpleDto.ID == selectedAssignable.ID)
				{
					item.Selected = true;
				}
			}
			_lvToDoList.EndUpdate();
		}

		private void UpdateCommands()
		{
			_btnSubmit.Enabled = _controller.TotalTime.TotalMinutes > 0 && _controller.CurrentTime == null;
			var timeRecord = _controller.GetCurrent();
			if (timeRecord != null)
			{
				_btnPlay.Enabled = true;
				_btnPlay.ImageIndex = 4;
			}
			else
			{
				_btnPlay.Enabled = (_lvToDoList.SelectedItems.Count == 1);
				_btnPlay.ImageIndex = 3;
				if (_controller.IsStarted)
				{
					_controller.Reset();
				}
			}
		}

		private void UpdateIndicators()
		{
			var totalTime = _controller.TotalTime;
			_lblTotalTime.Text = string.Format("{0:00}:{1:00}:{2:00}", totalTime.Hours, totalTime.Minutes, totalTime.Seconds);
			var currentTime = _controller.CurrentTime;
			if (currentTime != null)
			{
				_lblCurrentTime.Text = string.Format("{0:00}:{1:00}:{2:00}", currentTime.Value.Hours, currentTime.Value.Minutes,
				                                     currentTime.Value.Seconds);
				_lblCurrentTime.ForeColor = SystemColors.ControlText;
			}
			else
			{
				_lblCurrentTime.Text = string.Format("{0:00}:{1:00}:{2:00}", 0, 0, 0);
				_lblCurrentTime.ForeColor = SystemColors.GrayText;
			}
		}

		private void ToDoList_ColumnClick(object sender, ColumnClickEventArgs e)
		{
			var column = _sorter.SortColumn;
			if (e.Column == _sorter.SortColumn)
			{
				_sorter.SortOrder = _sorter.SortOrder == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;
			}
			else
			{
				_sorter.SortColumn = e.Column;
				_sorter.SortOrder = SortOrder.Ascending;
			}
			ListViewHelper.SetSortIcons(_lvToDoList, column, e.Column, _sorter.SortOrder);
			_lvToDoList.Sort();
		}

		private void ToDoList_SelectedIndexChanged(object sender, EventArgs e)
		{
			UpdateCommands();
		}

		private void ToDoList_DoubleClick(object sender, MouseEventArgs e)
		{
			if (_lvToDoList.SelectedItems.Count == 1)
			{
				var item = _lvToDoList.SelectedItems[0];
				var assignable = item.Tag as AssignableSimpleDTO;
				if (assignable != null)
				{
					if (item.SubItems[6].Bounds.Contains(e.X, e.Y))
					{
						_lvToDoList.ClearComboBox();
						_lvToDoList.AddComboBoxItem(item.SubItems[6].Text);
						if (assignable.NextStates != null && assignable.NextStates.Length > 0)
						{
							foreach (var entityState in assignable.NextStates)
							{
								_lvToDoList.AddComboBoxItem(entityState.Name);
							}
						}
						_lvToDoList.ShowComboBoxForSubItem(item.SubItems[6]);
						return;
					}
					var uri = UriHelper.ViewEntityUri(Settings.Default.Uri, assignable.ID.GetValueOrDefault());
					LaunchBrowser(uri);
				}
			}
		}

		private void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
		{
			if (_lvToDoList.SelectedItems.Count == 1)
			{
				var item = _lvToDoList.SelectedItems[0];
				var assignable = (AssignableSimpleDTO) item.Tag;
				_contextMenuStrip.Items.Clear();
				if (assignable.NextStates != null && assignable.NextStates.Length > 0)
				{
					foreach (var entityState in assignable.NextStates)
					{
						var toolStripMenuItem = new ToolStripMenuItem(entityState.Name)
						                        	{Tag = new ToDoListItemTag(assignable, entityState)};
						toolStripMenuItem.Click += ToolStripMenuItem_Click;
						_contextMenuStrip.Items.Add(toolStripMenuItem);
					}
				}
				else
				{
					e.Cancel = true;
				}
			}
			else
			{
				e.Cancel = true;
			}
		}

		private void ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (_controller.Connected)
			{
				var tag = (ToDoListItemTag) ((ToolStripMenuItem) sender).Tag;
				_controller.ChangeState(tag.Assignable, tag.EntityState);
			}
		}

		private void ToDoList_ComboBoxValueChange(object sender, ComboBoxEventArgs e)
		{
			if (_controller.Connected)
			{
				var item = _lvToDoList.SelectedItems[0];
				var assignable = (AssignableSimpleDTO) item.Tag;
				foreach (var entityState in assignable.NextStates)
				{
					if (entityState.Name.Equals(e.Value))
					{
						var combo = sender as ComboBox;
						if (combo != null)
							combo.Hide();
						_controller.ChangeState(assignable, entityState);
						break;
					}
				}
			}
		}

		private void Play_Click(object sender, EventArgs e)
		{
			var currentRecord = _controller.GetCurrent();
			if (currentRecord != null)
			{
				if (!_controller.IsStarted)
				{
					if (
						MessageBox.Show(new WindowHandle(_application.MainWindow.HWnd),
						                string.Format(
						                	@"TargetProcess VisualStudio add-in has detected another Instance of add-in already tracking time for assignable #{0}. Starting time tracking from another instance of TargetProcess VisualStudio add-in will affect the time currently being tracked and all tracked time will be lost. Do you want to proceed ?",
						                	currentRecord.AssignableID),
						                @"TargetProcess VisualStudio add-in Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Stop) ==
						DialogResult.Yes)
					{
						_controller.Stop("Time Tracking was canceled by another instance of TargetProcess VisualStudio add-in");
					}
					return;
				}
				_controller.Stop();
				var description = "Time tracked in Visual Studio";
				if (ModifierKeys == Keys.Shift)
				{
					_controller.Stop(description);
				}
				else
				{
					var form = new TimeDescriptionForm();
					if (form.ShowDialog(new WindowHandle(_application.MainWindow.HWnd)) == DialogResult.OK)
					{
						description = form.Description ?? description;
						_controller.Stop(description);
					}
					else
					{
						_controller.Resume();
					}
				}
			}
			else
			{
				if (_lvToDoList.SelectedItems.Count == 1)
				{
					var item = _lvToDoList.SelectedItems[0];
					var assignable = item.Tag as AssignableSimpleDTO;
					_controller.Start(assignable.ID.GetValueOrDefault());
				}
			}
			RefreshToDoList();
			UpdateIndicators();
			UpdateCommands();
		}

		private void Refresh_Click(object sender, EventArgs e)
		{
			_controller.Refresh();
		}

		private void Submit_Click(object sender, EventArgs e)
		{
			_controller.SubmitTime();
			UpdateCommands();
			_controller.Refresh();
		}

		private void ClockTimer_Tick(object sender, EventArgs e)
		{
			// RefreshToDoList(); Causes flicker on the To Do list.
			UpdateIndicators();
			UpdateCommands();
		}

		private void RefreshTimer_Tick(object sender, EventArgs e)
		{
			_controller.Refresh();
		}

		private void TotalTime_Click(object sender, EventArgs e)
		{
			if (_controller.TotalTime.TotalMinutes > 0.0)
			{
				var form = new TimeLogForm(_controller.GetLog(), _controller.Names);
				form.ShowDialog(new WindowHandle(_application.MainWindow.HWnd));
			}
		}

		private void LaunchBrowser(Uri uri)
		{
			try
			{
				Process.Start(uri.ToString());
			}
			catch (Win32Exception)
			{
				// "No application is associated with the specified file for this operation"
				try
				{
					Process.Start("iexplore.exe", uri.ToString());
				}
				catch (Exception ex)
				{
					_env.Trace.WriteLine(ex);
					_env.ShowMessageBox(ex.Message, "Could not start web browser", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}
	}

	internal sealed class ToDoListItemTag
	{
		private readonly AssignableSimpleDTO _assignable;
		private readonly EntityStateDTO _entityState;

		public ToDoListItemTag(AssignableSimpleDTO assignable, EntityStateDTO entityState)
		{
			_assignable = assignable;
			_entityState = entityState;
		}

		public AssignableSimpleDTO Assignable
		{
			get { return _assignable; }
		}

		public EntityStateDTO EntityState
		{
			get { return _entityState; }
		}
	}

	/// <summary>
	/// Compares two To Do items for sort order.
	/// </summary>
	internal sealed class ToDoListSorter : IComparer
	{
		/// <summary>
		/// Case insensitive comparer object.
		/// </summary>
		private readonly CaseInsensitiveComparer _objectCompare = new CaseInsensitiveComparer();

		/// <summary>
		/// Specifies the column to be sorted.
		/// </summary>
		private int _sortColumn;

		/// <summary>
		/// Specifies the order in which to sort (i.e. 'Ascending').
		/// </summary>
		private SortOrder _sortOrder = SortOrder.None;

		/// <summary>
		/// Gets or sets the number of the column to which to apply the sorting operation (Defaults to '0').
		/// </summary>
		public int SortColumn
		{
			set { _sortColumn = value; }
			get { return _sortColumn; }
		}

		/// <summary>
		/// Gets or sets the order of sorting to apply (for example, 'Ascending' or 'Descending').
		/// </summary>
		public SortOrder SortOrder
		{
			set { _sortOrder = value; }
			get { return _sortOrder; }
		}

		#region IComparer Members

		public int Compare(object x, object y)
		{
			// Cast the objects to be compared to ListViewItem objects
			var a = (AssignableSimpleDTO) ((ListViewItem) x).Tag;
			var b = (AssignableSimpleDTO) ((ListViewItem) y).Tag;

			int compareResult;

			switch (_sortColumn)
			{
				case 0: // id
					compareResult = _objectCompare.Compare(a.ID, b.ID);
					break;
				case 1: // type: use story, task or bug
					compareResult = _objectCompare.Compare(a.EntityTypeName, b.EntityTypeName);
					if (compareResult == 0)
					{
						compareResult = _objectCompare.Compare(a.Name, b.Name);
					}
					if (compareResult == 0)
					{
						compareResult = _objectCompare.Compare(a.ID, b.ID);
					}
					break;
				case 2: // name
					compareResult = _objectCompare.Compare(a.Name, b.Name);
					if (compareResult == 0)
					{
						compareResult = _objectCompare.Compare(a.ID, b.ID);
					}
					break;
				case 3: // project name
					compareResult = _objectCompare.Compare(a.ProjectName, b.ProjectName);
					if (compareResult == 0)
					{
						compareResult = _objectCompare.Compare(a.Name, b.Name);
					}
					if (compareResult == 0)
					{
						compareResult = _objectCompare.Compare(a.ID, b.ID);
					}
					break;
				case 4: // rank
					compareResult = _objectCompare.Compare(a.Rank, b.Rank);
					if (compareResult == 0)
					{
						compareResult = _objectCompare.Compare(a.Name, b.Name);
					}
					if (compareResult == 0)
					{
						compareResult = _objectCompare.Compare(a.ID, b.ID);
					}
					break;
				case 5: // severity
					compareResult = _objectCompare.Compare(a.SeverityName, b.SeverityName);
					if (compareResult == 0)
					{
						compareResult = _objectCompare.Compare(a.Name, b.Name);
					}
					if (compareResult == 0)
					{
						compareResult = _objectCompare.Compare(a.ID, b.ID);
					}
					break;
				case 6: // status
					compareResult = _objectCompare.Compare(a.EntityStateName, b.EntityStateName);
					if (compareResult == 0)
					{
						compareResult = _objectCompare.Compare(a.Name, b.Name);
					}
					if (compareResult == 0)
					{
						compareResult = _objectCompare.Compare(a.ID, b.ID);
					}
					break;
				case 7: // time spent
					compareResult = _objectCompare.Compare(a.TimeSpent, b.TimeSpent);
					if (compareResult == 0)
					{
						compareResult = _objectCompare.Compare(a.Name, b.Name);
					}
					if (compareResult == 0)
					{
						compareResult = _objectCompare.Compare(a.ID, b.ID);
					}
					break;
				case 8: // time remaining
					compareResult = _objectCompare.Compare(a.TimeRemain, b.TimeRemain);
					if (compareResult == 0)
					{
						compareResult = _objectCompare.Compare(a.Name, b.Name);
					}
					if (compareResult == 0)
					{
						compareResult = _objectCompare.Compare(a.ID, b.ID);
					}
					break;
				default:
					return 0;
			}

			if (_sortOrder == SortOrder.Ascending)
			{
				return +compareResult;
			}

			if (_sortOrder == SortOrder.Descending)
			{
				return -compareResult;
			}

			return 0;
		}

		#endregion
	}
}