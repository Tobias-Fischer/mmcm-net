namespace RobotConnector
{
    partial class OverviewForm
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

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridViewMotors = new System.Windows.Forms.DataGridView();
            this.ColumnHead = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnLeftArm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnRightArm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnLeftLeg = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnRightLeg = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnTorso = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMotors)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Robot Connected";
            // 
            // dataGridViewMotors
            // 
            this.dataGridViewMotors.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewMotors.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewMotors.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnHead,
            this.ColumnLeftArm,
            this.ColumnRightArm,
            this.ColumnLeftLeg,
            this.ColumnRightLeg,
            this.ColumnTorso});
            this.dataGridViewMotors.Location = new System.Drawing.Point(9, 29);
            this.dataGridViewMotors.Name = "dataGridViewMotors";
            this.dataGridViewMotors.Size = new System.Drawing.Size(640, 359);
            this.dataGridViewMotors.TabIndex = 1;
            // 
            // ColumnHead
            // 
            this.ColumnHead.HeaderText = "Head";
            this.ColumnHead.Name = "ColumnHead";
            // 
            // ColumnLeftArm
            // 
            this.ColumnLeftArm.HeaderText = "LeftArm";
            this.ColumnLeftArm.Name = "ColumnLeftArm";
            // 
            // ColumnRightArm
            // 
            this.ColumnRightArm.HeaderText = "RightArm";
            this.ColumnRightArm.Name = "ColumnRightArm";
            // 
            // ColumnLeftLeg
            // 
            this.ColumnLeftLeg.HeaderText = "LeftLeg";
            this.ColumnLeftLeg.Name = "ColumnLeftLeg";
            // 
            // ColumnRightLeg
            // 
            this.ColumnRightLeg.HeaderText = "RightLeg";
            this.ColumnRightLeg.Name = "ColumnRightLeg";
            // 
            // ColumnTorso
            // 
            this.ColumnTorso.HeaderText = "Torso";
            this.ColumnTorso.Name = "ColumnTorso";
            // 
            // OverviewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(663, 400);
            this.Controls.Add(this.dataGridViewMotors);
            this.Controls.Add(this.label1);
            this.Name = "OverviewForm";
            this.Text = "Robot Connector";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMotors)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridViewMotors;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnHead;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnLeftArm;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnRightArm;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnLeftLeg;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnRightLeg;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnTorso;
    }
}

