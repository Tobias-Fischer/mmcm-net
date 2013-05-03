using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;
using CVZ_Core.Modalities;

namespace CVZ_Core.CVZ.CTPC
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
        /// The TPC vector representing the averaged activity of the network over a given timelapse
        /// </summary>
        float[] tpcVector;

        private double learningRate = 0.0;
        /// <summary>
        /// Learning rate of the network. If 0.0 no learning occurs.
        /// </summary>
        public double LearningRate { get { return learningRate; } set { learningRate = MathFunctions.Clamp((float)value, 0.0f, 1.0f); } }

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
        public CVZ_TPC(string name, int _width,int _height, double neighboorhoudRadius, bool isHierarchicallySynchrone)
            : base(name)
        {
            height = _height;
            width = _width;
            neurons = new NeuronIzhikevich[width,height];
            this.neighborhoodRadius = neighboorhoudRadius;
            inputConnections = new Dictionary<string, List<NeuronIzhikevich.Connection>[]>();
            lateralConnections = new List<NeuronIzhikevich.Connection>();
            buildNeurons();
            setLateralConnectivity(neighboorhoudRadius);
            setInputConnectivity();
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
            setInputConnectivity();
        }

        /// <summary>
        /// Remove a modality
        /// </summary>
        /// <param name="modalityName"></param>
        public override void RemoveModality(string modalityName)
        {
            base.RemoveModality(modalityName);
            setInputConnectivity();
        }

        /// <summary>
        /// Build the neurons of the map
        /// </summary>
        protected void buildNeurons()
        {
            //Build the neurons
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    neurons[x, y] = new NeuronIzhikevich();
                    neurons[x, y].tag_x = x;
                    neurons[x, y].tag_y = y;
                }
            }
        }

        /// <summary>
        /// Reset the local connectivity of the map
        /// </summary>
        /// <param name="neighboorRadius"></param>
        protected void setLateralConnectivity(double neighboorRadius, double lateralW = 0.4)
        {
            //Reset the lateral connextivity the neurons
            lateralConnections.Clear();
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    neurons[x, y].lateralProjections.Clear();
                }
            }

            //Build the connections
            for(int y=0;y<height;y++)
            {
                for(int x=0;x<width;x++)
                {
                    //Go through all the connected neurons
                    for (int y2 = y - (int)neighboorRadius;  y2 <= y + (int)neighboorRadius ; y2++)
                    {
                        for (int x2 = x - (int)neighboorRadius;  x2 <= x + (int)neighboorRadius ; x2++)
                        {

                            //Calculate the right index for this connection (here we can go circular)
                            //For now we just check if the index is inbounds
                            if (y2 >= 0 && y2 < height &&
                                x2 >= 0 && x2 < width &&
                                (x != x2 || y != y2))
                            {
                                //if (x != x2 || y != y2)
                                {
                                    double distance = MathFunctions.EuclideanDistance(new float[] { x, y }, new float[] { x2, y2 });
                                    //Console.WriteLine("Distance = " + distance + " Neighbor = " + neighboorRadius);
                                    //If the neuron is in the neihboor range
                                    if (distance < neighboorRadius)
                                    {
                                        //We add a connection with strenght shaped by a mexican hat
                                        //We calculate sigma to so that the inhibition takes place on the border of the neighborhood
                                        float sigma = (float)neighboorRadius / 4.0f;
                                        float strenght = (float)MathFunctions.MexicanHat((float)distance, sigma);
                                        //Console.Write(strenght);
                                        //float strenght = (float)lateralW;
                                        NeuronIzhikevich.Connection cnt = new NeuronIzhikevich.Connection(neurons[x2, y2], distance, strenght);
                                        lateralConnections.Add(cnt);
                                        neurons[x, y].lateralProjections.Add(cnt);
                                    }
                                }
                            }
                        }
                        //Console.WriteLine();
                    }
                }
            }
        }

        /// <summary>
        /// build the input connectivity matrix.
        /// </summary>
        public void setInputConnectivity()
        {
            System.Random rand = new System.Random();
            inputConnections.Clear();

            foreach (NeuronIzhikevich n in neurons)
            {
                n.inputsConnections.Clear();
            }

            foreach (string key in modalities.Keys)
            {
                inputConnections[key] = new List<NeuronIzhikevich.Connection>[modalities[key].Size];
                for (int m = 0; m < modalities[key].Size; m++)
                {
                    inputConnections[key][m] = new List<NeuronIzhikevich.Connection>();                           
                    //Create the connection with a random weight
                    //inputConnections[key][m].Add(new NeuronIzhikevich.Connection(ref neurons[x, y], 1.0, rand.NextDouble()));                
                    for (int x = 0; x < width; x++)
                    {
                        for (int y = 0; y < height; y++)
                        {
                            NeuronIzhikevich.Connection c = new NeuronIzhikevich.Connection(neurons[x, y], 1.0, 0.5 /*rand.NextDouble()*/);
                            inputConnections[key][m].Add(c);
                            neurons[x, y].inputsConnections.Add(c);
                        }
                    }
                }
            }   
        }

        /// <summary>
        /// force the receptive field of a given area
        /// </summary>
        /// <param name="connectivityRatio"> define the probability for a connection to exist</param>
        public void setReceptiveField(int xCenter, int yCenter, double[] receptiveField)
        {
            //Go through all the connected neurons
            for (int y2 = yCenter - (int)neighborhoodRadius; y2 <= yCenter + (int)neighborhoodRadius; y2++)
            {
                for (int x2 = xCenter - (int)neighborhoodRadius; x2 <= xCenter + (int)neighborhoodRadius; x2++)
                {

                    //Calculate the right index for this connection (here we can go circular)
                    //For now we just check if the index is inbounds
                    if (y2 >= 0 && y2 < height &&
                        x2 >= 0 && x2 < width )
                    {
                        if (receptiveField.Length != neurons[x2, y2].inputsConnections.Count)
                        {
                            throw new Exception("Receptive fields size do not match");
                        }
                        else
                        {
                            for (int m = 0; m < receptiveField.Length; m++)
                            {
                                neurons[x2, y2].inputsConnections[m].w = receptiveField[m];
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

        bool testFirstTrial = true;
        /// <summary>
        /// Calculate and update the predicted values for all modalities
        /// that are connected to the map.
        /// 1 call correspond to 1ms in real time
        /// </summary>
        protected override void ComputePredictedValues()
        {   
            //Debug
            if (testFirstTrial)
            {
                testFirstTrial = false;
                setReceptiveField(10, 10, new double[] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 });
                setReceptiveField(30, 30, new double[] { 1.0, 1.0, 1.0, 1.0, 1.0, 1.0 });
            }

            // 1 --- Make the existing spikes to travel
            foreach(NeuronIzhikevich.Connection cnt in lateralConnections)
            {
                //cnt.update();
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
                        c.propagateSimilarity(modalities[m.Key].PerceivedValue[e], modalitiesInfluence[m.Key] / (double)m.Value.Count());
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

            // 5 --- Adapt the weights
            if (LearningRate > 0.0)
            {
                foreach (KeyValuePair<string, List<NeuronIzhikevich.Connection>[]> m in inputConnections)
                {
                    //Modality components
                    for (int e = 0; e < m.Value.Count(); e++)
                    {
                        //Connections orginating from here
                        foreach (NeuronIzhikevich.Connection c in m.Value[e])
                        {
                            //We learn only if the target neuron has spiked
                            if (c.target.HasJustSpiked)
                            {
                                double dW = (modalities[m.Key].PerceivedValue[e] - c.w) * learningRate;
                                c.w += dW;
                            }
                        }
                    }
                }
            }

            base.ComputePredictedValues();
        }

        #region GUI

	   //public bool GetVisualization(ref ImageRgb img)
	   //{
	   //    return HelpersLib.ImageManipulation.toImg(GetSpikeVisualization(), ref img);
	   //    //return HelpersLib.ImageManipulation.toImg(GetFrequencyVisualization(), ref img);
	   //    //return HelpersLib.ImageManipulation.toImg(GetMembranePotentialVisualization(), ref img);
	   //}

        public System.Drawing.Bitmap GetSpikeVisualization()
        {

            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(width, height);

            //Build the bmp
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (neurons[x,y].HasJustSpiked)
                        bmp.SetPixel(x, y, Color.Green);
                    else
                        bmp.SetPixel(x, y, Color.Black);
                }
            }

            //Console.WriteLine("Visualization computed in " + (time2 - time1).ToString());
            return bmp;
        }

        public System.Drawing.Bitmap GetFrequencyVisualization()
        {

            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(width, height);

            //Build the bmp
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    System.Drawing.Color color = ColorInterpolator.InterpolateBetween(Color.Blue, Color.Yellow, Color.Red, neurons[x, y].GetSpikingFrequency());
                    bmp.SetPixel(x, y, color);
                }
            }
            //Console.WriteLine("Visualization computed in " + (time2 - time1).ToString());
            return bmp;
        }

        public System.Drawing.Bitmap GetMembranePotentialVisualization()
        {

            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(width, height);

            //Build the bmp
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    System.Drawing.Color color = ColorInterpolator.InterpolateBetween(Color.Blue, Color.Yellow, Color.Red, neurons[x, y].V);
                    bmp.SetPixel(x, y, color);
                }
            }

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
            setLateralConnectivity(neighborhoodRadius);
            setInputConnectivity();
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
        }
        #endregion Serialization
    }
}
