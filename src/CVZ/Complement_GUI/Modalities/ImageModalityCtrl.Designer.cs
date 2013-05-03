namespace CVZ_Core.GUI.Modalities
{
    partial class ImageModalityCtrl
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
		  this.pictureBoxReal = new System.Windows.Forms.PictureBox();
		  this.pictureBoxPerceived = new System.Windows.Forms.PictureBox();
		  this.pictureBoxPredicted = new System.Windows.Forms.PictureBox();
		  this.labelName = new System.Windows.Forms.Label();
		  this.label1 = new System.Windows.Forms.Label();
		  this.label2 = new System.Windows.Forms.Label();
		  this.label3 = new System.Windows.Forms.Label();
		  ((System.ComponentModel.ISupportInitialize)(this.pictureBoxReal)).BeginInit();
		  ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPerceived)).BeginInit();
		  ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPredicted)).BeginInit();
		  this.SuspendLayout();
		  // 
		  // pictureBoxReal
		  // 
		  this.pictureBoxReal.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
		  this.pictureBoxReal.Location = new System.Drawing.Point(7, 38);
		  this.pictureBoxReal.Name = "pictureBoxReal";
		  this.pictureBoxReal.Size = new System.Drawing.Size(108, 119);
		  this.pictureBoxReal.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		  this.pictureBoxReal.TabIndex = 0;
		  this.pictureBoxReal.TabStop = false;
		  // 
		  // pictureBoxPerceived
		  // 
		  this.pictureBoxPerceived.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
		  this.pictureBoxPerceived.Location = new System.Drawing.Point(245, 38);
		  this.pictureBoxPerceived.Name = "pictureBoxPerceived";
		  this.pictureBoxPerceived.Size = new System.Drawing.Size(112, 119);
		  this.pictureBoxPerceived.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		  this.pictureBoxPerceived.TabIndex = 1;
		  this.pictureBoxPerceived.TabStop = false;
		  // 
		  // pictureBoxPredicted
		  // 
		  this.pictureBoxPredicted.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
		  this.pictureBoxPredicted.Location = new System.Drawing.Point(121, 38);
		  this.pictureBoxPredicted.Name = "pictureBoxPredicted";
		  this.pictureBoxPredicted.Size = new System.Drawing.Size(118, 119);
		  this.pictureBoxPredicted.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		  this.pictureBoxPredicted.TabIndex = 2;
		  this.pictureBoxPredicted.TabStop = false;
		  // 
		  // labelName
		  // 
		  this.labelName.AutoSize = true;
		  this.labelName.Location = new System.Drawing.Point(4, 4);
		  this.labelName.Name = "labelName";
		  this.labelName.Size = new System.Drawing.Size(35, 13);
		  this.labelName.TabIndex = 3;
		  this.labelName.Text = "Name";
		  // 
		  // label1
		  // 
		  this.label1.AutoSize = true;
		  this.label1.Location = new System.Drawing.Point(4, 22);
		  this.label1.Name = "label1";
		  this.label1.Size = new System.Drawing.Size(29, 13);
		  this.label1.TabIndex = 4;
		  this.label1.Text = "Real";
		  // 
		  // label2
		  // 
		  this.label2.AutoSize = true;
		  this.label2.Location = new System.Drawing.Point(242, 22);
		  this.label2.Name = "label2";
		  this.label2.Size = new System.Drawing.Size(55, 13);
		  this.label2.TabIndex = 5;
		  this.label2.Text = "Perceived";
		  // 
		  // label3
		  // 
		  this.label3.AutoSize = true;
		  this.label3.Location = new System.Drawing.Point(121, 22);
		  this.label3.Name = "label3";
		  this.label3.Size = new System.Drawing.Size(52, 13);
		  this.label3.TabIndex = 6;
		  this.label3.Text = "Predicted";
		  // 
		  // ImageModalityCtrl
		  // 
		  this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
		  this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		  this.AutoScroll = true;
		  this.AutoSize = true;
		  this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
		  this.Controls.Add(this.label3);
		  this.Controls.Add(this.label2);
		  this.Controls.Add(this.label1);
		  this.Controls.Add(this.labelName);
		  this.Controls.Add(this.pictureBoxPredicted);
		  this.Controls.Add(this.pictureBoxPerceived);
		  this.Controls.Add(this.pictureBoxReal);
		  this.Name = "ImageModalityCtrl";
		  this.Size = new System.Drawing.Size(360, 160);
		  ((System.ComponentModel.ISupportInitialize)(this.pictureBoxReal)).EndInit();
		  ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPerceived)).EndInit();
		  ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPredicted)).EndInit();
		  this.ResumeLayout(false);
		  this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxReal;
        private System.Windows.Forms.PictureBox pictureBoxPerceived;
        private System.Windows.Forms.PictureBox pictureBoxPredicted;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}
