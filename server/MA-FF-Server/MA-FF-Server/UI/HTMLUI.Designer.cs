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
            this.ClientSize = new System.Drawing.Size(768, 391);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "HTMLUI";
            this.ShowIcon = false;
            this.Text = "HTMLUI";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Browser_Closing);
            this.Load += new System.EventHandler(this.Browser_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion
    }
}