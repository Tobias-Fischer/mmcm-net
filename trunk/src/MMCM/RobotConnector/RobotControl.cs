using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotConnector
{
    class RobotControl
    {
        private Dictionary<string, IPositionControl> posCtrl;
        private Dictionary<string, IVelocityControl> velCtrl;
        private Dictionary<string, IEncoders> encs;
        private Dictionary<string, PolyDriver> polydrivers;
        private string robotName;
        private string localName;

        private RobotControl(string _robotName, string _localName)
        {
            robotName = "/" + _robotName;
            localName = "/" + _localName;
            posCtrl = new Dictionary<string, IPositionControl>();
            velCtrl = new Dictionary<string, IVelocityControl>();
            encs = new Dictionary<string, IEncoders>();
            polydrivers = new Dictionary<string, PolyDriver>();
            JointRange.Initialise();
        }

        private void AddPart(string partName)
        {
            Property options = new Property();
            options.put("device", "remote_controlboard");
		  options.put("robot", robotName);
            options.put("part", partName);
            options.put("remote", robotName + "/" + partName);
            options.put("local", localName + robotName + "/" + partName);

            PolyDriver pol = new PolyDriver(options);
            polydrivers.Add("/" + partName, pol);
            posCtrl.Add("/" + partName, pol.viewIPositionControl());
            velCtrl.Add("/" + partName, pol.viewIVelocityControl());
            encs.Add("/" + partName, pol.viewIEncoders());

        }

        public static RobotControl GetAnICub(string _robotName, string _localName)
        {
            RobotControl robot = new RobotControl(_robotName, _localName);
            robot.AddPart("head");
            robot.AddPart("right_arm");
            robot.AddPart("left_arm");
            robot.AddPart("right_leg");
            robot.AddPart("left_leg");
            robot.AddPart("torso");
            return robot;
        }

        public static RobotControl GetANao(string _robotName, string _localName)
        {
            throw new NotImplementedException();
        }

	   public bool GetPos(string part, ref double[] values)
	   {

		  int jntNumber = encs[part].getAxes();
		  values = new double[jntNumber];
		  for (int j = 0; j < jntNumber; j++)
		  {
			 values[j] = encs[part].getEncoder(j);
		  }

		  return true;
	   }

	   public bool SetPos(string part, double[] values)
	   {
		  DVector vals = new DVector(values);
		  return posCtrl[part].positionMove(vals);
	   }

	   public bool SetPos(string part, int jnt, double val)
	   {
		  return posCtrl[part].positionMove(jnt, val);
	   }

        public bool GetScaledPos(string part, ref double[] values)
        {

		  int jntNumber = encs[part].getAxes();
		  values = new double[jntNumber];
            for (int j = 0; j < jntNumber; j++)
            {
                values[j] = encs[part].getEncoder(j);
            }

            double[,] range;

            switch (robotName)
            {
                case "/icub":
                    {
                        range = JointRange.ICUB[part];
                        break;
                    }
                default:
                    {
                        return false;
                    }
            }

            for (int i = 0; i < jntNumber; i++)
            {
                values[i] = Math.Max(0,Math.Min(1, (values[i] - range[1, i]) / (range[0, i] - range[1, i])));
            }

            return true;
        }

        public bool SetScaledPos(string part, double[] values)
        {
            double[,] range;

            switch (robotName)
            {
                case "icub":
                    {
                        range = JointRange.ICUB[part];
                        break;
                    }
                default:
                    {
                        return false;
                    }
            }

            double[] unscaled = new double[values.Length];
            values.CopyTo(unscaled,0);
            for (int i = 0; i < values.GetLength(0); i++)
            {
                unscaled[i] = unscaled[i] * (range[0, i] - range[1, i]) + range[0, i];
            }

            DVector vals = new DVector(unscaled);
            posCtrl[part].positionMove(vals);

            return true;
        }

        public void close()
        {
            foreach (KeyValuePair<string, PolyDriver> d in polydrivers)
            {
                d.Value.close();
            }
        }
    }
}
