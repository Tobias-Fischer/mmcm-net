namespace RobotConnector
{
    partial class AnimatorCtrl
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
		  this.groupBox1 = new System.Windows.Forms.GroupBox();
		  this.buttonRecord = new System.Windows.Forms.Button();
		  this.label1 = new System.Windows.Forms.Label();
		  this.textBoxName = new System.Windows.Forms.TextBox();
		  this.listBoxAnimations = new System.Windows.Forms.ListBox();
		  this.buttonStop = new System.Windows.Forms.Button();
		  this.buttonPlay = new System.Windows.Forms.Button();
		  this.groupBox1.SuspendLayout();
		  this.SuspendLayout();
		  // 
		  // groupBox1
		  // 
		  this.groupBox1.Controls.Add(this.buttonPlay);
		  this.groupBox1.Controls.Add(this.buttonStop);
		  this.groupBox1.Controls.Add(this.buttonRecord);
		  this.groupBox1.Controls.Add(this.label1);
		  this.groupBox1.Controls.Add(this.textBoxName);
		  this.groupBox1.Controls.Add(this.listBoxAnimations);
		  this.groupBox1.Location = new System.Drawing.Point(3, 3);
		  this.groupBox1.Name = "groupBox1";
		  this.groupBox1.Size = new System.Drawing.Size(200, 186);
		  this.groupBox1.TabIndex = 1;
		  this.groupBox1.TabStop = false;
		  this.groupBox1.Text = "Hand Left";
		  // 
		  // buttonRecord
		  // 
		  this.buttonRecord.Location = new System.Drawing.Point(6, 146);
		  this.buttonRecord.Name = "buttonRecord";
		  this.buttonRecord.Size = new System.Drawing.Size(67, 23);
		  this.buttonRecord.TabIndex = 3;
		  this.buttonRecord.Text = "record";
		  this.buttonRecord.UseVisualStyleBackColor = true;
		  this.buttonRecord.Click += new System.EventHandler(this.buttonRecord_Click);
		  // 
		  // label1
		  // 
		  this.label1.AutoSize = true;
		  this.label1.Location = new System.Drawing.Point(6, 123);
		  this.label1.Name = "label1";
		  this.label1.Size = new System.Drawing.Size(33, 13);
		  this.label1.TabIndex = 2;
		  this.label1.Text = "name";
		  // 
		  // textBoxName
		  // 
		  this.textBoxName.Location = new System.Drawing.Point(45, 120);
		  this.textBoxName.Name = "textBoxName";
		  this.textBoxName.Size = new System.Drawing.Size(149, 20);
		  this.textBoxName.TabIndex = 1;
		  // 
		  // listBoxAnimations
		  // 
		  this.listBoxAnimations.FormattingEnabled = true;
		  this.listBoxAnimations.Location = new System.Drawing.Point(6, 19);
		  this.listBoxAnimations.Name = "listBoxAnimations";
		  this.listBoxAnimations.Size = new System.Drawing.Size(188, 95);
		  this.listBoxAnimations.TabIndex = 0;
		  // 
		  // buttonStop
		  // 
		  this.buttonStop.Enabled = false;
		  this.buttonStop.Location = new System.Drawing.Point(79, 146);
		  this.buttonStop.Name = "buttonStop";
		  this.buttonStop.Size = new System.Drawing.Size(48, 23);
		  this.buttonStop.TabIndex = 4;
		  this.buttonStop.Text = "stop";
		  this.buttonStop.UseVisualStyleBackColor = true;
		  this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
		  // 
		  // buttonPlay
		  // 
		  this.buttonPlay.Location = new System.Drawing.Point(133, 146);
		  this.buttonPlay.Name = "buttonPlay";
		  this.buttonPlay.Size = new System.Drawing.Size(48, 23);
		  this.buttonPlay.TabIndex = 5;
		  this.buttonPlay.Text = "play";
		  this.buttonPlay.UseVisualStyleBackColor = true;
		  this.buttonPlay.Click += new System.EventHandler(this.buttonPlay_Click);
		  // 
		  // AnimatorCtrl
		  // 
		  this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
		  this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		  this.Controls.Add(this.groupBox1);
		  this.Name = "AnimatorCtrl";
		  this.Size = new System.Drawing.Size(210, 198);
		  this.groupBox1.ResumeLayout(false);
		  this.groupBox1.PerformLayout();
		  this.ResumeLayout(false);

	   }

	   #endregion

	   private System.Windows.Forms.GroupBox groupBox1;
	   private System.Windows.Forms.Button buttonRecord;
	   private System.Windows.Forms.Label label1;
	   private System.Windows.Forms.TextBox textBoxName;
	   private System.Windows.Forms.ListBox listBoxAnimations;
	   private System.Windows.Forms.Button buttonStop;
	   private System.Windows.Forms.Button buttonPlay;

    }
}
