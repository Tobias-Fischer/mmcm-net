namespace MMCMLibrary
{
    partial class CTPC_RemoteControlPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MMCM_RemoteControlPanel));
            this.buttonRun = new System.Windows.Forms.Button();
            this.labelSteps = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.buttonStop = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.labelMapInfo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonRun
            // 
            this.buttonRun.Image = ((System.Drawing.Image)(resources.GetObject("buttonRun.Image")));
            this.buttonRun.Location = new System.Drawing.Point(242, 0);
            this.buttonRun.Name = "buttonRun";
            this.buttonRun.Size = new System.Drawing.Size(32, 36);
            this.buttonRun.TabIndex = 4;
            this.buttonRun.UseVisualStyleBackColor = true;
            this.buttonRun.Click += new System.EventHandler(this.buttonRun_Click);
            // 
            // labelSteps
            // 
            this.labelSteps.AutoSize = true;
            this.labelSteps.Location = new System.Drawing.Point(12, 370);
            this.labelSteps.Name = "labelSteps";
            this.labelSteps.Size = new System.Drawing.Size(67, 13);
            this.labelSteps.TabIndex = 5;
            this.labelSteps.Text = "Steps done :";
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.panel1.Location = new System.Drawing.Point(12, 28);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(262, 200);
            this.panel1.TabIndex = 6;
            this.panel1.WrapContents = false;
            // 
            // buttonStop
            // 
            this.buttonStop.Enabled = false;
            this.buttonStop.Image = ((System.Drawing.Image)(resources.GetObject("buttonStop.Image")));
            this.buttonStop.Location = new System.Drawing.Point(242, 0);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(32, 36);
            this.buttonStop.TabIndex = 9;
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Visible = false;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Location = new System.Drawing.Point(12, 234);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(268, 133);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Parameters";
            // 
            // labelMapInfo
            // 
            this.labelMapInfo.AutoSize = true;
            this.labelMapInfo.Location = new System.Drawing.Point(15, 405);
            this.labelMapInfo.Name = "labelMapInfo";
            this.labelMapInfo.Size = new System.Drawing.Size(60, 13);
            this.labelMapInfo.TabIndex = 15;
            this.labelMapInfo.Text = "Map Infos: ";
            // 
            // MMCM_RemoteControlPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(293, 430);
            this.Controls.Add(this.labelMapInfo);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.buttonRun);
            this.Controls.Add(this.buttonStop);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.labelSteps);
            this.Name = "MMCM_RemoteControlPanel";
            this.Text = "CTPC_ControlPanel";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonRun;
        private System.Windows.Forms.Label labelSteps;
        private System.Windows.Forms.FlowLayoutPanel panel1;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label labelMapInfo;

    }
}