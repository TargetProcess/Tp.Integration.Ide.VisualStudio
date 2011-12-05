using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Tp.Integration.Ide.VisualStudio.Utils;

namespace Tp.Integration.Ide.VisualStudio {
	internal sealed partial class Settings {
		private readonly Crypto _crypto = new Crypto();

		public string DecryptedPassword {
			get {
				try {
					return Password != null ? _crypto.Decrypt(Password) : null;
				}
				catch (Exception) {
					return "";
				}
			}
			set { Password = value != null ? _crypto.Encrypt(value) : null; }
		}

		public bool HasCredentials {
			get {
				return Uri != null &&
				       (UseWindowsLogin || !string.IsNullOrEmpty(Login) && !string.IsNullOrEmpty(DecryptedPassword));
			}
		}

		public ICredentials Credentials {
			get {
				if (UseWindowsLogin) {
					return CredentialCache.DefaultCredentials;
				}
				else {
					string domain = "";
					string user = Login;

					string[] parts = Login.Split(new[] {'\\'}, 2);
					if (parts.Length == 2)
					{
						domain = parts[0];
						user = parts[1];
					}

					return new NetworkCredential(user, DecryptedPassword, domain);
				}
			}
		}
	}
}