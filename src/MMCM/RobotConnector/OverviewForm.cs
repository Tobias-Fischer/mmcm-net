using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RobotConnector
{
    public partial class OverviewForm : Form
    {
        RobotControl robot;
        double[] leftArm = new double[16];
        double[] rightArm = new double[16];
        double[] rightLeg = new double[6];
        double[] leftLeg = new double[6];
        double[] torso = new double[3];
        double[] head = new double[6];
        System.Threading.Thread mThread;

        BufferedPortBottle portOutHeadScaled;
        //BufferedPortBottle portOutTorsoScaled;
        BufferedPortBottle portOutLeftArmScaled;
        BufferedPortBottle portOutRightArmScaled;
        //BufferedPortBottle portOutRightLegScaled;
        //BufferedPortBottle portOutLeftLegScaled;

        public OverviewForm()
        {
            InitializeComponent();
            dataGridViewMotors.Rows.Add(16);
            Network.init();

            robot = RobotControl.GetAnICub("icub", "MMCM/RobotConnector");

            //Open the ports that will stream the scaled encoders
            portOutHeadScaled = new BufferedPortBottle();
            portOutHeadScaled.open("/MMCM/RobotConnector/head/scaled:o");
            portOutLeftArmScaled = new BufferedPortBottle();
            portOutLeftArmScaled.open("/MMCM/RobotConnector/left_arm/scaled:o");
            portOutRightArmScaled = new BufferedPortBottle();
            portOutRightArmScaled.open("/MMCM/RobotConnector/right_arm/scaled:o");
            //portOutRightLegScaled = new BufferedPortBottle();
            //portOutRightLegScaled.open("/MMCM/RobotConnector/right_leg/scaled:o");
            //portOutLeftLegScaled = new BufferedPortBottle();
            //portOutLeftLegScaled.open("/MMCM/RobotConnector/left_leg/scaled:o");
            //portOutTorsoScaled = new BufferedPortBottle();
            //portOutTorsoScaled.open("/MMCM/RobotConnector/torso/scaled:o");

            mThread = new System.Threading.Thread(mainLoop);
            mThread.Start();
            onUpdate += RefreshGUI;
            this.FormClosing += CleanUp;
        }

        private void mainLoop()
        {
            while (mThread.IsAlive)
            {
                robot.GetScaledPos("/left_arm",ref leftArm);
                robot.GetScaledPos("/right_arm",ref rightArm);
                robot.GetScaledPos("/head",ref head);
                //robot.GetScaledPos("/torso", ref torso);
                //robot.GetScaledPos("/left_leg", ref leftLeg);
                //robot.GetScaledPos("/right_leg", ref rightLeg);

                SendScaledOutput();
                RefreshGUI(null, null);
            }  
        }

        private event EventHandler onUpdate;
        private void RefreshGUI(object sender, EventArgs args)
        {
            if (dataGridViewMotors.InvokeRequired)
            {
                this.Invoke(onUpdate, new object[] { sender, args });
            }
            else
            {
                for (int i = 0; i < 6; i++)
                {
                    dataGridViewMotors["ColumnHead", i].Value = head[i].ToString();
                }

                //for (int i = 0; i < 6; i++)
                //{
                //    dataGridViewMotors["ColumnLeftLeg", i].Value = leftLeg[i].ToString();
                //    dataGridViewMotors["ColumnRightLeg", i].Value = rightLeg[i].ToString();
                //}

                //for (int i = 0; i < 3; i++)
                //{
                //    dataGridViewMotors["ColumnTorso", i].Value = torso[i].ToString();
                //}

                for (int i = 0; i < 16; i++)
                {
                    dataGridViewMotors["ColumnLeftArm", i].Value = leftArm[i].ToString();
                    dataGridViewMotors["ColumnRightArm", i].Value = rightArm[i].ToString();
                }

            }
        }

        private void SendScaledOutput()
        {
            Bottle b = portOutHeadScaled.prepare();
            b.clear();
            for (int i = 0; i < 6; i++)
                b.addDouble(head[i]);
            portOutHeadScaled.write();

            b = portOutLeftArmScaled.prepare();
            b.clear();
            for (int i = 0; i < 16; i++)
                b.addDouble(leftArm[i]);
            portOutLeftArmScaled.write();

            b = portOutRightArmScaled.prepare();
            b.clear();
            for (int i = 0; i < 16; i++)
                b.addDouble(rightArm[i]);
            portOutRightArmScaled.write();

            //b = portOutRightLegScaled.prepare();
            //b.clear();
            //for (int i = 0; i < 6; i++)
            //    b.addDouble(rightLeg[i]);
            //portOutRightLegScaled.write();

            //b = portOutLeftLegScaled.prepare();
            //b.clear();
            //for (int i = 0; i < 6; i++)
            //    b.addDouble(leftLeg[i]);
            //portOutLeftLegScaled.write();

            //b = portOutTorsoScaled.prepare();
            //b.clear();
            //for (int i = 0; i < 3; i++)
            //    b.addDouble(torso[i]);
            //portOutTorsoScaled.write();
        }

        private void CleanUp(object sender, EventArgs e)
        {
            mThread.Abort();
            portOutHeadScaled.close();
            portOutLeftArmScaled.close();
            portOutRightArmScaled.close();
            //portOutRightLegScaled.close();
            //portOutLeftLegScaled.close();
            //portOutTorsoScaled.close();
            robot.close();
        }
    }
}
