using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotConnector
{
    class JointRange
    {

       public static Dictionary<string, double[,]> ICUB = new Dictionary<string, double[,]>();

        public static void Initialise()
        {
            double[,] left_arm = { { 90, 160.8, 100, 106, 90, 10, 40, 30, 105, 90, 90, 90, 90, 90, 90, 115 }, { -95.5, 0, -37, 5.5, -90, -90, -20, -20, -15, 0, 0, 0, 0, 0, 0, 0 } };
            ICUB.Add("/left_arm",left_arm);
            double[,] right_arm = { { 90, 160.8, 100, 106, 90, 10, 40, 30, 105, 90, 90, 90, 90, 90, 90, 115 }, { -95.5, 0, -37, 5.5, -90, -90, -20, -20, -15, 0, 0, 0, 0, 0, 0, 0 } };
            ICUB.Add("/right_arm",right_arm);            
            double[,] head = { { 30, 60, 55, 15, 52, 90 }, { -40, -70, -55, -35, -50, 0 } };
            ICUB.Add("/head",head);
        }
    }
}
