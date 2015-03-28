using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

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
            this.inputIP = new System.Windows.Forms.TextBox();
            this.inputPort = new System.Windows.Forms.TextBox();
            this.btnConnectLocal = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.labelIP = new System.Windows.Forms.Label();
            this.labelPort = new System.Windows.Forms.Label();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.labelSendIP = new System.Windows.Forms.Label();
            this.labelSendPort = new System.Windows.Forms.Label();
            this.inputSendIP = new System.Windows.Forms.TextBox();
            this.inputSendPort = new System.Windows.Forms.TextBox();
            this.consoleOut = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // serverStart
            // 
            this.serverStart.Location = new System.Drawing.Point(32, 52);
            this.serverStart.Name = "serverStart";
            this.serverStart.Size = new System.Drawing.Size(75, 23);
            this.serverStart.TabIndex = 0;
            this.serverStart.Text = "Start Server";
            this.serverStart.UseVisualStyleBackColor = true;
            this.serverStart.Click += new System.EventHandler(this.serverStart_Click);
            // 
            // serverStop
            // 
            this.serverStop.Location = new System.Drawing.Point(156, 52);
            this.serverStop.Name = "serverStop";
            this.serverStop.Size = new System.Drawing.Size(75, 23);
            this.serverStop.TabIndex = 1;
            this.serverStop.Text = "Stop Server";
            this.serverStop.UseVisualStyleBackColor = true;
            this.serverStop.Click += new System.EventHandler(this.serverStop_Click);
            // 
            // inputIP
            // 
            this.inputIP.Location = new System.Drawing.Point(63, 112);
            this.inputIP.Name = "inputIP";
            this.inputIP.Size = new System.Drawing.Size(100, 20);
            this.inputIP.TabIndex = 2;
            // 
            // inputPort
            // 
            this.inputPort.Location = new System.Drawing.Point(295, 112);
            this.inputPort.Name = "inputPort";
            this.inputPort.Size = new System.Drawing.Size(100, 20);
            this.inputPort.TabIndex = 3;
            // 
            // btnConnectLocal
            // 
            this.btnConnectLocal.Location = new System.Drawing.Point(295, 52);
            this.btnConnectLocal.Name = "btnConnectLocal";
            this.btnConnectLocal.Size = new System.Drawing.Size(103, 23);
            this.btnConnectLocal.TabIndex = 4;
            this.btnConnectLocal.Text = "Connect Local";
            this.btnConnectLocal.UseVisualStyleBackColor = true;
            this.btnConnectLocal.Click += new System.EventHandler(this.btnConnectLocal_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(57, 197);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 5;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // labelIP
            // 
            this.labelIP.AutoSize = true;
            this.labelIP.Location = new System.Drawing.Point(12, 119);
            this.labelIP.Name = "labelIP";
            this.labelIP.Size = new System.Drawing.Size(17, 13);
            this.labelIP.TabIndex = 6;
            this.labelIP.Text = "IP";
            // 
            // labelPort
            // 
            this.labelPort.AutoSize = true;
            this.labelPort.Location = new System.Drawing.Point(215, 119);
            this.labelPort.Name = "labelPort";
            this.labelPort.Size = new System.Drawing.Size(26, 13);
            this.labelPort.TabIndex = 7;
            this.labelPort.Text = "Port";
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Location = new System.Drawing.Point(295, 197);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(75, 23);
            this.btnDisconnect.TabIndex = 8;
            this.btnDisconnect.Text = "Disconnect";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // labelSendIP
            // 
            this.labelSendIP.AutoSize = true;
            this.labelSendIP.Location = new System.Drawing.Point(12, 163);
            this.labelSendIP.Name = "labelSendIP";
            this.labelSendIP.Size = new System.Drawing.Size(45, 13);
            this.labelSendIP.TabIndex = 12;
            this.labelSendIP.Text = "Send IP";
            // 
            // labelSendPort
            // 
            this.labelSendPort.AutoSize = true;
            this.labelSendPort.Location = new System.Drawing.Point(215, 159);
            this.labelSendPort.Name = "labelSendPort";
            this.labelSendPort.Size = new System.Drawing.Size(54, 13);
            this.labelSendPort.TabIndex = 11;
            this.labelSendPort.Text = "Send Port";
            // 
            // inputSendIP
            // 
            this.inputSendIP.Location = new System.Drawing.Point(63, 156);
            this.inputSendIP.Name = "inputSendIP";
            this.inputSendIP.Size = new System.Drawing.Size(100, 20);
            this.inputSendIP.TabIndex = 10;
            // 
            // inputSendPort
            // 
            this.inputSendPort.Location = new System.Drawing.Point(298, 156);
            this.inputSendPort.Name = "inputSendPort";
            this.inputSendPort.Size = new System.Drawing.Size(100, 20);
            this.inputSendPort.TabIndex = 9;
            // 
            // consoleOut
            // 
            this.consoleOut.Location = new System.Drawing.Point(12, 248);
            this.consoleOut.Name = "consoleOut";
            this.consoleOut.Size = new System.Drawing.Size(481, 398);
            this.consoleOut.TabIndex = 13;
            this.consoleOut.Text = "";

            // Instantiate the writer
            _writer = new TextBoxStreamWriter(consoleOut);
            // Redirect the out Console stream
            Console.SetOut(_writer);

            Server.Log("Now redirecting output to the text box");

            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(505, 658);
            this.Controls.Add(this.consoleOut);
            this.Controls.Add(this.labelSendIP);
            this.Controls.Add(this.labelSendPort);
            this.Controls.Add(this.inputSendIP);
            this.Controls.Add(this.inputSendPort);
            this.Controls.Add(this.btnDisconnect);
            this.Controls.Add(this.labelPort);
            this.Controls.Add(this.labelIP);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.btnConnectLocal);
            this.Controls.Add(this.inputPort);
            this.Controls.Add(this.inputIP);
            this.Controls.Add(this.serverStop);
            this.Controls.Add(this.serverStart);
            this.Name = "TestForm";
            this.Text = "TestForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button serverStart;
        private System.Windows.Forms.Button serverStop;
        private System.Windows.Forms.Label labelIP;
        private System.Windows.Forms.TextBox inputIP;

        private System.Windows.Forms.Label labelPort;
        private System.Windows.Forms.TextBox inputPort;
        private System.Windows.Forms.Button btnConnectLocal;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Label labelSendIP;
        private System.Windows.Forms.Label labelSendPort;
        private System.Windows.Forms.TextBox inputSendIP;
        private System.Windows.Forms.TextBox inputSendPort;
        private System.Windows.Forms.RichTextBox consoleOut;


        // That's our custom TextWriter class
        TextWriter _writer = null;
    }
}