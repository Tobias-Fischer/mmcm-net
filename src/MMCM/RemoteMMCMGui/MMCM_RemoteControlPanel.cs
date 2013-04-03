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
    public partial class MMCM_RemoteControlPanel : Form
    {
        Port rpcOut = null;
        List<BufferedPortImageRgb> layersActivity = null;

        List<PictureBox> m_layers;
        int r_w;
        int r_h;
        int r_l;
        int guiInstance;
        Timer updateTimer;

        public MMCM_RemoteControlPanel()
        {
            InitializeComponent();

            updateTimer = new Timer();
            updateTimer.Tick += new EventHandler(timer_Tick); // Everytime timer ticks, timer_Tick will be called
            updateTimer.Interval = 500;             // Timer will tick evert 50 seconds
            updateTimer.Enabled = true;                       // Enable the timer
            updateTimer.Start();  
        }

        public bool Initialise(string remoteMMCMName)
        {
            if (rpcOut == null)
            {
                rpcOut = new Port();

                guiInstance=0;
                string localName = "/" + remoteMMCMName + "/remote_gui_" + guiInstance + ":rpc";
                while (Network.exists(localName))
                {
                    guiInstance++;
                    localName = "/" + remoteMMCMName + "/remote_gui_" + guiInstance + ":rpc";
                }
                rpcOut.open(localName);
            }

            if (rpcOut.getOutputCount()<=0)
            {
                Network.connect(rpcOut.getName().c_str(), "/" + remoteMMCMName + "/rpc");
            }

            if (rpcOut.getOutputCount() <= 0)
            {
                return false;
            }

            //Display info about the map (change to some panel related organization)
            GetParameters(null, null);

            //Visualizations
            if (layersActivity == null)
            {
                layersActivity = new List<BufferedPortImageRgb>();
                for (int i = 0; i < r_l; i++)
                {
                    BufferedPortImageRgb p = new BufferedPortImageRgb();
                    p.open("/" + remoteMMCMName + "/remote_gui_"+guiInstance+"/activity/layer_" + i + ":i");
                    layersActivity.Add(p);
                    while(!Network.connect("/" + remoteMMCMName + "/activity/layer_" + i + ":o", p.getName().c_str()))
                        Time.delay(0.5);
                }
            }

            //Handle graphical display of maps
            panel1.Controls.Clear();
            m_layers = new List<PictureBox>(r_l);
            for (int i = 0; i < r_l; i++)
            {
                PictureBox p = new PictureBox();
                p.Height = 200;
                p.Width = 200;
                p.SizeMode = PictureBoxSizeMode.StretchImage;
                m_layers.Add(p);
                //Hook to the double click event (for stimulation purposes)
                //p.DoubleClick += new EventHandler(p_DoubleClick);
                //p.Click += new EventHandler(p_Click);
                panel1.Controls.Add(p);
            }
            return true;
        }

        private void GetParameters(object sender, EventArgs e)
        {
            Bottle cmd = new Bottle();
            Bottle rep = new Bottle();

            cmd.clear(); rep.clear();
            cmd.addString("get");
            cmd.addString("w");
            rpcOut.write(cmd,rep);
            r_w = rep.get(0).asInt();

            cmd.clear(); rep.clear();
            cmd.addString("get");
            cmd.addString("h");
            rpcOut.write(cmd, rep);
            r_h = rep.get(0).asInt();

            cmd.clear(); rep.clear();
            cmd.addString("get");
            cmd.addString("l");
            rpcOut.write(cmd, rep);
            r_l = rep.get(0).asInt();

            cmd.clear(); rep.clear();
            cmd.addString("get");
            cmd.addString("sigma");
            rpcOut.write(cmd, rep);
            textBoxSigma.Text = rep.get(0).asDouble().ToString();

            cmd.clear(); rep.clear();
            cmd.addString("get");
            cmd.addString("lrate");
            rpcOut.write(cmd, rep);
            textBoxLearningRate.Text = rep.get(0).asDouble().ToString();

            cmd.clear(); rep.clear();
            cmd.addString("get");
            cmd.addString("period");
            rpcOut.write(cmd, rep);
            textBoxPeriod.Text = rep.get(0).asInt().ToString();

            feedbakcTextBoxInfluence.Text = "???";

            //Map infos
            labelMapInfo.Text =
                "Map infos: " +
                "Width = " + r_w + " " +
                "Height = " + r_h + " " +
                "Layers = " + r_l + " ";
        }

        private void SetParameters(object sender, EventArgs e)
        {
            Bottle cmd = new Bottle();
            Bottle rep = new Bottle();

            cmd.clear(); rep.clear();
            cmd.addString("set");
            cmd.addString("sigma");
            cmd.addDouble(Convert.ToDouble(textBoxSigma.Text));
            rpcOut.write(cmd, rep);

            cmd.clear(); rep.clear();
            cmd.addString("set");
            cmd.addString("lrate");
            cmd.addDouble(Convert.ToDouble(textBoxLearningRate.Text));
            rpcOut.write(cmd, rep);

            cmd.clear(); rep.clear();
            cmd.addString("set");
            cmd.addString("period");
            cmd.addDouble(Convert.ToDouble(textBoxPeriod.Text));
            rpcOut.write(cmd, rep);

            //MessageBox.Show("Ok");
        }

        private void buttonRun_Click(object sender, EventArgs e)
        {
            Bottle cmd = new Bottle();
            Bottle rep = new Bottle();

            cmd.clear(); rep.clear();
            cmd.addString("run");
            rpcOut.write(cmd, rep);

            buttonRun.Enabled = false;
            buttonRun.Visible = false;
            buttonStop.Enabled = true;
            buttonStop.Visible = true;
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            Bottle cmd = new Bottle();
            Bottle rep = new Bottle();

            cmd.clear(); rep.clear();
            cmd.addString("pause");
            rpcOut.write(cmd, rep);

            buttonRun.Enabled = true;
            buttonRun.Visible = true;
            buttonStop.Enabled = false;
            buttonStop.Visible = false;
        }

        void timer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < r_l; i++)
            {
                ImageRgb img = layersActivity[i].read(false);
                if (img != null)
                {
                    m_layers[i].Image = HelpersLib.ImageManipulation.toBmp(img);
                }
            }
        }

        private void buttonGetParameters_Click(object sender, EventArgs e)
        {
            GetParameters(sender, e);
        }

        private void buttonSetParameters_Click(object sender, EventArgs e)
        {
            SetParameters(sender, e);
        }
    }
}
