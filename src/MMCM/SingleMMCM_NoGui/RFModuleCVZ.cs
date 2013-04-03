using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MMCMLibrary;
using MMCMLibrary.Modalities;

namespace SingleMMCM_NoGui
{
    public class RFModuleCVZ: RFModule
    {
        protected IConvergenceZone cvz = null;

        private Port rpc = new Port();
        private int period; //ms
        private bool isRunning = false;

        public override bool configure(ResourceFinder rf)
        {
            Console.WriteLine("----------------------------------");
            Console.WriteLine("Convergence Divergence Zone Module");
            Console.WriteLine("----------------------------------");

            bool loadingFromWeights = rf.check("load");
            period = rf.check("period", new Value(100)).asInt();
            Console.Write("Running every " + period + " ms");

            if (loadingFromWeights)
            {
                Console.Write("Loading MMCM from weights file...");
                string weightFile = rf.find("load").asString().c_str();
                string path = rf.findFile(weightFile).c_str();
                Console.WriteLine(path);
                cvz = CVZFactory.Create(path);
            }
            else
            {
                cvz = CVZFactory.Create(rf);
            }

            //Open the RPC communication
            rpc.open("/" + cvz.name + "/rpc");
            attach(rpc);

            return true;
        }

        public override bool interruptModule()
        { 
            rpc.interrupt();
            return base.interruptModule();
        }

        public override bool close()
        {
            rpc.close();
            cvz.Dispose();
            return base.close();
        }

        public override bool respond(Bottle command, Bottle reply)
        {
            string key = command.get(0).asString().c_str();
            switch (key)
            {
                case "set": set(command.tail(), reply); break;
                case "get": get(command.tail(), reply); break;
                case "run": isRunning = true; reply.addString("started"); break;
                case "pause": isRunning = false; reply.addString("paused"); break;
                case "quit": return false;

                default: reply.addString("unknown command"); break;
            }
            return true;
        }

        protected virtual void set(Bottle command, Bottle reply)
        {
            string key = command.get(0).asString().c_str();
            switch (key)
            {
                case "period": period = command.get(1).asInt(); reply.addString("period set"); break;

                default: reply.addString("unknown parameter"); break;
            }
        }

        protected virtual void get(Bottle command, Bottle reply)
        {
            string key = command.get(0).asString().c_str();
            switch (key)
            {
                case "period": reply.addInt(period); break;
                default: reply.addString("unknown parameter"); break;
            }
        }

        public override bool updateModule()
        { 
            double t1 = Time.now();
            if (isRunning)
            {
                cvz.Step();
            }
            double t2 = Time.now();

            double timeToSleep = period - (t2 - t1);
            if (timeToSleep <= 0)
            {
                Console.WriteLine("Impossible to match desired period. Execution time is: " + (t2 - t1).ToString());
            }
            else
            {
                Time.delay(timeToSleep / 1000.0);
            }
            return true;
        }
    }
}
