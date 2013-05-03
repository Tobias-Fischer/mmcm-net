namespace CVZ_Core.GUI
{
    partial class MMCM_RemoteControlPanel
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
            this.textBoxPeriod = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.buttonGetParameters = new System.Windows.Forms.Button();
            this.checkBoxLogError = new System.Windows.Forms.CheckBox();
            this.buttonSetParameters = new System.Windows.Forms.Button();
            this.feedbakcTextBoxInfluence = new System.Windows.Forms.TextBox();
            this.textBoxSigma = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxLearningRate = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.labelMapInfo = new System.Windows.Forms.Label();
            this.groupBox2.SuspendLayout();
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
            this.groupBox2.Controls.Add(this.textBoxPeriod);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.buttonGetParameters);
            this.groupBox2.Controls.Add(this.checkBoxLogError);
            this.groupBox2.Controls.Add(this.buttonSetParameters);
            this.groupBox2.Controls.Add(this.feedbakcTextBoxInfluence);
            this.groupBox2.Controls.Add(this.textBoxSigma);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.textBoxLearningRate);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Location = new System.Drawing.Point(12, 234);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(268, 133);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Parameters";
            // 
            // textBoxPeriod
            // 
            this.textBoxPeriod.Location = new System.Drawing.Point(6, 104);
            this.textBoxPeriod.Name = "textBoxPeriod";
            this.textBoxPeriod.Size = new System.Drawing.Size(30, 20);
            this.textBoxPeriod.TabIndex = 20;
            this.textBoxPeriod.Text = "1,0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(41, 107);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(37, 13);
            this.label6.TabIndex = 19;
            this.label6.Text = "Period";
            // 
            // buttonGetParameters
            // 
            this.buttonGetParameters.Location = new System.Drawing.Point(167, 41);
            this.buttonGetParameters.Name = "buttonGetParameters";
            this.buttonGetParameters.Size = new System.Drawing.Size(95, 23);
            this.buttonGetParameters.TabIndex = 18;
            this.buttonGetParameters.Text = "Get parameters";
            this.buttonGetParameters.UseVisualStyleBackColor = true;
            this.buttonGetParameters.Click += new System.EventHandler(this.buttonGetParameters_Click);
            // 
            // checkBoxLogError
            // 
            this.checkBoxLogError.AutoSize = true;
            this.checkBoxLogError.Checked = true;
            this.checkBoxLogError.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxLogError.Location = new System.Drawing.Point(194, 18);
            this.checkBoxLogError.Name = "checkBoxLogError";
            this.checkBoxLogError.Size = new System.Drawing.Size(68, 17);
            this.checkBoxLogError.TabIndex = 12;
            this.checkBoxLogError.Text = "Log error";
            this.checkBoxLogError.UseVisualStyleBackColor = true;
            // 
            // buttonSetParameters
            // 
            this.buttonSetParameters.Location = new System.Drawing.Point(167, 70);
            this.buttonSetParameters.Name = "buttonSetParameters";
            this.buttonSetParameters.Size = new System.Drawing.Size(95, 23);
            this.buttonSetParameters.TabIndex = 17;
            this.buttonSetParameters.Text = "Set parameters";
            this.buttonSetParameters.UseVisualStyleBackColor = true;
            this.buttonSetParameters.Click += new System.EventHandler(this.buttonSetParameters_Click);
            // 
            // feedbakcTextBoxInfluence
            // 
            this.feedbakcTextBoxInfluence.Location = new System.Drawing.Point(6, 75);
            this.feedbakcTextBoxInfluence.Name = "feedbakcTextBoxInfluence";
            this.feedbakcTextBoxInfluence.Size = new System.Drawing.Size(30, 20);
            this.feedbakcTextBoxInfluence.TabIndex = 12;
            this.feedbakcTextBoxInfluence.Text = "1,0";
            // 
            // textBoxSigma
            // 
            this.textBoxSigma.Location = new System.Drawing.Point(6, 45);
            this.textBoxSigma.Name = "textBoxSigma";
            this.textBoxSigma.Size = new System.Drawing.Size(30, 20);
            this.textBoxSigma.TabIndex = 16;
            this.textBoxSigma.Text = "1,0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(41, 78);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Feedback Influence";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(41, 48);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(112, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Sigma (Neighborhood)";
            // 
            // textBoxLearningRate
            // 
            this.textBoxLearningRate.Location = new System.Drawing.Point(6, 19);
            this.textBoxLearningRate.Name = "textBoxLearningRate";
            this.textBoxLearningRate.Size = new System.Drawing.Size(30, 20);
            this.textBoxLearningRate.TabIndex = 14;
            this.textBoxLearningRate.Text = "1,0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(41, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Learning Rate";
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
            this.Text = "MMCM_ControlPanel";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonRun;
        private System.Windows.Forms.Label labelSteps;
        private System.Windows.Forms.FlowLayoutPanel panel1;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBoxPeriod;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button buttonGetParameters;
        private System.Windows.Forms.CheckBox checkBoxLogError;
        private System.Windows.Forms.Button buttonSetParameters;
        private System.Windows.Forms.TextBox feedbakcTextBoxInfluence;
        private System.Windows.Forms.TextBox textBoxSigma;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxLearningRate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labelMapInfo;

    }
}