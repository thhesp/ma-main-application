namespace Server
{
    partial class TestForm
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
            this.serverStart = new System.Windows.Forms.Button();
            this.serverStop = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // serverStart
            // 
            this.serverStart.Location = new System.Drawing.Point(32, 52);
            this.serverStart.Name = "Start Server";
            this.serverStart.Size = new System.Drawing.Size(75, 23);
            this.serverStart.TabIndex = 0;
            this.serverStart.Text = "Start Server";
            this.serverStart.UseVisualStyleBackColor = true;
            this.serverStart.Click += new System.EventHandler(this.serverStart_Click);
            // 
            // serverStop
            // 
            this.serverStop.Location = new System.Drawing.Point(156, 52);
            this.serverStop.Name = "Stop Server";
            this.serverStop.Size = new System.Drawing.Size(75, 23);
            this.serverStop.TabIndex = 1;
            this.serverStop.Text = "Stop Server";
            this.serverStop.UseVisualStyleBackColor = true;
            this.serverStop.Click += new System.EventHandler(this.serverStop_Click);
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.serverStop);
            this.Controls.Add(this.serverStart);
            this.Name = "TestForm";
            this.Text = "TestForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button serverStart;
        private System.Windows.Forms.Button serverStop;
    }
}