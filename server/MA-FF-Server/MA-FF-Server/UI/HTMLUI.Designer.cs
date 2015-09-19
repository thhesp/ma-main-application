using System.Windows.Forms;
using System.Windows;
using System.Drawing;

namespace WebAnalyzer.UI
{
    partial class HTMLUI
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
            this.SuspendLayout();
            // 
            // HTMLUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            int height = (int)(Screen.PrimaryScreen.Bounds.Height - 40);
            int width = (int)(Screen.PrimaryScreen.Bounds.Width);
            this.Size = new Size(width, height);
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "WebAnalyzer";
            this.ShowIcon = false;
            this.Text = "WebAnalyzer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Browser_Closing);
            this.Load += new System.EventHandler(this.Browser_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

    }
}