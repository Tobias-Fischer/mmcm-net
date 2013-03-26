namespace MMCMLibrary
{
    partial class MMCM_ControlPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MMCM_ControlPanel));
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.buttonRun = new System.Windows.Forms.Button();
            this.labelSteps = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageParameters = new System.Windows.Forms.TabPage();
            this.checkBoxLogError = new System.Windows.Forms.CheckBox();
            this.feedbakcTextBoxInfluence = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.gridModalitiesParameters = new System.Windows.Forms.DataGridView();
            this.Columun_ModalityName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_EnactionFactor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Influence = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPageModalities = new System.Windows.Forms.TabPage();
            this.flowLayoutPanelModalities = new System.Windows.Forms.FlowLayoutPanel();
            this.tabPageErrors = new System.Windows.Forms.TabPage();
            this.predictionErrorCheckBoxLog = new System.Windows.Forms.CheckBox();
            this.chartButtonSavePlot = new System.Windows.Forms.Button();
            this.chartCheckListModalities = new System.Windows.Forms.CheckedListBox();
            this.chartButtonResetTime = new System.Windows.Forms.Button();
            this.chartPredictionError = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tabPageElectrodes = new System.Windows.Forms.TabPage();
            this.electrodeButtonResetPlot = new System.Windows.Forms.Button();
            this.electrodesButtonSavePlot = new System.Windows.Forms.Button();
            this.electrodeCheckBoxPlot = new System.Windows.Forms.CheckBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.electrodesCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.electrodeButtonAdd = new System.Windows.Forms.Button();
            this.chartElectrodes = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.electrodeTextBoxIntensity = new System.Windows.Forms.TextBox();
            this.electrodeTextBoxRadius = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.labelMapInfo = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonNewMap = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonLoadMap = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSaveMap = new System.Windows.Forms.ToolStripButton();
            this.buttonStop = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBoxLearningRate = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxSigma = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.buttonGetParameters = new System.Windows.Forms.Button();
            this.buttonSetParameters = new System.Windows.Forms.Button();
            this.textBoxPeriod = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPageParameters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridModalitiesParameters)).BeginInit();
            this.tabPageModalities.SuspendLayout();
            this.tabPageErrors.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartPredictionError)).BeginInit();
            this.tabPageElectrodes.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartElectrodes)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonRun
            // 
            this.buttonRun.Image = ((System.Drawing.Image)(resources.GetObject("buttonRun.Image")));
            this.buttonRun.Location = new System.Drawing.Point(217, 0);
            this.buttonRun.Name = "buttonRun";
            this.buttonRun.Size = new System.Drawing.Size(32, 36);
            this.buttonRun.TabIndex = 4;
            this.buttonRun.UseVisualStyleBackColor = true;
            this.buttonRun.Click += new System.EventHandler(this.buttonRun_Click);
            // 
            // labelSteps
            // 
            this.labelSteps.AutoSize = true;
            this.labelSteps.Location = new System.Drawing.Point(12, 404);
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
            this.panel1.Size = new System.Drawing.Size(237, 200);
            this.panel1.TabIndex = 6;
            this.panel1.WrapContents = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageParameters);
            this.tabControl1.Controls.Add(this.tabPageModalities);
            this.tabControl1.Controls.Add(this.tabPageErrors);
            this.tabControl1.Controls.Add(this.tabPageElectrodes);
            this.tabControl1.Location = new System.Drawing.Point(255, 28);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(496, 373);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
            this.tabControl1.TabIndex = 7;
            // 
            // tabPageParameters
            // 
            this.tabPageParameters.AutoScroll = true;
            this.tabPageParameters.BackColor = System.Drawing.Color.LightGray;
            this.tabPageParameters.Controls.Add(this.groupBox2);
            this.tabPageParameters.Controls.Add(this.gridModalitiesParameters);
            this.tabPageParameters.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tabPageParameters.Location = new System.Drawing.Point(4, 22);
            this.tabPageParameters.Name = "tabPageParameters";
            this.tabPageParameters.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageParameters.Size = new System.Drawing.Size(488, 347);
            this.tabPageParameters.TabIndex = 0;
            this.tabPageParameters.Text = "Parameters";
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
            this.checkBoxLogError.CheckedChanged += new System.EventHandler(this.checkBoxLogError_CheckedChanged);
            // 
            // feedbakcTextBoxInfluence
            // 
            this.feedbakcTextBoxInfluence.Location = new System.Drawing.Point(6, 75);
            this.feedbakcTextBoxInfluence.Name = "feedbakcTextBoxInfluence";
            this.feedbakcTextBoxInfluence.Size = new System.Drawing.Size(30, 20);
            this.feedbakcTextBoxInfluence.TabIndex = 12;
            this.feedbakcTextBoxInfluence.Text = "1,0";
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
            // gridModalitiesParameters
            // 
            this.gridModalitiesParameters.AllowUserToAddRows = false;
            this.gridModalitiesParameters.AllowUserToDeleteRows = false;
            this.gridModalitiesParameters.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridModalitiesParameters.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Columun_ModalityName,
            this.Column_EnactionFactor,
            this.Column_Influence});
            this.gridModalitiesParameters.Location = new System.Drawing.Point(6, 145);
            this.gridModalitiesParameters.Name = "gridModalitiesParameters";
            this.gridModalitiesParameters.Size = new System.Drawing.Size(268, 149);
            this.gridModalitiesParameters.TabIndex = 4;
            // 
            // Columun_ModalityName
            // 
            this.Columun_ModalityName.HeaderText = "Modality";
            this.Columun_ModalityName.Name = "Columun_ModalityName";
            this.Columun_ModalityName.ReadOnly = true;
            this.Columun_ModalityName.Width = 75;
            // 
            // Column_EnactionFactor
            // 
            this.Column_EnactionFactor.HeaderText = "Enaction Factor";
            this.Column_EnactionFactor.Name = "Column_EnactionFactor";
            this.Column_EnactionFactor.Width = 75;
            // 
            // Column_Influence
            // 
            this.Column_Influence.HeaderText = "Influence";
            this.Column_Influence.Name = "Column_Influence";
            this.Column_Influence.Width = 75;
            // 
            // tabPageModalities
            // 
            this.tabPageModalities.AutoScroll = true;
            this.tabPageModalities.Controls.Add(this.flowLayoutPanelModalities);
            this.tabPageModalities.Location = new System.Drawing.Point(4, 22);
            this.tabPageModalities.Name = "tabPageModalities";
            this.tabPageModalities.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageModalities.Size = new System.Drawing.Size(488, 347);
            this.tabPageModalities.TabIndex = 1;
            this.tabPageModalities.Text = "Modalities";
            this.tabPageModalities.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanelModalities
            // 
            this.flowLayoutPanelModalities.AutoScroll = true;
            this.flowLayoutPanelModalities.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanelModalities.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanelModalities.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanelModalities.Name = "flowLayoutPanelModalities";
            this.flowLayoutPanelModalities.Size = new System.Drawing.Size(482, 341);
            this.flowLayoutPanelModalities.TabIndex = 0;
            this.flowLayoutPanelModalities.WrapContents = false;
            // 
            // tabPageErrors
            // 
            this.tabPageErrors.Controls.Add(this.predictionErrorCheckBoxLog);
            this.tabPageErrors.Controls.Add(this.chartButtonSavePlot);
            this.tabPageErrors.Controls.Add(this.chartCheckListModalities);
            this.tabPageErrors.Controls.Add(this.chartButtonResetTime);
            this.tabPageErrors.Controls.Add(this.chartPredictionError);
            this.tabPageErrors.Location = new System.Drawing.Point(4, 22);
            this.tabPageErrors.Name = "tabPageErrors";
            this.tabPageErrors.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageErrors.Size = new System.Drawing.Size(488, 347);
            this.tabPageErrors.TabIndex = 2;
            this.tabPageErrors.Text = "Prediction Errors";
            this.tabPageErrors.UseVisualStyleBackColor = true;
            // 
            // predictionErrorCheckBoxLog
            // 
            this.predictionErrorCheckBoxLog.AutoSize = true;
            this.predictionErrorCheckBoxLog.Location = new System.Drawing.Point(7, 10);
            this.predictionErrorCheckBoxLog.Name = "predictionErrorCheckBoxLog";
            this.predictionErrorCheckBoxLog.Size = new System.Drawing.Size(78, 17);
            this.predictionErrorCheckBoxLog.TabIndex = 4;
            this.predictionErrorCheckBoxLog.Text = "enable plot";
            this.predictionErrorCheckBoxLog.UseVisualStyleBackColor = true;
            // 
            // chartButtonSavePlot
            // 
            this.chartButtonSavePlot.Location = new System.Drawing.Point(7, 63);
            this.chartButtonSavePlot.Name = "chartButtonSavePlot";
            this.chartButtonSavePlot.Size = new System.Drawing.Size(75, 23);
            this.chartButtonSavePlot.TabIndex = 3;
            this.chartButtonSavePlot.Text = "Save Plot";
            this.chartButtonSavePlot.UseVisualStyleBackColor = true;
            this.chartButtonSavePlot.Click += new System.EventHandler(this.chartButtonSavePlot_Click);
            // 
            // chartCheckListModalities
            // 
            this.chartCheckListModalities.FormattingEnabled = true;
            this.chartCheckListModalities.Location = new System.Drawing.Point(7, 93);
            this.chartCheckListModalities.Name = "chartCheckListModalities";
            this.chartCheckListModalities.Size = new System.Drawing.Size(120, 364);
            this.chartCheckListModalities.TabIndex = 2;
            this.chartCheckListModalities.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.chartCheckListModalities_ItemCheck);
            // 
            // chartButtonResetTime
            // 
            this.chartButtonResetTime.Location = new System.Drawing.Point(7, 33);
            this.chartButtonResetTime.Name = "chartButtonResetTime";
            this.chartButtonResetTime.Size = new System.Drawing.Size(75, 23);
            this.chartButtonResetTime.TabIndex = 1;
            this.chartButtonResetTime.Text = "Reset Time";
            this.chartButtonResetTime.UseVisualStyleBackColor = true;
            this.chartButtonResetTime.Click += new System.EventHandler(this.chartButtonResetTime_Click);
            // 
            // chartPredictionError
            // 
            chartArea1.Name = "ChartArea1";
            this.chartPredictionError.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chartPredictionError.Legends.Add(legend1);
            this.chartPredictionError.Location = new System.Drawing.Point(133, 6);
            this.chartPredictionError.Name = "chartPredictionError";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chartPredictionError.Series.Add(series1);
            this.chartPredictionError.Size = new System.Drawing.Size(297, 242);
            this.chartPredictionError.TabIndex = 0;
            this.chartPredictionError.Text = "chart1";
            title1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
            title1.Name = "Title1";
            title1.Text = "Prediction Error";
            this.chartPredictionError.Titles.Add(title1);
            // 
            // tabPageElectrodes
            // 
            this.tabPageElectrodes.Controls.Add(this.electrodeButtonResetPlot);
            this.tabPageElectrodes.Controls.Add(this.electrodesButtonSavePlot);
            this.tabPageElectrodes.Controls.Add(this.electrodeCheckBoxPlot);
            this.tabPageElectrodes.Controls.Add(this.groupBox5);
            this.tabPageElectrodes.Controls.Add(this.chartElectrodes);
            this.tabPageElectrodes.Controls.Add(this.groupBox3);
            this.tabPageElectrodes.Location = new System.Drawing.Point(4, 22);
            this.tabPageElectrodes.Name = "tabPageElectrodes";
            this.tabPageElectrodes.Size = new System.Drawing.Size(488, 347);
            this.tabPageElectrodes.TabIndex = 3;
            this.tabPageElectrodes.Text = "Electrodes";
            this.tabPageElectrodes.UseVisualStyleBackColor = true;
            // 
            // electrodeButtonResetPlot
            // 
            this.electrodeButtonResetPlot.Location = new System.Drawing.Point(9, 21);
            this.electrodeButtonResetPlot.Name = "electrodeButtonResetPlot";
            this.electrodeButtonResetPlot.Size = new System.Drawing.Size(75, 23);
            this.electrodeButtonResetPlot.TabIndex = 18;
            this.electrodeButtonResetPlot.Text = "Reset Plot";
            this.electrodeButtonResetPlot.UseVisualStyleBackColor = true;
            this.electrodeButtonResetPlot.Click += new System.EventHandler(this.electrodeButtonResetPlot_Click);
            // 
            // electrodesButtonSavePlot
            // 
            this.electrodesButtonSavePlot.Location = new System.Drawing.Point(9, 46);
            this.electrodesButtonSavePlot.Name = "electrodesButtonSavePlot";
            this.electrodesButtonSavePlot.Size = new System.Drawing.Size(75, 23);
            this.electrodesButtonSavePlot.TabIndex = 8;
            this.electrodesButtonSavePlot.Text = "Save Plot";
            this.electrodesButtonSavePlot.UseVisualStyleBackColor = true;
            this.electrodesButtonSavePlot.Click += new System.EventHandler(this.electrodesButtonSavePlot_Click);
            // 
            // electrodeCheckBoxPlot
            // 
            this.electrodeCheckBoxPlot.AutoSize = true;
            this.electrodeCheckBoxPlot.Location = new System.Drawing.Point(9, 4);
            this.electrodeCheckBoxPlot.Name = "electrodeCheckBoxPlot";
            this.electrodeCheckBoxPlot.Size = new System.Drawing.Size(80, 17);
            this.electrodeCheckBoxPlot.TabIndex = 7;
            this.electrodeCheckBoxPlot.Text = "Enable Plot";
            this.electrodeCheckBoxPlot.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.electrodesCheckedListBox);
            this.groupBox5.Controls.Add(this.electrodeButtonAdd);
            this.groupBox5.Location = new System.Drawing.Point(95, 4);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(246, 100);
            this.groupBox5.TabIndex = 6;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Electrodes List";
            // 
            // electrodesCheckedListBox
            // 
            this.electrodesCheckedListBox.FormattingEnabled = true;
            this.electrodesCheckedListBox.Location = new System.Drawing.Point(7, 18);
            this.electrodesCheckedListBox.Name = "electrodesCheckedListBox";
            this.electrodesCheckedListBox.Size = new System.Drawing.Size(184, 64);
            this.electrodesCheckedListBox.TabIndex = 1;
            this.electrodesCheckedListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.electrodesCheckedListBox_ItemCheck);
            this.electrodesCheckedListBox.SelectedIndexChanged += new System.EventHandler(this.electrodesCheckedListBox_SelectedIndexChanged);
            // 
            // electrodeButtonAdd
            // 
            this.electrodeButtonAdd.Location = new System.Drawing.Point(197, 18);
            this.electrodeButtonAdd.Name = "electrodeButtonAdd";
            this.electrodeButtonAdd.Size = new System.Drawing.Size(43, 23);
            this.electrodeButtonAdd.TabIndex = 0;
            this.electrodeButtonAdd.Text = "Add";
            this.electrodeButtonAdd.UseVisualStyleBackColor = true;
            this.electrodeButtonAdd.Click += new System.EventHandler(this.electrodeButtonAdd_Click);
            // 
            // chartElectrodes
            // 
            chartArea2.Name = "ChartArea1";
            this.chartElectrodes.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.chartElectrodes.Legends.Add(legend2);
            this.chartElectrodes.Location = new System.Drawing.Point(9, 110);
            this.chartElectrodes.Name = "chartElectrodes";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.chartElectrodes.Series.Add(series2);
            this.chartElectrodes.Size = new System.Drawing.Size(470, 196);
            this.chartElectrodes.TabIndex = 5;
            this.chartElectrodes.Text = "chart1";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.electrodeTextBoxIntensity);
            this.groupBox3.Controls.Add(this.electrodeTextBoxRadius);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Location = new System.Drawing.Point(347, 4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(132, 100);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Electrode Parameters";
            // 
            // electrodeTextBoxIntensity
            // 
            this.electrodeTextBoxIntensity.Enabled = false;
            this.electrodeTextBoxIntensity.Location = new System.Drawing.Point(63, 19);
            this.electrodeTextBoxIntensity.Name = "electrodeTextBoxIntensity";
            this.electrodeTextBoxIntensity.Size = new System.Drawing.Size(30, 20);
            this.electrodeTextBoxIntensity.TabIndex = 15;
            this.electrodeTextBoxIntensity.Text = "0,0";
            // 
            // electrodeTextBoxRadius
            // 
            this.electrodeTextBoxRadius.Enabled = false;
            this.electrodeTextBoxRadius.Location = new System.Drawing.Point(63, 45);
            this.electrodeTextBoxRadius.Name = "electrodeTextBoxRadius";
            this.electrodeTextBoxRadius.Size = new System.Drawing.Size(30, 20);
            this.electrodeTextBoxRadius.TabIndex = 17;
            this.electrodeTextBoxRadius.Text = "1,0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Enabled = false;
            this.label2.Location = new System.Drawing.Point(6, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Intensity";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Enabled = false;
            this.label3.Location = new System.Drawing.Point(6, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "Radius";
            // 
            // labelMapInfo
            // 
            this.labelMapInfo.AutoSize = true;
            this.labelMapInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelMapInfo.Location = new System.Drawing.Point(12, 245);
            this.labelMapInfo.Name = "labelMapInfo";
            this.labelMapInfo.Size = new System.Drawing.Size(61, 15);
            this.labelMapInfo.TabIndex = 8;
            this.labelMapInfo.Text = "Map infos :";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonNewMap,
            this.toolStripButtonLoadMap,
            this.toolStripButtonSaveMap});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(758, 25);
            this.toolStrip1.TabIndex = 8;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButtonNewMap
            // 
            this.toolStripButtonNewMap.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonNewMap.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonNewMap.Image")));
            this.toolStripButtonNewMap.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonNewMap.Name = "toolStripButtonNewMap";
            this.toolStripButtonNewMap.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonNewMap.Text = "Create a new map (not implemented)";
            this.toolStripButtonNewMap.Click += new System.EventHandler(this.toolStripButtonNewMap_Click);
            // 
            // toolStripButtonLoadMap
            // 
            this.toolStripButtonLoadMap.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonLoadMap.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonLoadMap.Image")));
            this.toolStripButtonLoadMap.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonLoadMap.Name = "toolStripButtonLoadMap";
            this.toolStripButtonLoadMap.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonLoadMap.Text = "Load an existing map";
            this.toolStripButtonLoadMap.Click += new System.EventHandler(this.toolStripButtonLoadMap_Click);
            // 
            // toolStripButtonSaveMap
            // 
            this.toolStripButtonSaveMap.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSaveMap.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSaveMap.Image")));
            this.toolStripButtonSaveMap.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSaveMap.Name = "toolStripButtonSaveMap";
            this.toolStripButtonSaveMap.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonSaveMap.Text = "Save the current map";
            this.toolStripButtonSaveMap.Click += new System.EventHandler(this.toolStripButtonSaveMap_Click);
            // 
            // buttonStop
            // 
            this.buttonStop.Enabled = false;
            this.buttonStop.Image = ((System.Drawing.Image)(resources.GetObject("buttonStop.Image")));
            this.buttonStop.Location = new System.Drawing.Point(217, 0);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(32, 36);
            this.buttonStop.TabIndex = 9;
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Visible = false;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
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
            this.groupBox2.Location = new System.Drawing.Point(6, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(268, 133);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Parameters";
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
            // textBoxSigma
            // 
            this.textBoxSigma.Location = new System.Drawing.Point(6, 45);
            this.textBoxSigma.Name = "textBoxSigma";
            this.textBoxSigma.Size = new System.Drawing.Size(30, 20);
            this.textBoxSigma.TabIndex = 16;
            this.textBoxSigma.Text = "1,0";
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
            // buttonGetParameters
            // 
            this.buttonGetParameters.Location = new System.Drawing.Point(167, 41);
            this.buttonGetParameters.Name = "buttonGetParameters";
            this.buttonGetParameters.Size = new System.Drawing.Size(95, 23);
            this.buttonGetParameters.TabIndex = 18;
            this.buttonGetParameters.Text = "Get parameters";
            this.buttonGetParameters.UseVisualStyleBackColor = true;
            // 
            // buttonSetParameters
            // 
            this.buttonSetParameters.Location = new System.Drawing.Point(167, 70);
            this.buttonSetParameters.Name = "buttonSetParameters";
            this.buttonSetParameters.Size = new System.Drawing.Size(95, 23);
            this.buttonSetParameters.TabIndex = 17;
            this.buttonSetParameters.Text = "Set parameters";
            this.buttonSetParameters.UseVisualStyleBackColor = true;
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
            // MMCM_ControlPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(758, 424);
            this.Controls.Add(this.buttonRun);
            this.Controls.Add(this.buttonStop);
            this.Controls.Add(this.labelMapInfo);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.labelSteps);
            this.Name = "MMCM_ControlPanel";
            this.Text = "MMCM_ControlPanel";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MMCM_ControlPanel_FormClosing);
            this.tabControl1.ResumeLayout(false);
            this.tabPageParameters.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridModalitiesParameters)).EndInit();
            this.tabPageModalities.ResumeLayout(false);
            this.tabPageErrors.ResumeLayout(false);
            this.tabPageErrors.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartPredictionError)).EndInit();
            this.tabPageElectrodes.ResumeLayout(false);
            this.tabPageElectrodes.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartElectrodes)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonRun;
        private System.Windows.Forms.Label labelSteps;
        private System.Windows.Forms.FlowLayoutPanel panel1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageParameters;
        private System.Windows.Forms.Label labelMapInfo;
        private System.Windows.Forms.DataGridView gridModalitiesParameters;
        private System.Windows.Forms.TabPage tabPageModalities;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelModalities;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonNewMap;
        private System.Windows.Forms.ToolStripButton toolStripButtonLoadMap;
        private System.Windows.Forms.ToolStripButton toolStripButtonSaveMap;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Label label1;
	   private System.Windows.Forms.TextBox feedbakcTextBoxInfluence;
        private System.Windows.Forms.TabPage tabPageErrors;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartPredictionError;
        private System.Windows.Forms.CheckedListBox chartCheckListModalities;
        private System.Windows.Forms.Button chartButtonResetTime;
        private System.Windows.Forms.Button chartButtonSavePlot;
        private System.Windows.Forms.TabPage tabPageElectrodes;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.CheckedListBox electrodesCheckedListBox;
        private System.Windows.Forms.Button electrodeButtonAdd;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartElectrodes;
        private System.Windows.Forms.Button electrodeButtonResetPlot;
        private System.Windows.Forms.TextBox electrodeTextBoxRadius;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox electrodeTextBoxIntensity;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox predictionErrorCheckBoxLog;
        private System.Windows.Forms.CheckBox electrodeCheckBoxPlot;
        private System.Windows.Forms.Button electrodesButtonSavePlot;
	   private System.Windows.Forms.CheckBox checkBoxLogError;
	   private System.Windows.Forms.DataGridViewTextBoxColumn Columun_ModalityName;
	   private System.Windows.Forms.DataGridViewTextBoxColumn Column_EnactionFactor;
       private System.Windows.Forms.DataGridViewTextBoxColumn Column_Influence;
       private System.Windows.Forms.GroupBox groupBox2;
       private System.Windows.Forms.Button buttonGetParameters;
       private System.Windows.Forms.Button buttonSetParameters;
       private System.Windows.Forms.TextBox textBoxSigma;
       private System.Windows.Forms.Label label5;
       private System.Windows.Forms.TextBox textBoxLearningRate;
       private System.Windows.Forms.Label label4;
       private System.Windows.Forms.TextBox textBoxPeriod;
       private System.Windows.Forms.Label label6;

    }
}