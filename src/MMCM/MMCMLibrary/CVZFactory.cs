using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MMCMLibrary.Modalities;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace MMCMLibrary
{
    public class CVZFactory
    {
        public static IConvergenceZone Create(string path)
        {
            IConvergenceZone m = null;
            Stream stream = File.Open(path, FileMode.Open);
            BinaryFormatter bformatter = new BinaryFormatter();
            m = (IConvergenceZone)bformatter.Deserialize(stream);
            stream.Close();
            return m;
        }

        public static IConvergenceZone Create(ResourceFinder rf)
        {
            IConvergenceZone cvz = null;
            string mapType = rf.check("mapType", new Value("MMCM")).asString().c_str();
            string mapName = rf.check("mapName", new Value("defaultMap")).asString().c_str();
            int w = rf.check("width", new Value(10)).asInt();
            int h = rf.check("height", new Value(10)).asInt();
            int l = rf.check("layers", new Value(4)).asInt();
            Console.WriteLine("Map " + mapName + " (" + mapType + ")");
            Console.WriteLine("Shape " + l + "x(" + w + "x" + h + ")");

            switch (mapType)
            {
                case "MMCM":
                    {
                        cvz = new CVZ_MMCM(mapName, h, w, l);
                        float learningRate = (float)rf.check("learningRate", new Value(0.07f)).asDouble();
                        float sigma = (float)rf.check("sigma", new Value((float)((1 / 4.0) * (w + h) / 2.0))).asDouble();
                        Console.WriteLine("Learning Rate: " + learningRate);
                        Console.WriteLine("Sigma: " + sigma);
                        break;
                    }

                case "MMCM_GPU":
                    {
                        cvz = new CVZ_MMCM_GPU(mapName, h, w, l);
                        float learningRate = (float)rf.check("learningRate", new Value(0.07f)).asDouble();
                        float sigma = (float)rf.check("sigma", new Value((float)((1 / 4.0) * (w + h) / 2.0))).asDouble();
                        Console.WriteLine("Learning Rate: " + learningRate);
                        Console.WriteLine("Sigma: " + sigma);
                        break;
                    }
            }

            //Unknown map type
            if (cvz == null)
            {
                Console.WriteLine("Unknown map type: " + mapType);
                return null;
            }
            else
            {
                Console.WriteLine("Map created. Adding modalities...");
            }


            int modalityCount = rf.check("modalityCount", new Value(0)).asInt();
            for (int i = 0; i < modalityCount; i++)
            {
                IModality mod = null;
                Bottle modalityGroup = rf.findGroup("modality_" + i);
                string name = modalityGroup.check("name", new Value("defaultModality" + i)).asString().c_str();
                string type = modalityGroup.check("type", new Value("random")).asString().c_str();
                float influence = (float)modalityGroup.check("influence", new Value(1.0)).asDouble();
                Console.WriteLine("Modality: " + name + " (" + type + ")");
			 Console.WriteLine("Influence: " + influence);
                switch (type)
                {
                    case "random":
                        {
                            int size = modalityGroup.check("size", new Value(1)).asInt();
                            mod = new RandomModality(mapName, name, size);
                            break;
                        }

                    case "yarpVector":
                        {
                            int size = modalityGroup.check("size", new Value(1)).asInt();
                            float[] minBounds = null;
                            float[] maxBounds = null;
					   bool isBlocking = modalityGroup.check("isBlocking");
					   Console.WriteLine("isBlocking: " + isBlocking);
                            if (modalityGroup.check("minBounds"))
                            {
                                minBounds = new float[size];
                                Bottle minBot = modalityGroup.find("minBounds").asList();
                                for (int c = 0; c < minBot.size(); c++)
                                    minBounds[c] = (float)minBot.get(c).asDouble();
                            }

                            if (modalityGroup.check("maxBounds"))
                            {
                                maxBounds = new float[size];
                                Bottle maxBot = modalityGroup.find("maxBounds").asList();
                                for (int c = 0; c < maxBot.size(); c++)
                                    maxBounds[c] = (float)maxBot.get(c).asDouble();
                            }
					   mod = new YarpModalityVector(mapName, name, size, minBounds, maxBounds, isBlocking);

                            if (modalityGroup.check("autoconnect"))
                            {
                                string sourceConnection = modalityGroup.find("autoconnect").asString().c_str();
						  while (!Network.connect(sourceConnection, YarpModalityVector.getRealPortName(mapName, name)))
						  {
							 Console.WriteLine("Waiting for..." + sourceConnection);
							 Time.delay(1.0);
						  }
                            }
                            break;
                        }

                    case "yarpString":
				    {
					   bool isBlocking = modalityGroup.check("isBlocking");
					   mod = new YarpModalityString(mapName, name, isBlocking);
					   Console.WriteLine("isBlocking: " + isBlocking);

                            if (modalityGroup.check("autoconnect"))
                            {
                                string sourceConnection = modalityGroup.find("autoconnect").asString().c_str();
                                Network.connect(sourceConnection, YarpModalityVector.getRealPortName(mapName, name));
                            }
                            break;
                        }

                    case "yarpImageRgb":
				    {
					   bool isBlocking = modalityGroup.check("isBlocking");
					   Console.WriteLine("isBlocking: " + isBlocking);
                            int wImg = modalityGroup.check("width", new Value(32)).asInt();
                            int hImg = modalityGroup.check("height", new Value(32)).asInt();
					   mod = new YarpModalityImageRgb(mapName, name, wImg, hImg, isBlocking);
                            Console.WriteLine("Resolution: " + wImg + "x" + hImg);
                            if (modalityGroup.check("autoconnect"))
                            {
                                string sourceConnection = modalityGroup.find("autoconnect").asString().c_str();
                                Network.connect(sourceConnection, YarpModalityVector.getRealPortName(mapName, name));
                            }
                            break;
                        }

                    case "yarpImageFloat":
				    {
					   bool isBlocking = modalityGroup.check("isBlocking");
					   Console.WriteLine("isBlocking: " + isBlocking);
                            int wImg = modalityGroup.check("width", new Value(32)).asInt();
                            int hImg = modalityGroup.check("height", new Value(32)).asInt();
                            int padding = modalityGroup.check("padding", new Value(0)).asInt();
					   mod = new YarpModalityImageFloat(mapName, name, wImg, hImg, padding, isBlocking);
                            Console.WriteLine("Resolution: " + wImg + "x" + hImg);
                            if (modalityGroup.check("autoconnect"))
                            {
                                string sourceConnection = modalityGroup.find("autoconnect").asString().c_str();
                                Network.connect(sourceConnection, YarpModalityVector.getRealPortName(mapName, name));
                            }
                            break;
                        }
				case "yarpSound":
				    {
					   bool isBlocking = modalityGroup.check("isBlocking");
					   Console.WriteLine("isBlocking: " + isBlocking);
					   int size = modalityGroup.check("size", new Value(512)).asInt();
					   mod = new YarpModalitySound(mapName, name, size, isBlocking);
					   Console.WriteLine("Buffer: " + size);
					   if (modalityGroup.check("autoconnect"))
					   {
						  string sourceConnection = modalityGroup.find("autoconnect").asString().c_str();
						  Network.connect(sourceConnection, YarpModalityVector.getRealPortName(mapName, name));
					   }
					   break;
				    }
                }

                Console.WriteLine();
                //Unknown modality type
                if (mod == null)
                    return null;

                cvz.AddModality(mod, influence);
            }

            return cvz;
        }
    }
}
