using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace Tp.Integration.Ide.VisualStudio
{
    [RunInstaller(true)]
    public partial class Installer : System.Configuration.Install.Installer
    {
        public Installer()
        {
            InitializeComponent();
        }

        // Override the 'Install' method.
        // The Installation will call this method to run the Custom Action
        public override void Install(IDictionary savedState)
        {
            base.Install(savedState);

            var localByName =
                Process.GetProcessesByName("devenv");
            if (localByName.Length > 0)
            {
                MessageBox.Show(
                    "Setup has detected Microsoft Visual Studio running on your computer.\r\n         Please, restart Visual Studio to complete the installation.",
                    "Installer", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}