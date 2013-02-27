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

namespace MMCMLibrary
{
    [Serializable]
    public class CVZ_MMCM : IConvergenceZone
    {
        /// <summary>
        /// The map size 
        /// To do : set accessers for read only
        /// </summary>
        private int height, width, layers;
        //private bool visualizationON = false;

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
        public int Layers
        {
            get { return layers; }
            set { layers = value; }
        }
        //public bool VisualizationON
        //{
        //    get { return visualizationON; }
        //    set { visualizationON = value; }
        //}

        /// <summary>
        /// The map activity
        /// </summary>
        public float[, ,] mapActivity;

        /// <summary>
        /// Contains the weights for each modality. (Indexed by modality name)
        /// The order is : weights[modComponent,xOnMap,yOnMap,zOnMap]
        /// </summary>
        public Dictionary<string, float[, , ,]> weights = new Dictionary<string,float[,,,]>();

        private float learningRate = 0.1f;
        /// <summary>
        /// Obtain the learning rate property. A negative learning rate means no learning
        /// </summary>
        public float LearningRate
        {
            get { return learningRate; }
            set { learningRate = value; }
        }

        private float sigma = 20.0f;
        /// <summary>
        /// Obtain the neighboorhood size
        /// </summary>
        public float Sigma
        {
            get { return sigma; }
            set { if (value > 0) sigma = value; else throw new Exception("Sigma cannot be negative."); }
        }

        /// <summary>
        /// A list of the electrodes attached to the map.
        /// </summary>
        public List<MMCM_Electrode> electrodes = new List<MMCM_Electrode>();

        protected int lastWinX, lastWinY, lastWinZ;

        protected override float[] feedForward()
        {
            float[] scaledWinner = new float[] { lastWinX / (float)width, lastWinY / (float)height, lastWinZ / (float)layers };
            return scaledWinner;
        }

        protected override int hierarchicalModalityDesiredSize()
        {
            return 3;
        }

        /// <summary>
        /// Basic constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="_height"></param>
        /// <param name="_width"></param>
        public CVZ_MMCM(string name, int _width,int _height , int _layers)
            : base(name)
        {
            height = _height;
            width = _width;
            layers = _layers;

            mapActivity = new float[width,height, layers];
            weights = new Dictionary<string, float[, , ,]>();

            sigma = (float)((1 / 4.0) * (height + width) / 2.0);

            this.CreateHierarchicalModality(3, 0.0f);
        }

        protected virtual void RebuildWeightsMatrix()
        {
            mapActivity = new float[width, height, layers];

            System.Random rand = new System.Random();
            weights.Clear();
            foreach (string key in modalities.Keys)
            {

                weights[key] = new float[modalities[key].Size, width, height, layers];
                //Randomize the weights
                for (int m = 0; m < modalities[key].Size; m++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        for (int y = 0; y < height; y++)
                        {
                            for (int z = 0; z < layers; z++)
                            {
                                weights[key][m, x, y, z] = (float)rand.NextDouble();
                            }
                        }
                    }
                }
            }
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
            RebuildWeightsMatrix();
        }

        /// <summary>
        /// Remove a modality
        /// </summary>
        /// <param name="modalityName"></param>
        public override void RemoveModality(string modalityName)
        {
            base.RemoveModality(modalityName);
            RebuildWeightsMatrix();
        }

        /// <summary>
        /// Calculate and update the predicted values for all modalities
        /// that are connected to the map.
        /// </summary>
        protected override void ComputePredictedValues()
        {
            int yWin = 0,
                xWin = 0,
                zWin = 0;
            CalculateMapActivity(ref yWin, ref xWin, ref zWin);

            //Get the predicted values from the winner
            Parallel.ForEach(modalities, m =>
            {
                Parallel.For(0, m.Value.Size, delegate(int i)
                {
                    m.Value.PredictedValue[i] = weights[m.Key][i, xWin, yWin, zWin];
                });
            });

            //Train the map if needed
            if (LearningRate > 0)
            {
                AdaptWeights(xWin, yWin, zWin);
            }

            base.ComputePredictedValues();
        }

