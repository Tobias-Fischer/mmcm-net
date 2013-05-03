namespace CVZ_Core.GUI.Modalities
{
    partial class IModalityControl
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
            this.dataGridViewModalities = new System.Windows.Forms.DataGridView();
            this.Real = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Perceived = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Predicted = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.labelName = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewModalities)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewModalities
            // 
            this.dataGridViewModalities.AllowUserToDeleteRows = false;
            this.dataGridViewModalities.AllowUserToResizeColumns = false;
            this.dataGridViewModalities.AllowUserToResizeRows = false;
            this.dataGridViewModalities.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridViewModalities.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridViewModalities.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewModalities.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Real,
            this.Perceived,
            this.Predicted});
            this.dataGridViewModalities.Location = new System.Drawing.Point(0, 26);
            this.dataGridViewModalities.Name = "dataGridViewModalities";
            this.dataGridViewModalities.ReadOnly = true;
            this.dataGridViewModalities.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridViewModalities.Size = new System.Drawing.Size(254, 512);
            this.dataGridViewModalities.TabIndex = 0;
            // 
            // Real
            // 
            this.Real.HeaderText = "Real";
            this.Real.Name = "Real";
            this.Real.ReadOnly = true;
            this.Real.Width = 54;
            // 
            // Perceived
            // 
            this.Perceived.HeaderText = "Perceived";
            this.Perceived.Name = "Perceived";
            this.Perceived.ReadOnly = true;
            this.Perceived.Width = 80;
            // 
            // Predicted
            // 
            this.Predicted.HeaderText = "Predicted";
            this.Predicted.Name = "Predicted";
            this.Predicted.ReadOnly = true;
            this.Predicted.Width = 77;
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
            // IModalityControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.labelName);
            this.Controls.Add(this.dataGridViewModalities);
            this.Name = "IModalityControl";
            this.Size = new System.Drawing.Size(257, 541);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewModalities)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewModalities;
        private System.Windows.Forms.DataGridViewTextBoxColumn Real;
        private System.Windows.Forms.DataGridViewTextBoxColumn Perceived;
        private System.Windows.Forms.DataGridViewTextBoxColumn Predicted;
        private System.Windows.Forms.Label labelName;
    }
}
