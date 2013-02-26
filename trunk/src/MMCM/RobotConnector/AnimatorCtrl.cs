using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RobotConnector
{
    public partial class AnimatorCtrl : UserControl
    {
	   Timer timer = new Timer();
	   bool isRecording = false;
	   bool isPlaying = false;
	   int playedFrames = 0;
	   KeyValuePair<string, PartAnimation> currentRecord = new KeyValuePair<string, PartAnimation>();
	   PartAnimation currentReplay = new PartAnimation();
	   Dictionary<string, PartAnimation> animations = new Dictionary<string,PartAnimation>();
	   static RobotControl icub;
	   public string part;

	   public AnimatorCtrl(string part = "head")
	   {
		  InitializeComponent();
		  if (icub == null)
			 icub = RobotControl.GetAnICub("icubSim", "animator");
		  this.part = part;
		  this.groupBox1.Text = part;

            timer.Tick += new EventHandler(timer_Tick); // Everytime timer ticks, timer_Tick will be called
            timer.Interval = 50;             // Timer will tick evert 50 seconds
            timer.Enabled = true;                       // Enable the timer
            timer.Start();    
	   }
           
        void timer_Tick(object sender, EventArgs e)
        {
		  if (isRecording)
		  {
			 currentRecord.Value.framestack.Add(getFrame());
		  }
		  if (isPlaying)
		  {
			 setFrame(currentRecord.Value.framestack[playedFrames]);
			 playedFrames++;
			 if (playedFrames >= currentRecord.Value.framestack.Count)
			 {
				buttonPlay.Enabled = true;
				isPlaying = false;
			 }
		  }
        }



	   private void buttonRecord_Click(object sender, EventArgs e)
	   {
		  currentRecord = new KeyValuePair<string, PartAnimation>(textBoxName.Text, new PartAnimation());
		  buttonRecord.Enabled = false;
		  buttonStop.Enabled = true;
		  isRecording = true;
	   }


	   private void buttonStop_Click(object sender, EventArgs e)
	   {
		  animations.Add(currentRecord.Key,currentRecord.Value);
		  listBoxAnimations.Items.Add(currentRecord.Key);
		  buttonRecord.Enabled = true;
		  buttonStop.Enabled = false;
		  isRecording = false;
	   }

	   void setFrame(PartAnimation.Frame f)
	   {
		  double[] valuesTotal = new double[0]; ;
		  double[] valuesFrame = new double[0];

		  if (part == "left_hand")
		  {
			 for (int i = 9; i < 16; i++)
			 {
				icub.SetPos("/left_arm",i, f.posture[i-9]);
			 }
		  }

		  else if (part == "right_hand")
		  {
			 for (int i = 9; i < 16; i++)
			 {
				icub.SetPos("/right_arm", i, f.posture[i - 9]);
			 }
		  }

		  else if (part == "left_arm")
		  {
			 for (int i = 0; i < 9; i++)
			 {
				icub.SetPos("/left_arm", i, f.posture[i]);
			 }
		  }

		  else if (part == "right_arm")
		  {
			 for (int i = 0; i < 9; i++)
			 {
				icub.SetPos("/right_arm", i, f.posture[i]);
			 }
		  }
		  else
			 icub.SetPos("/" + part, valuesFrame);
	   }

	   PartAnimation.Frame getFrame()
	   {
		  PartAnimation.Frame f = new PartAnimation.Frame();
		  double[] valuesTotal = new double[0]; ;
		  double[] valuesFrame = new double[0];

		  if (part == "left_hand" )
		  {
			 valuesFrame = new double[7];
			 icub.GetPos("/left_arm",ref valuesTotal);
			 for(int i = 9; i<16;i++)
			 {
				valuesFrame[i-9] = valuesTotal[i];
			 }
		  }

		  else if (part == "right_hand")
		  {
			 valuesFrame = new double[7];
			 icub.GetPos("/right_arm",ref valuesTotal);
			 for(int i = 9; i<16;i++)
			 {
				valuesFrame[i-9] = valuesTotal[i];
			 }
		  }

		  else if (part == "left_arm" )
		  {
			 valuesFrame = new double[9];
			 icub.GetPos("/left_arm",ref valuesTotal);
			 for(int i = 0; i<9;i++)
			 {
				valuesFrame[i] = valuesTotal[i];
			 }
		  }

		  else if (part == "right_arm")
		  {
			 valuesFrame = new double[9];
			 icub.GetPos("/right_arm",ref valuesTotal);
			 for(int i = 0; i<9;i++)
			 {
				valuesFrame[i] = valuesTotal[i];
			 }
		  }
		  else
			 icub.GetPos("/"+part,ref valuesFrame);


		  f.timeStamp = Time.now();
		  f.posture = valuesFrame;
		  return f;
	   }

	   private void buttonPlay_Click(object sender, EventArgs e)
	   {
		  currentReplay = animations[listBoxAnimations.SelectedItem.ToString()];
		  playedFrames = 0;
		  isPlaying = true;
		  buttonPlay.Enabled = false;
	   }

	   public bool playAnim(string anim)
	   {
		  if (!animations.ContainsKey(anim))
			 return false;
		  currentReplay = animations[anim];
		  playedFrames = 0;
		  isPlaying = true;

		  return true;
	   }

    }
}
