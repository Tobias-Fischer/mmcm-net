namespace MMCMLibrary.Modalities
{
    partial class StringModalityControl
    {
        /// <summary> 
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.labelName = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.labelReal = new System.Windows.Forms.Label();
            this.labelPerceived = new System.Windows.Forms.Label();
            this.labelPredicted = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(4, 4);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(35, 13);
            this.labelName.TabIndex = 1;
            this.labelName.Text = "Name";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Real:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Perceived:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Predicted:";
            // 
            // labelReal
            // 
            this.labelReal.AutoSize = true;
            this.labelReal.Location = new System.Drawing.Point(70, 26);
            this.labelReal.Name = "labelReal";
            this.labelReal.Size = new System.Drawing.Size(30, 13);
            this.labelReal.TabIndex = 5;
            this.labelReal.Text = "word";
            // 
            // labelPerceived
            // 
            this.labelPerceived.AutoSize = true;
            this.labelPerceived.Location = new System.Drawing.Point(70, 39);
            this.labelPerceived.Name = "labelPerceived";
            this.labelPerceived.Size = new System.Drawing.Size(30, 13);
            this.labelPerceived.TabIndex = 6;
            this.labelPerceived.Text = "word";
            // 
            // labelPredicted
            // 
            this.labelPredicted.AutoSize = true;
            this.labelPredicted.Location = new System.Drawing.Point(70, 52);
            this.labelPredicted.Name = "labelPredicted";
            this.labelPredicted.Size = new System.Drawing.Size(30, 13);
            this.labelPredicted.TabIndex = 7;
            this.labelPredicted.Text = "word";
            // 
            // StringModalityControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.labelPredicted);
            this.Controls.Add(this.labelPerceived);
            this.Controls.Add(this.labelReal);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelName);
            this.Name = "StringModalityControl";
            this.Size = new System.Drawing.Size(103, 65);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelReal;
        private System.Windows.Forms.Label labelPerceived;
        private System.Windows.Forms.Label labelPredicted;
    }
}
