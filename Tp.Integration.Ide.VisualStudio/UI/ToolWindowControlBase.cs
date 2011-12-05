using System;
using System.Collections.Generic;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms;

namespace Tp.Integration.Ide.VisualStudio.UI {
	public class ToolWindowControlBase : UserControl {
		/// <summary>
		/// Enable the IME status handling for this control.
		/// </summary>
		protected override bool CanEnableIme {
			get { return true; }
		}

		/// <summary> 
		/// Let this control process the mnemonics.
		/// </summary>
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		protected override bool ProcessDialogChar(char charCode) {
			// If we're the top-level form or control, we need to do the mnemonic handling
			if (charCode != ' ' && ProcessMnemonic(charCode)) {
				return true;
			}
			return base.ProcessDialogChar(charCode);
		}
	}
}