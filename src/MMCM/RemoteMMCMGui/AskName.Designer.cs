namespace RemoteMMCMGui
{
    partial class AskName
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
            this.button1 = new System.Windows.Forms.Button();
            this.txtName = new System.Windows.Forms.TextBox();
            this.radioButtonMMCM = new System.Windows.Forms.RadioButton();
            this.radioButtonCTPC = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(118, 9);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(45, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Go!";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(12, 12);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(100, 20);
            this.txtName.TabIndex = 1;
            this.txtName.Text = "defaultMap";
            // 
            // radioButtonMMCM
            // 
            this.radioButtonMMCM.AutoSize = true;
            this.radioButtonMMCM.Checked = true;
            this.radioButtonMMCM.Location = new System.Drawing.Point(12, 39);
            this.radioButtonMMCM.Name = "radioButtonMMCM";
            this.radioButtonMMCM.Size = new System.Drawing.Size(59, 17);
            this.radioButtonMMCM.TabIndex = 2;
            this.radioButtonMMCM.TabStop = true;
            this.radioButtonMMCM.Text = "MMCM";
            this.radioButtonMMCM.UseVisualStyleBackColor = true;
            // 
            // radioButtonCTPC
            // 
            this.radioButtonCTPC.AutoSize = true;
            this.radioButtonCTPC.Location = new System.Drawing.Point(77, 39);
            this.radioButtonCTPC.Name = "radioButtonCTPC";
            this.radioButtonCTPC.Size = new System.Drawing.Size(53, 17);
            this.radioButtonCTPC.TabIndex = 3;
            this.radioButtonCTPC.Text = "CTPC";
            this.radioButtonCTPC.UseVisualStyleBackColor = true;
            // 
            // AskName
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(174, 75);
            this.Controls.Add(this.radioButtonCTPC);
            this.Controls.Add(this.radioButtonMMCM);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.button1);
            this.Name = "AskName";
            this.Text = "AskName";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.RadioButton radioButtonMMCM;
        private System.Windows.Forms.RadioButton radioButtonCTPC;
    }
}