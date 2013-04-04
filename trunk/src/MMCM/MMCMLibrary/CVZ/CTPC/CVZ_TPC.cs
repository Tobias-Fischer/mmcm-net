using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using HelpersLib;
using System.Threading;
using System.Threading.Tasks;
using MMCMLibrary.Modalities;

namespace MMCMLibrary.CVZ.CTPC
{
    [Serializable]
    public class CVZ_TPC : IConvergenceZone
    {
        /// <summary>
        /// The map size 
        /// To do : set accessers for read only
        /// </summary>
        private int height, width;

        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        /// <summary>
        /// The network neurons
        /// </summary>
        /// 
        public NeuronIzhikevich[,] neurons;

        /// <summary>
        /// The connections between neurons of the network
        /// </summary>
        private List<NeuronIzhikevich.Connection> lateralConnections;

        /// <summary>
        /// The connections from the modality layer to the network
        /// </summary>
        private Dictionary<string, List<NeuronIzhikevich.Connection>[]> inputConnections;

        /// <summary>
        /// The radius defining the lateral connectivity
        /// </summary>
        private double neighborhoodRadius;

        /// <summary>
        /// The probabiliy of a connection between a neuron of the input and a neuron of the network
        /// </summary>
        private double inputConnectivityProbability;

        float[] tpcVector;

        /// <summary>
        /// The activity to be propagated to upper cvz (the TPC)
        /// </summary>
        /// <returns></returns>
        protected override float[] feedForward()
        {
            return tpcVector;
        }

        /// <summary>
        /// The desired size of the TPC vector (defined by the buffering time)
        /// </summary>
        /// <returns></returns>
        protected override int hierarchicalModalityDesiredSize()
        {
            return 128;
        }

        /// <summary>
        /// Basic constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="_height"></param>
        /// <param name="_width"></param>
        public CVZ_TPC(string name, int _width,int _height, double neighboorhoudRadius, double inputConnectivityProbability, bool isHierarchicallySynchrone)
            : base(name)
        {
            height = _height;
            width = _width;
            neurons = new NeuronIzhikevich[width,height];
            this.neighborhoodRadius = neighboorhoudRadius;
            this.inputConnectivityProbability = inputConnectivityProbability;
            inputConnections = new Dictionary<string, List<NeuronIzhikevich.Connection>[]>();
            lateralConnections = new List<NeuronIzhikevich.Connection>();
            resetNetwork(neighboorhoudRadius);
            resetInputConnections(inputConnectivityProbability);
            tpcVector = new float[hierarchicalModalityDesiredSize()];
		    this.CreateHierarchicalModality(hierarchicalModalityDesiredSize(), 0.0f, isHierarchicallySynchrone);
        }

        /// <summary>
        /// Connect a modality
        /// </summary>
        /// <param name="modalityName">Modality name</param>
        /// <param name="size">Modality vector size</param>
        /// <param name="influence">Modality influence on the map</param>
        public override void AddModality(IModality m, float influence)
        {
            base.AddModality(m, influence);
            resetInputConnections(inputConnectivityProbability);
        }

        /// <summary>
        /// Remove a modality
        /// </summary>
        /// <param name="modalityName"></param>
        public override void RemoveModality(string modalityName)
        {
            base.RemoveModality(modalityName);
            resetInputConnections(inputConnectivityProbability);
        }

        /// <summary>
        /// Reset the local connectivity of the map
        /// </summary>
        /// <param name="neighboorRadius"></param>
        protected void resetNetwork(double neighboorRadius)
        {
            //Build the neurons
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    neurons[x, y] = new NeuronIzhikevich();
                }
            }

