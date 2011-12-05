namespace Tp.Integration.Ide.VisualStudio.UI
{
	partial class OptionsForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.btnOk = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this._cbStopTrackingOnVisualStudioClose = new System.Windows.Forms.CheckBox();
			this._tbAutoRefreshInterval = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this._cbStopTrackingOnUserIdle = new System.Windows.Forms.CheckBox();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this._cbAutoRefresh = new System.Windows.Forms.CheckBox();
			this._cbAutoLogin = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize) (this._tbAutoRefreshInterval)).BeginInit();
			this.SuspendLayout();
			// 
			// btnOk
			// 
			this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOk.Location = new System.Drawing.Point(226, 154);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(75, 23);
			this.btnOk.TabIndex = 6;
			this.btnOk.Text = "OK";
			this.btnOk.UseVisualStyleBackColor = true;
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(307, 154);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 7;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// _cbStopTrackingOnVisualStudioClose
			// 
			this._cbStopTrackingOnVisualStudioClose.AutoSize = true;
			this._cbStopTrackingOnVisualStudioClose.Checked = global::Tp.Integration.Ide.VisualStudio.Settings.Default.StopTrackingOnVisualStudioClose;
			this._cbStopTrackingOnVisualStudioClose.CheckState = System.Windows.Forms.CheckState.Checked;
			
			this._cbStopTrackingOnVisualStudioClose.Location = new System.Drawing.Point(13, 85);
			this._cbStopTrackingOnVisualStudioClose.Name = "_cbStopTrackingOnVisualStudioClose";
			this._cbStopTrackingOnVisualStudioClose.Size = new System.Drawing.Size(237, 17);
			this._cbStopTrackingOnVisualStudioClose.TabIndex = 4;
			this._cbStopTrackingOnVisualStudioClose.Text = "Stop tracking time when Visual Studio closes";
			this._cbStopTrackingOnVisualStudioClose.UseVisualStyleBackColor = true;
			// 
			// _tbAutoRefreshInterval
			// 
			this._tbAutoRefreshInterval.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			
			this._tbAutoRefreshInterval.Location = new System.Drawing.Point(307, 59);
			this._tbAutoRefreshInterval.Maximum = new decimal(new int[] {
            120,
            0,
            0,
            0});
			this._tbAutoRefreshInterval.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this._tbAutoRefreshInterval.Name = "_tbAutoRefreshInterval";
			this._tbAutoRefreshInterval.Size = new System.Drawing.Size(75, 20);
			this._tbAutoRefreshInterval.TabIndex = 3;
			this._tbAutoRefreshInterval.Value = global::Tp.Integration.Ide.VisualStudio.Settings.Default.AutoRefreshInterval;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(29, 61);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(212, 13);
			this.label1.TabIndex = 6;
			this.label1.Text = "To Do list synchronization inteval in minutes";
			// 
			// _cbStopTrackingOnUserIdle
			// 
			this._cbStopTrackingOnUserIdle.AutoSize = true;
			this._cbStopTrackingOnUserIdle.Checked = global::Tp.Integration.Ide.VisualStudio.Settings.Default.StopTrackingOnUserIdle;
			this._cbStopTrackingOnUserIdle.CheckState = System.Windows.Forms.CheckState.Checked;
			
			this._cbStopTrackingOnUserIdle.Location = new System.Drawing.Point(13, 109);
			this._cbStopTrackingOnUserIdle.Name = "_cbStopTrackingOnUserIdle";
			this._cbStopTrackingOnUserIdle.Size = new System.Drawing.Size(271, 17);
			this._cbStopTrackingOnUserIdle.TabIndex = 5;
			this._cbStopTrackingOnUserIdle.Text = "Stop tracking time when user is away from computer";
			this._cbStopTrackingOnUserIdle.UseVisualStyleBackColor = true;
			// 
			// _cbAutoRefresh
			// 
			this._cbAutoRefresh.AutoSize = true;
			this._cbAutoRefresh.Checked = global::Tp.Integration.Ide.VisualStudio.Settings.Default.AutoRefresh;
			this._cbAutoRefresh.CheckState = System.Windows.Forms.CheckState.Checked;

			
			this._cbAutoRefresh.Location = new System.Drawing.Point(13, 36);
			this._cbAutoRefresh.Name = "_cbAutoRefresh";
			this._cbAutoRefresh.Size = new System.Drawing.Size(249, 17);
			this._cbAutoRefresh.TabIndex = 2;
			this._cbAutoRefresh.Text = "Automatically synchronize To Do list with server";
			this._cbAutoRefresh.UseVisualStyleBackColor = true;
			// 
			// _cbAutoLogin
			// 
			this._cbAutoLogin.AutoSize = true;
			this._cbAutoLogin.Checked = global::Tp.Integration.Ide.VisualStudio.Settings.Default.AutoLogin;
			this._cbAutoLogin.CheckState = System.Windows.Forms.CheckState.Checked;
			this._cbAutoLogin.Location = new System.Drawing.Point(13, 13);
			this._cbAutoLogin.Name = "_cbAutoLogin";
			this._cbAutoLogin.Size = new System.Drawing.Size(278, 17);
			this._cbAutoLogin.TabIndex = 1;
			this._cbAutoLogin.Text = "Automatically login to server when Visual Studio starts";
			this._cbAutoLogin.UseVisualStyleBackColor = true;
			// 
			// OptionsForm
			// 
			this.AcceptButton = this.btnOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(394, 189);
			this.Controls.Add(this._cbStopTrackingOnUserIdle);
			this.Controls.Add(this.label1);
			this.Controls.Add(this._tbAutoRefreshInterval);
			this.Controls.Add(this._cbStopTrackingOnVisualStudioClose);
			this.Controls.Add(this._cbAutoRefresh);
			this.Controls.Add(this._cbAutoLogin);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOk);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "OptionsForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Options";
			((System.ComponentModel.ISupportInitialize) (this._tbAutoRefreshInterval)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.CheckBox _cbAutoLogin;
		private System.Windows.Forms.CheckBox _cbAutoRefresh;
		private System.Windows.Forms.CheckBox _cbStopTrackingOnVisualStudioClose;
		private System.Windows.Forms.NumericUpDown _tbAutoRefreshInterval;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.CheckBox _cbStopTrackingOnUserIdle;
		private System.Windows.Forms.ToolTip toolTip1;
	}
}