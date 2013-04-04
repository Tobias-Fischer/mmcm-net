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
    public partial class CTPC_RemoteControlPanel : Form
    {
        Port rpcOut = null;
        BufferedPortImageRgb activity = null;

        PictureBox m_activity;
        int r_w;
        int r_h;
        int guiInstance;
        Timer updateTimer;

        public CTPC_RemoteControlPanel()
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
            if (activity == null)
            {
                activity = new BufferedPortImageRgb();
                activity.open("/" + remoteMMCMName + "/remote_gui_"+guiInstance+"/activity:i");
                while(!Network.connect("/" + remoteMMCMName + "/activity:o", activity.getName().c_str()))
                        Time.delay(0.5);
            }

            //Handle graphical display of maps
            panel1.Controls.Clear();
            m_activity = new PictureBox();
            m_activity.Height = 200;
            m_activity.Width = 200;
            m_activity.SizeMode = PictureBoxSizeMode.StretchImage;

            //Hook to the double click event (for stimulation purposes)
            //p.DoubleClick += new EventHandler(p_DoubleClick);
            //p.Click += new EventHandler(p_Click);
            panel1.Controls.Add(m_activity);
            return true;
        }

        private void GetParameters(object sender, EventArgs e)
        {
  
        }

        private void SetParameters(object sender, EventArgs e)
        {
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
            ImageRgb img = activity.read(false);
            if (img != null)
            {
                m_activity.Image = HelpersLib.ImageManipulation.toBmp(img);
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