            //Build the connections
            lateralConnections.Clear();
            for(int y=0;y<height;y++)
            {
                for(int x=0;x<width;x++)
                {

                    //Go through all the neurons
                    for(int y2=0;y2<height;y2++)
                    {
                        for(int x2=0;x2<width;x2++)
                        {
                            if (x != x2 && y != y2)
                            {
                                double distance = MathFunctions.EuclideanDistance(new float[] { x, y }, new float[] { x2, y2 });
                                
                                //If the neuron is in the neihboor range
                                if (distance  < neighboorRadius)
                                {
                                    //We add a connection
                                    NeuronIzhikevich.Connection cnt = new NeuronIzhikevich.Connection(ref neurons[x2, y2], distance, 1.0);
                                    lateralConnections.Add(cnt);
                                    neurons[x, y].projections.Add(cnt);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// build the input connectivity matrix.
        /// </summary>
        /// <param name="connectivityRatio"> define the probability for a connection to exist</param>
        public void resetInputConnections(double connectivityProbability = 1.0f)
        {
            System.Random rand = new System.Random();
            inputConnections.Clear();

            foreach (string key in modalities.Keys)
            {
                inputConnections[key] = new List<NeuronIzhikevich.Connection>[modalities[key].Size];

                //Go through all network neurons
                for (int m = 0; m < modalities[key].Size; m++)
                {
                    inputConnections[key][m] = new List<NeuronIzhikevich.Connection>();

                    for (int x = 0; x < width; x++)
                    {
                        for (int y = 0; y < height; y++)
                        {
                            //Roll a dice to see if this connection exists
                            if (rand.NextDouble() < connectivityProbability)
                            {
                                //Create the connection with a random weight
                                inputConnections[key][m].Add(new NeuronIzhikevich.Connection( ref neurons[x,y], 1.0, rand.NextDouble() ));
                            }
                        }
                    }
                }
            }
            
        }

        /// <summary>
        /// Calculate and update the predicted values for all modalities
        /// that are connected to the map.
        /// 1 call correspond to 1ms in real time
        /// </summary>
        //protected override void ComputePredictedValues()
        //{
        //    // 1 --- Make the existing spikes to travel
        //    Parallel.ForEach(lateralConnections, cnt =>
        //    {
        //        cnt.update();
        //    });

        //    // 2 --- Propagate the input
        //    //Modalities
        //    Parallel.ForEach(inputConnections, m =>
        //    //foreach (KeyValuePair<string, IModality> m in modalities)
        //    {
        //        //Modality components
        //        Parallel.For(0, m.Value.Count(), delegate(int e)
        //        {
        //            //Connections orginating from here
        //            Parallel.ForEach(m.Value[e], c =>
        //            //foreach (KeyValuePair<string, IModality> m in modalities)
        //            {
        //                c.propagateSimilarity(modalities[m.Key].PerceivedValue[e], modalitiesInfluence[m.Key]);
        //            });
        //        });
        //    });


        //    // 3 --- Update the neurons
        //    Parallel.For(0, height, delegate(int y)
        //    {
        //        Parallel.For(0, width, delegate(int x)
        //        {
        //            neurons[x, y].update();
        //        });
        //    });

        //    // 4 --- Compute the predicted activity ?!

        //    base.ComputePredictedValues();
        //}

        /// <summary>
        /// Calculate and update the predicted values for all modalities
        /// that are connected to the map.
        /// 1 call correspond to 1ms in real time
        /// </summary>
        protected override void ComputePredictedValues()
        {   
            // 1 --- Make the existing spikes to travel
            foreach(NeuronIzhikevich.Connection cnt in lateralConnections)
            {
                cnt.update();
            }

            // 2 --- Propagate the input
            //Modalities
            foreach(KeyValuePair<string, List<NeuronIzhikevich.Connection>[]> m in inputConnections)
            {
                //Modality components
                for(int e = 0; e< m.Value.Count();e++)
                {
                    //Connections orginating from here
                    foreach(NeuronIzhikevich.Connection c in  m.Value[e])
                    //foreach (KeyValuePair<string, IModality> m in modalities)
                    {
                        c.propagateSimilarity(modalities[m.Key].PerceivedValue[e], modalitiesInfluence[m.Key]);
                    }
                }
            }


            // 3 --- Update the neurons
            for(int y=0;y< height;y++)
            {
                for(int x=0;x< width;x++)
                {
                    neurons[x, y].update();
                }
            }

            // 4 --- Compute the predicted activity ?!

            base.ComputePredictedValues();
        }
        #region GUI

        public bool GetVisualization(ref ImageRgb img)
        {
            return HelpersLib.ImageManipulation.toImg(GetVisualization(), ref img);
        }

        public System.Drawing.Bitmap GetVisualization()
        {
            double time1 = Time.now();

            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(width, height);

            float[,] scaled = new float[width, height];
            float max = float.NegativeInfinity;
            float min = float.PositiveInfinity;

            //Scale for visualization
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    scaled[x, y] = (float)Math.Pow(neurons[x,y].V, 2.0);
                    min = Math.Min(scaled[x, y], min);
                    max = Math.Max(scaled[x, y], max);
                }
            }
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    scaled[x, y] = (scaled[x, y] - min) / (max - min);
                }
            }

            //Build the bmp
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    System.Drawing.Color color = HelpersLib.ColorInterpolator.InterpolateBetween(Color.Blue, Color.Yellow, Color.Red, (float)scaled[x, y]);
                    bmp.SetPixel(x, y, color);
                }
            }

            double time2 = Time.now();
            //Console.WriteLine("Visualization computed in " + (time2 - time1).ToString());
            return bmp;
        }

        #endregion GUI

        #region Serialization
        //Deserialization constructor.

        public CVZ_TPC(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
        {
            //Get the values from info and assign them to the appropriate properties
            height = (int)info.GetValue("Height", typeof(int));
            width = (int)info.GetValue("Width", typeof(int));
            neighborhoodRadius = (double)info.GetValue("radius", typeof(double));
            inputConnectivityProbability = (double)info.GetValue("connectivityProba", typeof(double));
            resetNetwork(neighborhoodRadius);
            resetInputConnections(inputConnectivityProbability);
            CreateHierarchicalModality(hierarchicalModalityDesiredSize(), 0.0f, false);//potential bug with herarchicalSYnchro
            int modalitiesCount = (int)info.GetValue("ModalitiesCount", typeof(int));
        }

        //Serialization function.
        public override void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            base.GetObjectData(info, ctxt);
            info.AddValue("Height", height);
            info.AddValue("Width", width);
            info.AddValue("radius", neighborhoodRadius);
            info.AddValue("connectivityProba", inputConnectivityProbability);
        }
        #endregion Serialization
    }
}
