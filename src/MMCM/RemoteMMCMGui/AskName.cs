using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RemoteMMCMGui
{
    public partial class AskName : Form
    {

        public AskName()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButtonMMCM.Checked)
            {
                MMCMLibrary.MMCM_RemoteControlPanel remotePanel = new MMCMLibrary.MMCM_RemoteControlPanel();
                while (!remotePanel.Initialise(txtName.Text)) ;
                remotePanel.Show();
            }
            else if (radioButtonCTPC.Checked)
            {
                MMCMLibrary.CTPC_RemoteControlPanel remotePanel = new MMCMLibrary.CTPC_RemoteControlPanel();
                while (!remotePanel.Initialise(txtName.Text)) ;
                remotePanel.Show();
            }
        }
    }
}
