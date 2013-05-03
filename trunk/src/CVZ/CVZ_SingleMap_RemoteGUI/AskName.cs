using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CVZ_Core.GUI;

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
                MMCM_RemoteControlPanel remotePanel = new MMCM_RemoteControlPanel();
                while (!remotePanel.Initialise(txtName.Text)) ;
                remotePanel.Show();
            }
            else if (radioButtonCTPC.Checked)
            {
                CTPC_RemoteControlPanel remotePanel = new CTPC_RemoteControlPanel();
                while (!remotePanel.Initialise(txtName.Text)) ;
                remotePanel.Show();
            }
        }
    }
}
