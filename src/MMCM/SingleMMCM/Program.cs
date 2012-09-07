using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

using MMCMLibrary;
using MMCMLibrary.Modalities;

namespace ConsoleLauncher
{
    class Program
    {

        [STAThread]
        static void Main(string[] args)
        {
            Network.init();
            Console.WriteLine("----------------------------------");
            Console.WriteLine("Convergence Divergence Zone Module");
            Console.WriteLine("----------------------------------");

            ResourceFinder rf = new ResourceFinder();
            rf.setDefaultContext("MMCM/conf");

            //Workaround the "--from" issue
            bool loadingFromWeights = false;
            string configFile = "defaultMap.ini";
            for (int i = 0; i < args.Count(); i++)
            {
                if (args[i] == "--from")
                    configFile = args[i + 1];
                if (args[i] == "--load")
                {
                    loadingFromWeights = true;
                    configFile = args[i + 1];
                }
            }

            if (!loadingFromWeights)
                rf.setDefaultConfigFile(configFile);

            SVector argsVect = new SVector(args);
            rf.configure("EFAA_ROOT", argsVect);

            IConvergenceZone cvz = null;
            if (loadingFromWeights)
            {
                Console.Write("Loading MMCM from weights file...");
                string path = rf.findFile(configFile).c_str();
                Console.WriteLine(path);
                cvz = CVZFactory.Create(path);
            }
            else
            {
                cvz = CVZFactory.Create(rf);
            }

            Application.EnableVisualStyles();
            Application.Run((cvz as CVZ_MMCM).GetControlPanel());
        }
    }
}