        private void ResetMapActivity()
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    for (int z = 0; z < layers; z++)
                    {
                        mapActivity[x, y, z] = 0.0f;
                    }
                }
            }
        }

        /// <summary>
        /// Calculate the map activity based on the current perceived
        /// values of every modality.
        /// </summary>
        protected virtual void CalculateMapActivity(ref int yWin, ref int xWin, ref int zWin)
        {
            //Zero the map activity
            ResetMapActivity();

            //Calculate the influence sum to normalize the map activity
            //this can be executed only one time if the influence doesn't change over time
            //(to save computation time)
            float influenceSum = 0;
            int activeModalities = 0;
            foreach (KeyValuePair<string, float> influence in modalitiesInfluence)
            {
                influenceSum += influence.Value;
            }

            if (HierarchicalModalitySet)
                influenceSum += (float)feedbackInfluence;

            //Compute the map activity
            Parallel.For(0, height, delegate(int y)
            {
                Parallel.For(0, width, delegate(int x)
                {
                    Parallel.For(0, layers, delegate(int z)
                    {
                        Parallel.ForEach(modalities, m =>
                        //foreach (KeyValuePair<string, IModality> m in modalities)
                        {
                            //Temporary value for the activity.
                            //We don't add it directly to the map in order to
                            //take into account the influence of the modality.
                            float a = 0;
                            for (int i = 0; i < m.Value.PerceivedValue.Count(); i++)
                            {
                                float[, , ,] w = weights[m.Key];
                                a += Math.Abs(w[i, x, y, z] - m.Value.PerceivedValue[i]);
                            }
                            a = modalitiesInfluence[m.Key] * a / m.Value.Size;
                            mapActivity[x, y, z] += a;
                        });

                        //Move toward the feedback            
                        if (HierarchicalModalitySet)
                        {
                            float[] FBWinner = FeedBack;
                            float aFB = 0;
                            aFB += Math.Abs(x / (float)width - FeedBack[0]);
                            aFB += Math.Abs(y / (float)height - FeedBack[1]);
                            aFB += Math.Abs(z / (float)layers - FeedBack[2]);

                            aFB = (float)feedbackInfluence * aFB / hierarchicalModalityDesiredSize();
                            mapActivity[x, y, z] += aFB;
                        }
                        if (influenceSum != 0)
                            mapActivity[x, y, z] = mapActivity[x, y, z] / influenceSum;
                        else
                            mapActivity[x, y, z] = 0;

                        if (mapActivity[x, y, z] > 1)
                        {
                            //System.Windows.Forms.MessageBox.Show("Probleme.");
                        }
                    });
                });
            });

            //Detect the winner                        
            xWin = 0;
            yWin = 0;
            zWin = 0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    for (int z = 0; z < layers; z++)
                    {
                        if (mapActivity[x, y, z] < mapActivity[xWin, yWin, zWin])
                        {
                            yWin = y;
                            xWin = x;
                            zWin = z;
                        }
                    }
                }
            }

            lastWinX = xWin;
            lastWinY = yWin;
            lastWinZ = zWin;

            //Record the activity on the electrodes
            foreach (MMCM_Electrode elec in electrodes)
            {
                elec.activity = mapActivity[elec.x, elec.y, elec.z];
            }
        }

        /// <summary>
        /// Predict the output on a modality of certain neuron (its receptive field)
        /// </summary>
        /// <param name="modalityName">Name of the modality to predict</param>
        /// <param name="yWin">y of the Winner on the map</param>
        /// <param name="xWin">x of the Winner on the map</param>
        /// <returns>The predicted vector</returns>
        private float[] Predict(string modalityName, int xWin, int yWin, int zWin)
        {
            float[] pred = new float[modalities[modalityName].Size];
            for (int i = 0; i < pred.Count(); i++)
            {
                pred[i] = weights[modalityName][i, xWin, yWin, zWin];
            }
            return pred;
        }

        protected virtual void AdaptWeights(int xWin, int yWin, int zWin)
        {
            //sigma = Math.Pow(height * width * layers, 1 / 3.0) * (float)(1.0 / 20.0f); ;
            Parallel.ForEach(weights, w =>
            //foreach (KeyValuePair<string, float[, , ,]> w in weights)
            {
                Parallel.For(0, w.Value.GetLength(0), delegate(int comp)
                {
                    float winnerError = (float)Math.Pow(modalities[w.Key].PerceivedValue[comp] - w.Value[comp, xWin, yWin, zWin], 2.0);
                    Parallel.For(0, height, delegate(int y)
                    {
                        Parallel.For(0, width, delegate(int x)
                        {
                            Parallel.For(0, layers, delegate(int z)
                            {
                                float distance = (float)Math.Sqrt(Math.Pow(x - xWin, 2.0) + Math.Pow(y - yWin, 2.0) + Math.Pow(z - zWin, 2.0));
                                //float distanceCoef = MathFunctions.MexicanHat(distance, sigma);
                                float distanceCoef = MathFunctions.GaussianBell(distance, sigma);

                                float error = modalities[w.Key].PerceivedValue[comp] - w.Value[comp, x, y, z];

                                w.Value[comp, x, y, z] +=
                                    learningRate *
                                    distanceCoef *
                                    //winnerError *
                                    error;

                                //Keep the weights beetween 0 and 1
                                w.Value[comp, x, y, z] = (float)Math.Max(0.0, w.Value[comp, x, y, z]);
                                w.Value[comp, x, y, z] = (float)Math.Min(1.0, w.Value[comp, x, y, z]);
                            });
                        });
                    });
                });
            });
        }

        #region GUI

        public System.Drawing.Bitmap GetVisualization(int layer)
        {
            double time1 = Time.now();

            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(width, height);

            float[, ,] scaled = (float[, ,])mapActivity.Clone();
            float max = float.NegativeInfinity;
            float min = float.PositiveInfinity;

            //Scale for visualization
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    scaled[x, y, layer] = (float)Math.Pow(mapActivity[x, y, layer], 2.0);
                    min = Math.Min(scaled[x, y, layer], min);
                    max = Math.Max(scaled[x, y, layer], max);
                }
            }
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    scaled[x, y, layer] = (scaled[x, y, layer] - min) / (max - min);
                }
            }

            //Build the bmp
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    System.Drawing.Color color = HelpersLib.ColorInterpolator.InterpolateBetween(Color.Blue, Color.Yellow, Color.Red, (float)scaled[x, y, layer]);

                    bmp.SetPixel(x, y, color);

                    if (x == lastWinX && y == lastWinY && layer == lastWinZ)
                    {
                        bmp.SetPixel(x, y, Color.Red);
                    }
                }
            }

            foreach (MMCM_Electrode e in electrodes)
            {
                if (e.x < width && e.x >= 0 &&
                    e.y < height && e.y >= 0 &&
                    e.z == layer)
                    bmp.SetPixel(e.x, e.y, Color.HotPink);
            }

            double time2 = Time.now();
            //Console.WriteLine("Visualization computed in " + (time2 - time1).ToString());
            return bmp;
        }

        /// <summary>
        /// Return a winform used to control the parameters of the MMCM
        /// </summary>
        /// <returns></returns>
        public MMCM_ControlPanel GetControlPanel()
        {
            return new MMCM_ControlPanel(this);
        }

        #endregion GUI

        #region Serialization
        //Deserialization constructor.

        public CVZ_MMCM(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
        {
            //Get the values from info and assign them to the appropriate properties
            height = (int)info.GetValue("Height", typeof(int));
            width = (int)info.GetValue("Width", typeof(int));
            layers = (int)info.GetValue("Layers", typeof(int));
            weights = new Dictionary<string, float[, , ,]>();
            mapActivity = new float[width, height, layers];
            CreateHierarchicalModality(3, 0.0f);
            int modalitiesCount = (int)info.GetValue("ModalitiesCount", typeof(int));
            for (int i = 0; i < modalitiesCount; i++)
            {
                float[, , ,] w = (float[, , ,])info.GetValue("ModalityWeights" + i, typeof(float[, , ,]));
                weights.Add(modalities.ElementAt(i).Key, w);
            }

        }

        //Serialization function.
        public override void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            base.GetObjectData(info, ctxt);
            info.AddValue("Height", height);
            info.AddValue("Width", width);
            info.AddValue("Layers", layers);
            int i = 0;
            foreach (KeyValuePair<string, float[, , ,]> w in weights)
            {
                info.AddValue("ModalityWeights" + i, w.Value);
                i++;
            }
        }
        #endregion Serialization
    }
}
