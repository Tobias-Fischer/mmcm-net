using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotConnector
{
    public class PartAnimation
    {
	   public struct Frame
	   {
		  public double timeStamp;
		  public double[] posture;
	   }

	   public List<Frame> framestack = new List<Frame>();
    }
}
