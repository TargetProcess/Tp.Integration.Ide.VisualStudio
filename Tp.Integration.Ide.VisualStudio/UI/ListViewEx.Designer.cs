namespace Tp.Integration.Ide.VisualStudio.UI
{
    partial class ListViewEx
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.ComboBox comboBox = null;

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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            this.comboBox = new System.Windows.Forms.ComboBox();

            this.comboBox.Size = new System.Drawing.Size(0, 0);
            this.comboBox.Location = new System.Drawing.Point(0, 0);
            this.comboBox.LostFocus += new System.EventHandler(this.OnComboBoxFocusExit);
            this.comboBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnComboBoxKeyPress);
            this.comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox.SelectedValueChanged += new System.EventHandler(OnComboBoxSelectedValueChanged);
            this.comboBox.Hide();

            SetStyle(System.Windows.Forms.ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(System.Windows.Forms.ControlStyles.OptimizedDoubleBuffer, true);

            this.Controls.Add(this.comboBox);
            this.DoubleBuffered = true;
            this.OwnerDraw = true;
            this.ResumeLayout(false);
        }

        #endregion
    }
}
