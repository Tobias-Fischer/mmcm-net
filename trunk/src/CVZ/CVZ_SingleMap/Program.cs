using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVZ_Core;


namespace SingleMMCM_NoGui
{
    class Program
    {
        static void Main(string[] args)
        {
            Network.init();
            ResourceFinder rf = new ResourceFinder();

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

            rf.setDefaultContext("MMCM/conf");
            SVector argsVect = new SVector(args);
            rf.configure("MMCM_ROOT", argsVect);

            string mapType = CVZFactory.GetCvzType(rf);
            switch (mapType)
            {
                case "MMCM":
                    {
                        RFModuleMMCM module = new RFModuleMMCM();
                        module.configure(rf);
                        module.runModule();
                        break;
                    }

                case "CTPC":
                    {
                        RFModuleCTPC module = new RFModuleCTPC();
                        module.configure(rf);
                        module.runModule();
                        break;
                    }
                default:
                    throw new Exception("Unknown CVZ type");
            }

        }
    }
}
