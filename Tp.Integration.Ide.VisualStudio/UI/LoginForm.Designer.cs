namespace Tp.Integration.Ide.VisualStudio.UI
{
	partial class LoginForm
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
            this._lblUri = new System.Windows.Forms.Label();
            this._lblLogin = new System.Windows.Forms.Label();
            this._tbLogin = new System.Windows.Forms.TextBox();
            this._lblPassword = new System.Windows.Forms.Label();
            this._tbPassword = new System.Windows.Forms.TextBox();
            this._cbRememberCredentials = new System.Windows.Forms.CheckBox();
            this._btnLogin = new System.Windows.Forms.Button();
            this._errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this._btnCancel = new System.Windows.Forms.Button();
            this._tbUri = new System.Windows.Forms.TextBox();
            this._cbUseWindowsLogonName = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize) (this._errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // _lblUri
            // 
            this._lblUri.AutoSize = true;
            this._lblUri.Location = new System.Drawing.Point(12, 15);
            this._lblUri.Name = "_lblUri";
            this._lblUri.Size = new System.Drawing.Size(32, 13);
            this._lblUri.TabIndex = 0;
            this._lblUri.Text = "URL:";
            // 
            // _lblLogin
            // 
            this._lblLogin.AutoSize = true;
            this._lblLogin.Location = new System.Drawing.Point(12, 42);
            this._lblLogin.Name = "_lblLogin";
            this._lblLogin.Size = new System.Drawing.Size(36, 13);
            this._lblLogin.TabIndex = 2;
            this._lblLogin.Text = "Login:";
            // 
            // _tbLogin
            // 
            this._tbLogin.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._tbLogin.Location = new System.Drawing.Point(88, 39);
            this._tbLogin.MaxLength = 255;
            this._tbLogin.Name = "_tbLogin";
            this._tbLogin.Size = new System.Drawing.Size(213, 20);
            this._tbLogin.TabIndex = 3;
            // 
            // _lblPassword
            // 
            this._lblPassword.AutoSize = true;
            this._lblPassword.Location = new System.Drawing.Point(12, 68);
            this._lblPassword.Name = "_lblPassword";
            this._lblPassword.Size = new System.Drawing.Size(56, 13);
            this._lblPassword.TabIndex = 4;
            this._lblPassword.Text = "Password:";
            // 
            // _tbPassword
            // 
            this._tbPassword.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._tbPassword.Location = new System.Drawing.Point(88, 65);
            this._tbPassword.MaxLength = 50;
            this._tbPassword.Name = "_tbPassword";
            this._tbPassword.Size = new System.Drawing.Size(213, 20);
            this._tbPassword.TabIndex = 5;
            this._tbPassword.UseSystemPasswordChar = true;
            // 
            // _cbRememberCredentials
            // 
            this._cbRememberCredentials.AutoSize = true;
            this._cbRememberCredentials.Checked = global::Tp.Integration.Ide.VisualStudio.Settings.Default.SaveLogin;
            this._cbRememberCredentials.CheckState = System.Windows.Forms.CheckState.Checked;
            this._cbRememberCredentials.Location = new System.Drawing.Point(88, 115);
            this._cbRememberCredentials.Name = "_cbRememberCredentials";
            this._cbRememberCredentials.Size = new System.Drawing.Size(175, 17);
            this._cbRememberCredentials.TabIndex = 7;
            this._cbRememberCredentials.Text = "Remember me on this computer";
            this._cbRememberCredentials.UseVisualStyleBackColor = true;
            // 
            // _btnLogin
            // 
            this._btnLogin.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._btnLogin.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._btnLogin.Location = new System.Drawing.Point(162, 146);
            this._btnLogin.Name = "_btnLogin";
            this._btnLogin.Size = new System.Drawing.Size(75, 23);
            this._btnLogin.TabIndex = 8;
            this._btnLogin.Text = "Login";
            this._btnLogin.UseVisualStyleBackColor = true;
            // 
            // _errorProvider
            // 
            this._errorProvider.ContainerControl = this;
            // 
            // _btnCancel
            // 
            this._btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._btnCancel.Location = new System.Drawing.Point(243, 146);
            this._btnCancel.Name = "_btnCancel";
            this._btnCancel.Size = new System.Drawing.Size(75, 23);
            this._btnCancel.TabIndex = 9;
            this._btnCancel.Text = "Cancel";
            this._btnCancel.UseVisualStyleBackColor = true;
            // 
            // _tbUri
            // 
            this._tbUri.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._tbUri.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this._tbUri.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.AllUrl;
            this._tbUri.Location = new System.Drawing.Point(88, 12);
            this._tbUri.MaxLength = 1024;
            this._tbUri.Name = "_tbUri";
            this._tbUri.Size = new System.Drawing.Size(213, 20);
            this._tbUri.TabIndex = 1;
            // 
            // _cbUseWindowsLogonName
            // 
            this._cbUseWindowsLogonName.AutoSize = true;
            this._cbUseWindowsLogonName.Checked = global::Tp.Integration.Ide.VisualStudio.Settings.Default.UseWindowsLogin;
            this._cbUseWindowsLogonName.Location = new System.Drawing.Point(88, 92);
            this._cbUseWindowsLogonName.Name = "_cbUseWindowsLogonName";
            this._cbUseWindowsLogonName.Size = new System.Drawing.Size(150, 17);
            this._cbUseWindowsLogonName.TabIndex = 6;
            this._cbUseWindowsLogonName.Text = "Use Windows logon name";
            this._cbUseWindowsLogonName.UseVisualStyleBackColor = true;
            // 
            // LoginForm
            // 
            this.AcceptButton = this._btnLogin;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.CancelButton = this._btnCancel;
            this.ClientSize = new System.Drawing.Size(330, 181);
            this.Controls.Add(this._btnCancel);
            this.Controls.Add(this._btnLogin);
            this.Controls.Add(this._cbRememberCredentials);
            this.Controls.Add(this._cbUseWindowsLogonName);
            this.Controls.Add(this._tbPassword);
            this.Controls.Add(this._lblPassword);
            this.Controls.Add(this._tbLogin);
            this.Controls.Add(this._lblLogin);
            this.Controls.Add(this._tbUri);
            this.Controls.Add(this._lblUri);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Login to TargetProcess";
            ((System.ComponentModel.ISupportInitialize) (this._errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label _lblUri;
		private System.Windows.Forms.TextBox _tbUri;
		private System.Windows.Forms.Label _lblLogin;
		private System.Windows.Forms.TextBox _tbLogin;
		private System.Windows.Forms.Label _lblPassword;
		private System.Windows.Forms.TextBox _tbPassword;
		private System.Windows.Forms.CheckBox _cbUseWindowsLogonName;
		private System.Windows.Forms.CheckBox _cbRememberCredentials;
		private System.Windows.Forms.Button _btnLogin;
		private System.Windows.Forms.Button _btnCancel;
		private System.Windows.Forms.ErrorProvider _errorProvider;
	}
}