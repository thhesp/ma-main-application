using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

using WebAnalyzer.Util;

namespace WebAnalyzer.Test.Communication
{
    partial class MouseTracker
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
            this.consoleOut = new System.Windows.Forms.RichTextBox();
            this.mousetrackingStart = new System.Windows.Forms.Button();
            this.mousetrackingStop = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // serverStart
            // 
            this.serverStart.Location = new System.Drawing.Point(38, 12);
            this.serverStart.Name = "serverStart";
            this.serverStart.Size = new System.Drawing.Size(75, 23);
            this.serverStart.TabIndex = 0;
            this.serverStart.Text = "Start Server";
            this.serverStart.UseVisualStyleBackColor = true;
            this.serverStart.Click += new System.EventHandler(this.serverStart_Click);
            // 
            // serverStop
            // 
            this.serverStop.Location = new System.Drawing.Point(38, 52);
            this.serverStop.Name = "serverStop";
            this.serverStop.Size = new System.Drawing.Size(75, 23);
            this.serverStop.TabIndex = 1;
            this.serverStop.Text = "Stop Server";
            this.serverStop.UseVisualStyleBackColor = true;
            this.serverStop.Click += new System.EventHandler(this.serverStop_Click);
            // 
            // consoleOut
            // 
            this.consoleOut.Location = new System.Drawing.Point(12, 81);
            this.consoleOut.Name = "consoleOut";
            this.consoleOut.Size = new System.Drawing.Size(481, 398);
            this.consoleOut.TabIndex = 13;
            this.consoleOut.Text = "";

            // Instantiate the writer
            //_writer = new TextBoxStreamWriter(consoleOut);
            // Redirect the out Console stream
            //Console.SetOut(_writer);

            //Logger.Log("Now redirecting output to the text box");


            // 
            // mousetrackingStart
            // 
            this.mousetrackingStart.Location = new System.Drawing.Point(276, 12);
            this.mousetrackingStart.Name = "mousetrackingStart";
            this.mousetrackingStart.Size = new System.Drawing.Size(122, 23);
            this.mousetrackingStart.TabIndex = 14;
            this.mousetrackingStart.Text = "Start Mousetracking";
            this.mousetrackingStart.UseVisualStyleBackColor = true;
            this.mousetrackingStart.Click += new System.EventHandler(this.mousetrackingStart_Click);
            // 
            // mousetrackingStop
            // 
            this.mousetrackingStop.Location = new System.Drawing.Point(276, 52);
            this.mousetrackingStop.Name = "mousetrackingStop";
            this.mousetrackingStop.Size = new System.Drawing.Size(122, 23);
            this.mousetrackingStop.TabIndex = 15;
            this.mousetrackingStop.Text = "Stop Mousetracking";
            this.mousetrackingStop.UseVisualStyleBackColor = true;
            this.mousetrackingStop.Click += new System.EventHandler(this.mousetrackingStop_Click);
            // 
            // MouseTracker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(505, 489);
            this.Controls.Add(this.mousetrackingStop);
            this.Controls.Add(this.mousetrackingStart);
            this.Controls.Add(this.consoleOut);
            this.Controls.Add(this.serverStop);
            this.Controls.Add(this.serverStart);
            this.Name = "MouseTracker";
            this.Text = "MouseTracker";
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Button serverStart;
        private System.Windows.Forms.Button serverStop;
        private System.Windows.Forms.RichTextBox consoleOut;


        // That's our custom TextWriter class
        TextWriter _writer = null;
        private Button mousetrackingStart;
        private Button mousetrackingStop;
    }
}