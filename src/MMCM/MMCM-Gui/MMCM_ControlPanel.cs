using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MMCMLibrary.Modalities;

using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace MMCMLibrary
{
    public partial class MMCM_ControlPanel : Form
    {
        CVZ_MMCM m_map = null;
        Dictionary<string,float> m_enactionFactor;
        List<PictureBox> m_layers;
        MMCM_Electrode currentManipulatedElectrode = null;

        /// <summary>
        /// Contains the errors over time for every modality
        /// </summary>
        //Dictionary<string, List<float>> errorLogs = new Dictionary<string,List<float>>();

        public bool displayModalities = true;
        int m_stepsDone = 0;

        int m_period = 100;
        System.Threading.Thread m_mapThread;

        public MMCM_ControlPanel()
        {
            InitializeComponent();
            onStimulationDone += RefreshGUI;
            //Generate a new map
            toolStripButtonLoadMap_Click(null, null);
        }

        public MMCM_ControlPanel(CVZ_MMCM linkedMMCM)
        {
            InitializeComponent();
            Initialise(linkedMMCM);
            onStimulationDone += RefreshGUI;
            
        }

        public void Initialise(CVZ_MMCM linkedMMCM)
        {
            if (m_mapThread == null)
            {
                m_mapThread = new System.Threading.Thread(StimulateMap);
                m_mapThread.Start();
            }

            if (m_mapThread.ThreadState == System.Threading.ThreadState.Running)
            {
                buttonStop_Click(null, null);
                while (m_mapThread.ThreadState != System.Threading.ThreadState.Suspended)
                    System.Threading.Thread.Sleep(10);
            }

            //if (m_map != null)
            //    m_map.Dispose();
            
            m_map = linkedMMCM;
            m_enactionFactor = new Dictionary<string, float>(m_map.modalities.Count + 1);

            flowLayoutPanelModalities.Controls.Clear();
            chartPredictionError.Series.Clear();
            chartElectrodes.Series.Clear();

            foreach (IModality m in m_map.modalities.Values)
            {
                m_enactionFactor.Add(m.name, 0.0f);

                //Create the modalities tab
                if (displayModalities)
                {
                    //flowLayoutPanelModalities.Controls.Add(m.GetControl(!(m is YarpModalityString)));
                    UserControl ctrl;
                    if (!(m is YarpModalityString))
                    {
                        ctrl = new ImageModalityCtrl();
                        (ctrl as ImageModalityCtrl).LinkToModality(m); 
                    }
                    else
                    {
                        ctrl = new IModalityControl();
                        (ctrl as IModalityControl).LinkToModality(m);
                    }
                    flowLayoutPanelModalities.Controls.Add(ctrl);
                }

                //Create the chart tab
                chartPredictionError.Series.Add(m.name);
                chartPredictionError.Series[m.name].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                chartCheckListModalities.Items.Add(m.name, true);

            }

            //Display info about the map (change to some panel related organization)
            GetParameters(null, null);

            //Handle graphical display of maps
            panel1.Controls.Clear();
            m_layers = new List<PictureBox>(m_map.Layers);
            for (int i = 0; i < m_map.Layers; i++)
            {
                PictureBox p = new PictureBox();
                p.Height = 200;
                p.Width = 200;
                p.SizeMode = PictureBoxSizeMode.StretchImage;
                m_layers.Add(p);
                //Hook to the double click event (for stimulation purposes)
                p.DoubleClick += new EventHandler(p_DoubleClick);
                p.Click += new EventHandler(p_Click);
                panel1.Controls.Add(p);
            }

		  checkBoxLogError_CheckedChanged(null, null);
        }

        private void GetParameters(object sender, EventArgs e)
        {

            //0 : learning rate
            //1 : Sigma
            //2 : Period of refreshment (part of the control pannel)

            textBoxLearningRate.Text = m_map.LearningRate.ToString();
            textBoxSigma.Text = m_map.Sigma.ToString();
            textBoxPeriod.Text = m_period.ToString();

            while (gridModalitiesParameters.RowCount < m_map.modalities.Count)
            {
                gridModalitiesParameters.Rows.Add();
            }

            //0 : name
            //1 : enactionFactor
            //2 : influence of the modality
            int row = 0;    
            foreach (IModality m in m_map.modalities.Values)
            {
                gridModalitiesParameters[0, row].Value = m.name;
                gridModalitiesParameters[1, row].Value = m_enactionFactor[m.name];
                gridModalitiesParameters[2, row].Value = m_map.modalitiesInfluence[m.name];
                row++;
            }
            feedbakcTextBoxInfluence.Text = m_map.feedbackInfluence.ToString();

            //Map infos
            labelMapInfo.Text =
                "Map infos:\n" +
                "Width = " + m_map.Width + "\n" +
                "Height = " + m_map.Height + "\n" +
                "Layers = " + m_map.Layers + "\n";

            foreach (IModality m in m_map.modalities.Values)
            {
                labelMapInfo.Text += "Input Modality : " + m.name + "(" + m.Size + ") \n";
            }

        }

        private void SetParameters(object sender, EventArgs e)
        {
            m_map.LearningRate = (float)Convert.ToDouble(textBoxLearningRate.Text);
            m_map.Sigma = (float)Convert.ToDouble(textBoxSigma.Text);
            m_period = Convert.ToInt16(textBoxPeriod.Text);
            m_map.feedbackInfluence = (float)Convert.ToDouble(feedbakcTextBoxInfluence.Text);
           for (int r = 0; r < gridModalitiesParameters.Rows.Count; r++)
			{
                m_enactionFactor[gridModalitiesParameters[0, r].Value.ToString()] = (float)Convert.ToDouble(gridModalitiesParameters[1, r].Value);
                m_map.modalitiesInfluence[gridModalitiesParameters[0, r].Value.ToString()] = (float)Convert.ToDouble(gridModalitiesParameters[2, r].Value);
			}
        }

        private void buttonRun_Click(object sender, EventArgs e)
        {
            if (m_mapThread.ThreadState == System.Threading.ThreadState.Unstarted)
                m_mapThread.Start();
            else
                m_mapThread.Resume();

            buttonRun.Enabled = false;
            buttonRun.Visible = false;
            buttonStop.Enabled = true;
            buttonStop.Visible = true;
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            m_mapThread.Suspend();
            buttonRun.Enabled = true;
            buttonRun.Visible = true;
            buttonStop.Enabled = false;
            buttonStop.Visible = false;
        }

        private event EventHandler onStimulationDone;
        private void RefreshGUI(object sender, EventArgs args)
        {
            if (labelSteps.InvokeRequired)
            {
                this.Invoke(onStimulationDone, new object[] { sender, args });
            }
            else
            {
                labelSteps.Text = "Steps done : " + m_stepsDone.ToString();

                for (int l = 0; l < m_map.Layers; l++)
                {
                    m_layers[l].Image = m_map.GetVisualization(l);
                }

                //Refresh the prediction error chart
                if (predictionErrorCheckBoxLog.Checked)
                    foreach (IModality m in m_map.modalities.Values)
                    {
                        chartPredictionError.Series[m.name].Points.AddY(m.RealErrorMean());
                        //errorLogs[m.name].Add(m.GetPredictionError);
                    }

                //Refresh the electrodes chart
                if (electrodeCheckBoxPlot.Checked)
                    foreach (MMCM_Electrode elec in m_map.electrodes)
                    {
                        chartElectrodes.Series[elec.name].Points.Add(elec.activity);
                    }
            }
        }

        private void StimulateMap()
        {
            while (m_mapThread.ThreadState == System.Threading.ThreadState.Running)
            {
                if (m_map != null)
                {
                    m_map.Step(m_enactionFactor.Values.ToArray());
                    m_stepsDone++;
   
                    if (onStimulationDone != null)
                        onStimulationDone(null, null);
                }
                System.Threading.Thread.Sleep(m_period);
            }
        }

        private void toolStripButtonNewMap_Click(object sender, EventArgs e)
        {
            if (m_map != null)
                m_map.Dispose();

            //todo Use a form to create the new map
            CVZ_MMCM newMap = new CVZ_MMCM("newMap", 10, 10, 3,false);
            newMap.AddModality(new RandomModality("newMap","defaultRandomModality",10),1.0f);
            Initialise(newMap);
        }

        private void toolStripButtonLoadMap_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "MMCM Parameters Files (*.ini)|*.ini | MMCM Weights Files (*.mmcm)|*.mmcm";

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //Dispose the previous map
                if (m_map != null)
                    m_map.Dispose();

                string filePath = openFileDialog1.FileName;
                CVZ_MMCM m = null;
                Stream stream = File.Open(filePath, FileMode.Open);
                BinaryFormatter bformatter = new BinaryFormatter();
                m = (CVZ_MMCM)bformatter.Deserialize(stream);
                stream.Close();
                Initialise(m);
            }
        }

        private void toolStripButtonSaveMap_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "MMCM Weights Files (*.mmcm)|*.mmcm";
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string fileName = saveFileDialog1.FileName;
                Stream stream = File.Open(fileName, FileMode.Create);
                BinaryFormatter bformatter = new BinaryFormatter();
                bformatter.Serialize(stream, m_map);
                stream.Close();
            }
        }

        private void MMCM_ControlPanel_FormClosing(object sender, FormClosingEventArgs e)
        {
            buttonStop_Click(null,null);
            m_map.Dispose();
        }

        /// <summary>
        /// Generate files to plot the map representations for each modality
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonGeneratePlot_Click(object sender, EventArgs e)
        {
            //Make sure we are stopped
            buttonStop_Click(null, null);
            foreach (IModality m in m_map.modalities.Values)
            {
                string fileName = "plot_" + m_map.name + "_" + m.name + ".xyz";
                StreamWriter file = new StreamWriter(fileName);
                for (int x = 0; x < m_map.Width; x++)
                {
                    for (int y = 0; y < m_map.Height; y++)
                    {
                        for (int z = 0; z < m_map.Layers; z++)
                        {
                            string line = "";
                            line += x+"\t"+y+"\t"+z+"\t";

                            for (int mComp = 0; mComp < m.Size; mComp++)
                            {
                                line += m_map.weights[m.name][mComp, x, y, z] + "\t";
                            }
                            file.WriteLine(line);
                        }
                    }
                }
                file.Close();
            }
        }

        private void chartButtonResetTime_Click(object sender, EventArgs e)
        {
            foreach (System.Windows.Forms.DataVisualization.Charting.Series serie in chartPredictionError.Series)
                serie.Points.Clear();
        }

        private void chartCheckListModalities_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            chartPredictionError.Series[chartCheckListModalities.Items[e.Index].ToString()].Enabled = (e.NewValue == CheckState.Checked);
        }

        private void chartButtonSavePlot_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "PNG Files (*.png)|*.png";
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                chartPredictionError.SaveImage(saveFileDialog1.FileName, System.Drawing.Imaging.ImageFormat.Png);
            }
        }

        #region Electrodes

        /// <summary>
        /// Add a new electrode to the electrodes list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void electrodeButtonAdd_Click(object sender, EventArgs e)
        {
            MMCM_Electrode elec = new MMCM_Electrode();
            elec.name = "Electrode_" + m_map.electrodes.Count;
            elec.radius = 1;
            elec.intensity = 0;
            electrodesCheckedListBox.Items.Add(elec);
            m_map.electrodes.Add(elec);
            chartElectrodes.Series.Add(elec.name);
            chartElectrodes.Series[elec.name].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
        }

        /// <summary>
        /// Handle the current electrode selection in the list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void electrodesCheckedListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Display the electrode specific parameters on the gui
            MMCM_Electrode elec = electrodesCheckedListBox.SelectedItem as MMCM_Electrode;
            electrodeTextBoxIntensity.Text = elec.intensity.ToString();
            electrodeTextBoxRadius.Text = elec.radius.ToString();
            currentManipulatedElectrode = elec;
        }

        /// <summary>
        /// Catches a simple click on the map representation. Move the current electrode there.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void p_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = e as MouseEventArgs;

            if (me.Button == System.Windows.Forms.MouseButtons.Right && currentManipulatedElectrode != null )
            {
                //**********************************************
                //Get the coordinates of the stimulated neurons from the click coordinates
                PictureBox pb = sender as PictureBox;

                int xPos = (int)((me.X / (double)pb.Width) * m_map.Width);
                int yPos = (int)((me.Y / (double)pb.Height) * m_map.Height);
                int zPos = m_layers.IndexOf(pb);
                Console.WriteLine("Received manual stimulation on : " + xPos + " / " + yPos + " on layer " + zPos);

                //**********************************************
                //Move the electrode to this location
                currentManipulatedElectrode.x = xPos;
                currentManipulatedElectrode.y = yPos;
                currentManipulatedElectrode.z = zPos;
            }
        }

        /// <summary>
        /// Catches a double click on a map representation, if the map thread is not running it will activate the hierarchical modality of this map and make predictions on lower modalities
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void p_DoubleClick(object sender, EventArgs e)
        {
            if (currentManipulatedElectrode != null)
            {
                //**********************************************
                //Get the coordinates of the stimulated neurons from the click coordinates
                PictureBox pb = sender as PictureBox;
                MouseEventArgs me = e as MouseEventArgs;

                int xPos = (int)((me.X / (double)pb.Width) * m_map.Width);
                int yPos = (int)((me.Y / (double)pb.Height) * m_map.Height);
                int zPos = m_layers.IndexOf(pb);
                Console.WriteLine("Received manual stimulation on : " + xPos + " / " + yPos + " on layer " + zPos);

                //Make sure the hierarchical modality is set
                if (!m_map.HierarchicalModalitySet)
                    Console.WriteLine("Error : the hierarchical modality needs to be set first.");
                else
                {
                    //Single shot (stops the thread)
                    //Make sure the thread is stopped
                    buttonStop_Click(null, null);

                    //Store previous values
                    float previousLearningRate = m_map.LearningRate;
                    float[] previousInfluences = new float[m_map.modalitiesInfluence.Count];
                    float previousFBInfluence = m_map.feedbackInfluence;

                    //Disable learning
                    m_map.LearningRate = 0.0f;

                    //Set the influence of every modality to 0
                    for (int i = 0; i < m_map.modalitiesInfluence.Count; i++)
                    {
                        string key = m_map.modalitiesInfluence.ElementAt(i).Key;
                        previousInfluences[i] = m_map.modalitiesInfluence[key];
                        m_map.modalitiesInfluence[key] = 0.0f;
                    }

                    //Force the feedback influence to 1
                    m_map.feedbackInfluence = 1;
                    //Force the value to the stimulated one
                    m_map.hierarchicalModality.ForceNextRealValue(new float[] { xPos / (float)m_map.Width, yPos / (float)m_map.Height, zPos / (float)m_map.Layers });

                    //Process a step (without enaction)
                    m_map.Step(0.0f);
                    m_stepsDone++;
                    if (onStimulationDone != null)
                        onStimulationDone(null, null);

                    //Set back parameters to their original values
                    m_map.LearningRate = previousLearningRate;
                    for (int i = 0; i < m_map.modalitiesInfluence.Count; i++)
                    {
                        string key = m_map.modalitiesInfluence.ElementAt(i).Key;
                        m_map.modalitiesInfluence[key] = previousInfluences[i];
                    }

                    m_map.feedbackInfluence = previousFBInfluence;
                }
            }
        }
        

        /// <summary>
        /// Reset the logs of the electrodes chart
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void electrodeButtonResetPlot_Click(object sender, EventArgs e)
        {
            foreach (System.Windows.Forms.DataVisualization.Charting.Series serie in chartElectrodes.Series)
                serie.Points.Clear();
        }

        private void electrodesButtonSavePlot_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "PNG Files (*.png)|*.png";
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                chartElectrodes.SaveImage(saveFileDialog1.FileName, System.Drawing.Imaging.ImageFormat.Png);
            }
        }

        private void electrodesCheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            chartElectrodes.Series[(chartCheckListModalities.Items[e.Index] as MMCM_Electrode).name].Enabled = (e.NewValue == CheckState.Checked);
        }

#endregion

	   private void checkBoxLogError_CheckedChanged(object sender, EventArgs e)
	   {
		  if (checkBoxLogError.CheckState == CheckState.Checked)
			 m_map.startLoggingError(m_map.name + "_error.txt");
		  else
			 m_map.stopLoggingError();
	   }







    }
}
