namespace Tp.Integration.Ide.VisualStudio.UI
{
	partial class ToolWindowControl
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}


		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ToolWindowControl));
			this._lvToDoList = new ListViewEx();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader7 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader8 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader9 = new System.Windows.Forms.ColumnHeader();
			this._contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this._imageList = new System.Windows.Forms.ImageList(this.components);
			this.panel1 = new System.Windows.Forms.Panel();
			this._btnSubmit = new System.Windows.Forms.Button();
			this._btnRefresh = new System.Windows.Forms.Button();
			this._btnPlay = new System.Windows.Forms.Button();
			this._lblCurrentTime = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this._lblTotalTime = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this._toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// _lvToDoList
			// 
			this._lvToDoList.AllowColumnReorder = true;
			this._lvToDoList.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this._lvToDoList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader8,
            this.columnHeader9});
			this._lvToDoList.ContextMenuStrip = this._contextMenuStrip;
			this._lvToDoList.Dock = System.Windows.Forms.DockStyle.Fill;
			this._lvToDoList.FullRowSelect = true;
			this._lvToDoList.GridLines = true;
			this._lvToDoList.HideSelection = false;
			this._lvToDoList.Location = new System.Drawing.Point(0, 29);
			this._lvToDoList.MultiSelect = false;
			this._lvToDoList.Name = "_lvToDoList";
			this._lvToDoList.ShowItemToolTips = true;
			this._lvToDoList.Size = new System.Drawing.Size(721, 225);
			this._lvToDoList.SmallImageList = this._imageList;
			this._lvToDoList.TabIndex = 11;
			this._lvToDoList.UseCompatibleStateImageBehavior = false;
			this._lvToDoList.View = System.Windows.Forms.View.Details;
			this._lvToDoList.SelectedIndexChanged += new System.EventHandler(this.ToDoList_SelectedIndexChanged);
			this._lvToDoList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ToDoList_DoubleClick);
			this._lvToDoList.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.ToDoList_ColumnClick);
            this._lvToDoList.ComboBoxValueChange +=new System.EventHandler<ComboBoxEventArgs>(this.ToDoList_ComboBoxValueChange);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "ID";
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Type";
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "Name";
			this.columnHeader3.Width = 250;
			// 
			// columnHeader4
			// 
			this.columnHeader4.Text = "Project";
			this.columnHeader4.Width = 120;
			// 
			// columnHeader5
			// 
			this.columnHeader5.Text = "Rank";
			// 
			// columnHeader6
			// 
			this.columnHeader6.Text = "Severity";
			this.columnHeader6.Width = 90;
			// 
			// columnHeader7
			// 
			this.columnHeader7.Text = "Status";
			this.columnHeader7.Width = 90;
			// 
			// columnHeader8
			// 
			this.columnHeader8.Text = "Time Spent";
			this.columnHeader8.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.columnHeader8.Width = 80;
			// 
			// columnHeader9
			// 
			this.columnHeader9.Text = "Time Remaining";
			this.columnHeader9.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.columnHeader9.Width = 80;
			// 
			// _contextMenuStrip
			// 
			this._contextMenuStrip.Name = "_contextMenuStrip";
			this._contextMenuStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this._contextMenuStrip.ShowImageMargin = false;
			this._contextMenuStrip.Size = new System.Drawing.Size(36, 4);
			this._contextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.ContextMenuStrip_Opening);
			// 
			// _imageList
			// 
			this._imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer) (resources.GetObject("_imageList.ImageStream")));
			this._imageList.TransparentColor = System.Drawing.Color.Transparent;
			this._imageList.Images.SetKeyName(0, "userstory.png");
			this._imageList.Images.SetKeyName(1, "task.png");
			this._imageList.Images.SetKeyName(2, "bug.png");
			this._imageList.Images.SetKeyName(3, "BtnStart.png");
			this._imageList.Images.SetKeyName(4, "BtnStop.png");
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this._btnSubmit);
			this.panel1.Controls.Add(this._btnRefresh);
			this.panel1.Controls.Add(this._btnPlay);
			this.panel1.Controls.Add(this._lblCurrentTime);
			this.panel1.Controls.Add(this.label2);
			this.panel1.Controls.Add(this._lblTotalTime);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(721, 29);
			this.panel1.TabIndex = 8;
			// 
			// _btnSubmit
			// 
			this._btnSubmit.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._btnSubmit.Enabled = false;
			this._btnSubmit.Location = new System.Drawing.Point(628, 3);
			this._btnSubmit.Name = "_btnSubmit";
			this._btnSubmit.Size = new System.Drawing.Size(90, 23);
			this._btnSubmit.TabIndex = 9;
			this._btnSubmit.Text = "Submit Time";
			this._toolTip.SetToolTip(this._btnSubmit, "Post recorderd time to server.");
			this._btnSubmit.UseVisualStyleBackColor = true;
			this._btnSubmit.Click += new System.EventHandler(this.Submit_Click);
			// 
			// _btnRefresh
			// 
			this._btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._btnRefresh.Location = new System.Drawing.Point(532, 3);
			this._btnRefresh.Name = "_btnRefresh";
			this._btnRefresh.Size = new System.Drawing.Size(90, 23);
			this._btnRefresh.TabIndex = 8;
			this._btnRefresh.Text = "Refresh List";
			this._toolTip.SetToolTip(this._btnRefresh, "Get newest data from the server.");
			this._btnRefresh.UseVisualStyleBackColor = true;
			this._btnRefresh.Click += new System.EventHandler(this.Refresh_Click);
			// 
			// _btnPlay
			// 
			this._btnPlay.Enabled = false;
			this._btnPlay.ImageIndex = 3;
			this._btnPlay.ImageList = this._imageList;
			this._btnPlay.Location = new System.Drawing.Point(319, 3);
			this._btnPlay.Name = "_btnPlay";
			this._btnPlay.Size = new System.Drawing.Size(26, 23);
			this._btnPlay.TabIndex = 5;
			this._toolTip.SetToolTip(this._btnPlay, "Start or stop time recording.");
			this._btnPlay.UseVisualStyleBackColor = true;
			this._btnPlay.Click += new System.EventHandler(this.Play_Click);
			// 
			// _lblCurrentTime
			// 
			this._lblCurrentTime.AutoSize = true;
			this._lblCurrentTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			this._lblCurrentTime.Location = new System.Drawing.Point(233, 3);
			this._lblCurrentTime.Name = "_lblCurrentTime";
			this._lblCurrentTime.Size = new System.Drawing.Size(80, 24);
			this._lblCurrentTime.TabIndex = 4;
			this._lblCurrentTime.Text = "00:00:00";
			this._toolTip.SetToolTip(this._lblCurrentTime, "Time spend for the current task");
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(161, 11);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(66, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "Current time:";
			// 
			// _lblTotalTime
			// 
			this._lblTotalTime.AutoSize = true;
			this._lblTotalTime.Cursor = System.Windows.Forms.Cursors.Default;
			this._lblTotalTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			this._lblTotalTime.Location = new System.Drawing.Point(65, 3);
			this._lblTotalTime.Name = "_lblTotalTime";
			this._lblTotalTime.Size = new System.Drawing.Size(80, 24);
			this._lblTotalTime.TabIndex = 2;
			this._lblTotalTime.Text = "00:00:00";
			this._toolTip.SetToolTip(this._lblTotalTime, "Total time recorded so far");
			this._lblTotalTime.Click += new System.EventHandler(this.TotalTime_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(3, 11);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(56, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Total time:";
			// 
			// ToolWindowControl
			// 
			this.BackColor = System.Drawing.SystemColors.Control;
			this.Controls.Add(this._lvToDoList);
			this.Controls.Add(this.panel1);
			this.Name = "ToolWindowControl";
			this.Size = new System.Drawing.Size(721, 254);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);

		}
		#endregion

		private ListViewEx _lvToDoList;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.ColumnHeader columnHeader4;
		private System.Windows.Forms.ColumnHeader columnHeader5;
		private System.Windows.Forms.ColumnHeader columnHeader6;
		private System.Windows.Forms.ColumnHeader columnHeader7;
		private System.Windows.Forms.ContextMenuStrip _contextMenuStrip;
		private System.Windows.Forms.ColumnHeader columnHeader8;
		private System.Windows.Forms.ColumnHeader columnHeader9;
		private System.Windows.Forms.ImageList _imageList;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label _lblTotalTime;
		private System.Windows.Forms.Button _btnPlay;
		private System.Windows.Forms.Button _btnSubmit;
		private System.Windows.Forms.Button _btnRefresh;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label _lblCurrentTime;
		private System.Windows.Forms.ToolTip _toolTip;
	}
}