﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CVZ_Core;
using CVZ_Core.Modalities;

namespace SingleMMCM_NoGui
{
    public class RFModuleCVZ: RFModule
    {
        protected IConvergenceZone cvz = null;
        private Port rpc = new Port();
        private int period; //ms
        private float enactionFactor = 0.0f;
        private double lastUpdateTime = 0.0;
        private bool isRunning = false;
        private bool broadcastDisplay;

        public override bool configure(ResourceFinder rf)
        {
            Time.turboBoost();
            Console.WriteLine("----------------------------------");
            Console.WriteLine("Convergence Divergence Zone Module");
            Console.WriteLine("----------------------------------");

            bool loadingFromWeights = rf.check("load");
            period = rf.check("period", new Value(100)).asInt();
            broadcastDisplay = !rf.check("noDisplay");
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
                case "fbInf": cvz.feedbackInfluence = command.get(1).asInt(); reply.addString("feedback Inf. set"); break;

                default: reply.addString("unknown parameter"); break;
            }
        }

        protected virtual void get(Bottle command, Bottle reply)
        {
            string key = command.get(0).asString().c_str();
            switch (key)
            {
                case "period": reply.addInt(period); break;
                case "fbInf": reply.addDouble(cvz.feedbackInfluence); break;

                default: reply.addString("unknown parameter"); break;
            }
        }

        public override double getPeriod()
        {
            return period / 1000.0;
        }

        public override bool updateModule()
        {
            double currentTime = Time.now(); 
            double executionTime = Math.Round(currentTime - lastUpdateTime);
            lastUpdateTime = currentTime;
            if ( executionTime > period / 1000.0)
            {
                Console.WriteLine("Impossible to match desired period (" + period / 1000.0 +"). Execution time is: " + executionTime);
            }

            if (isRunning)
            {
                cvz.Step(enactionFactor);
            }

            return true;
        }
    }
}
