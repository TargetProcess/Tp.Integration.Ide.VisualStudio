using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Tp.Integration.Ide.VisualStudio.UI {
	internal sealed partial class LoginForm : Form {
		internal LoginForm(Settings settings) {
			InitializeComponent();

			_tbUri.Validating += Uri_Validating;
			_tbUri.Validated += delegate { _errorProvider.SetError(_tbUri, null); };
			_tbLogin.Validating += Login_Validating;
			_tbLogin.Validated += delegate { _errorProvider.SetError(_tbLogin, null); };
			_tbPassword.Validating += Password_Validating;
			_tbPassword.Validated += delegate { _errorProvider.SetError(_tbPassword, null); };
			{
				var binding = new Binding("Text", settings, "Uri");
				binding.Format += (object sender, ConvertEventArgs e) => { e.Value = (e.Value ?? "").ToString(); };
				binding.Parse += (object sender, ConvertEventArgs e) => { e.Value = new Uri((string) e.Value, UriKind.Absolute); };
				_tbUri.DataBindings.Add(binding);
			}
			{
				var binding = new Binding("Text", settings, "Login");
				binding.Format += (object sender, ConvertEventArgs e) => {
					if (settings.UseWindowsLogin) {
						e.Value = string.Format("{0}\\{1}", Environment.UserDomainName, Environment.UserName);
					}
				};
				binding.Parse += (object sender, ConvertEventArgs e) => {
					if (settings.UseWindowsLogin) {
						e.Value = settings.Login;
					}
				};
				_tbLogin.DataBindings.Add(binding);
			}
			{
				var binding = new Binding("Text", settings, "DecryptedPassword");
				binding.Format += (object sender, ConvertEventArgs e) => {
					if (settings.UseWindowsLogin) {
						e.Value = "";
					}
				};
				binding.Parse += (object sender, ConvertEventArgs e) => {
					if (settings.UseWindowsLogin) {
						e.Value = settings.DecryptedPassword;
					}
				};
				_tbPassword.DataBindings.Add(binding);
			}
			_cbUseWindowsLogonName.DataBindings.Add(new Binding("Checked", settings, "UseWindowsLogin"));
			_cbRememberCredentials.DataBindings.Add(new Binding("Checked", settings, "SaveLogin"));
			{
				var binding = new Binding("Enabled", _cbUseWindowsLogonName, "Checked");
				binding.Format += (object sender, ConvertEventArgs e) => { e.Value = !(bool) e.Value; };
				_tbLogin.DataBindings.Add(binding);
			}
			{
				var binding = new Binding("Enabled", _cbUseWindowsLogonName, "Checked");
				binding.Format += (object sender, ConvertEventArgs e) => { e.Value = !(bool) e.Value; };
				_tbPassword.DataBindings.Add(binding);
			}
			_cbUseWindowsLogonName.CheckedChanged += (sender, e) => {
			                                         	if (_cbUseWindowsLogonName.Checked) {
															_tbLogin.Text = string.Format("{0}\\{1}", Environment.UserDomainName, Environment.UserName);
			                                         		_tbPassword.Text = "";
															_errorProvider.SetError(_tbLogin, null);
															_errorProvider.SetError(_tbPassword, null);
			                                         	} 
														else {
															_tbLogin.Text = settings.Login;
															_tbPassword.Text = settings.DecryptedPassword;
			                                         	}
			                                         };
			FormClosing += LoginForm_FormClosing;
		}

		private void Uri_Validating(object sender, CancelEventArgs e) {
			try {
				var uri = new Uri(_tbUri.Text, UriKind.Absolute);
			}
			catch (Exception ex) {
				_errorProvider.SetError(_tbUri, ex.Message);
				e.Cancel = true;
			}
		}

		private void Login_Validating(object sender, CancelEventArgs e) {
			if (_cbUseWindowsLogonName.Checked == false && _tbLogin.Text.Length == 0) {
				_errorProvider.SetError(_tbLogin, "Invalid login");
				e.Cancel = true;
			}
		}

		private void Password_Validating(object sender, CancelEventArgs e) {
			if (_cbUseWindowsLogonName.Checked == false && _tbPassword.Text.Length == 0) {
				_errorProvider.SetError(_tbPassword, "Invalid password");
				e.Cancel = true;
			}
		}

		private void LoginForm_FormClosing(object sender, FormClosingEventArgs e) {
			if (DialogResult == DialogResult.OK) {
				e.Cancel = !ValidateChildren();
			}
		}
	}
}