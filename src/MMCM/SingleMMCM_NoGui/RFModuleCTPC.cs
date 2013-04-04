using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MMCMLibrary;
using MMCMLibrary.Modalities;
using MMCMLibrary.CVZ.CTPC;

namespace SingleMMCM_NoGui
{
    public class RFModuleCTPC : RFModuleCVZ
    {
        BufferedPortImageRgb activity = new BufferedPortImageRgb();
        private bool broadcastDisplay;

        public override bool configure(ResourceFinder rf)
        {
            if (base.configure(rf))
            {
                broadcastDisplay = !rf.check("noDisplay");

                //Open ports for visualizing the activity
                CVZ_TPC ctpc = cvz as CVZ_TPC;
                 activity = new BufferedPortImageRgb();
                 activity.open("/" + ctpc.name + "/activity:o");
                return true;
            }
            else
                return false ;
        }

        public override bool interruptModule()
        {
            base.interruptModule();
            activity.interrupt();
            return true;
        }

        public override bool close()
        {
            base.close();
            activity.close();
            return true;
        }

        protected override void set(Bottle command, Bottle reply)
        {
            string key = command.get(0).asString().c_str();
            switch (key)
            {
                default: base.set(command, reply); break;
            }
        }

        protected override void get(Bottle command, Bottle reply)
        {
            string key = command.get(0).asString().c_str();
            switch (key)
            {
                default: base.get(command, reply); break;
            }
        }

        public override bool updateModule()
        {
            bool error = base.updateModule();
            //Broadcast the activity
            CVZ_TPC ctpc = cvz as CVZ_TPC;
            if (broadcastDisplay)
            {
                ImageRgb img = activity.prepare();
                ctpc.GetVisualization(ref img);
                activity.write();
            }
            return error;
        }
    }
}
