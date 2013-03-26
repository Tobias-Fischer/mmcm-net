using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MMCMLibrary;
using MMCMLibrary.Modalities;

namespace SingleMMCM_NoGui
{
    public class RFModuleMMCM : RFModuleCVZ
    {
        List<BufferedPortImageRgb> layersActivity = new List<BufferedPortImageRgb>();

        public override bool configure(ResourceFinder rf)
        {
            if (base.configure(rf))
            {
                //Open ports for visualizing the activity
                CVZ_MMCM mmcm = cvz as CVZ_MMCM;
                for (int i = 0; i < mmcm.Layers; i++)
                {
                    BufferedPortImageRgb p = new BufferedPortImageRgb();
                    p.open("/"+mmcm.name+"/activity/layer_"+i+":o");
                    layersActivity.Add(p);
                }
                return true;
            }
            else
                return false ;
        }

        public override bool interruptModule()
        {
            base.interruptModule();
            foreach (BufferedPortImageRgb p in layersActivity)
                p.interrupt();
            return true;
        }

        public override bool close()
        {
            base.close();
            foreach (BufferedPortImageRgb p in layersActivity)
                p.close();
            return true;
        }

        protected override void set(Bottle command, Bottle reply)
        {
            string key = command.get(0).asString().c_str();
            switch (key)
            {
                case "lrate": (cvz as CVZ_MMCM).LearningRate = (float)command.get(1).asDouble(); reply.addString("ack"); break;
                case "sigma": (cvz as CVZ_MMCM).Sigma = (float)command.get(1).asDouble(); reply.addString("ack"); break;
                default: base.set(command, reply); break;
            }
        }

        protected override void get(Bottle command, Bottle reply)
        {
            string key = command.get(0).asString().c_str();
            switch (key)
            {
                case "w": reply.addInt((cvz as CVZ_MMCM).Width); break;
                case "h": reply.addInt((cvz as CVZ_MMCM).Height); break;
                case "l": reply.addInt((cvz as CVZ_MMCM).Layers); break;
                case "lrate": reply.addDouble((cvz as CVZ_MMCM).LearningRate); break;
                case "sigma": reply.addDouble((cvz as CVZ_MMCM).Sigma); break;
                default: base.get(command, reply); break;
            }
        }

        public override bool updateModule()
        {
            bool error = base.updateModule();

            //Broadcast the activity
            CVZ_MMCM mmcm = cvz as CVZ_MMCM;
            for (int i = 0; i < mmcm.Layers; i++)
            {
                ImageRgb img = layersActivity[i].prepare();
                mmcm.GetVisualization(i, ref img);
                layersActivity[i].write();
            }

            return error;
        }
    }
}
