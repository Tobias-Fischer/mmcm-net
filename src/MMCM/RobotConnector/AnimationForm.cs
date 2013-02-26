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
    public partial class AnimationForm : Form
    {
	   BufferedPortBottle rpc;
	   System.Threading.Thread rpcThread;
	   Dictionary<string, AnimatorCtrl> ctrls = new Dictionary<string,AnimatorCtrl>();

	   public AnimationForm(	  )
	   {
		  Network.init();
		  InitializeComponent();
		  ctrls.Add("left_hand", new AnimatorCtrl("left_hand"));
		  ctrls.Add("left_arm", new AnimatorCtrl("left_arm"));

		  foreach(AnimatorCtrl c in ctrls.Values)
			 flowLayoutPanel1.Controls.Add(c);

		  rpc = new BufferedPortBottle();
		  rpc.open("/animator/rpc");
		  rpcThread = new System.Threading.Thread(handle);
		  rpcThread.Start();

	   }

	   void handle()
	   {
		  while (rpcThread.IsAlive)
		  {
			 Bottle cmd = rpc.read(false);
			 if (cmd != null)
			 {
				if (cmd.get(0).asString().c_str() == "play")
				{
				    string part = cmd.get(1).asString().c_str();
				    string anim = cmd.get(2).asString().c_str();
				    ctrls[part].playAnim(anim);
				}
			 }
			 Time.delay(0.1);
		  }
		  rpc.close();
	   }

	   private void AnimationForm_FormClosing(object sender, FormClosingEventArgs e)
	   {
		  rpcThread.Abort();
	   }
    }
}
