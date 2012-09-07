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
using Microsoft.ParallelArrays;
using FPA = Microsoft.ParallelArrays.FloatParallelArray;
using PA = Microsoft.ParallelArrays.ParallelArrays;
using GPUTarget = Microsoft.ParallelArrays.MulticoreTarget;
using MMCMLibrary.Modalities;

namespace MMCMLibrary
{
    [Serializable]
    public class CVZ_MMCM_GPU : CVZ_MMCM
    {
        /// <summary>
        /// The map activity
        /// </summary>
        private float[][,] activity;

        public enum OptimizationType { input, map, auto };

        OptimizationType desiredOptimizationType;
        OptimizationType currentOptimizationType;

        /// <summary>
        /// Contains the weights for each modality. (Indexed by modality name)
        /// The order is : weights[modalityComponent][layer][x,y]
        /// </summary>
        private Dictionary<string, float[][][,]> mapOptimizedWeights = new Dictionary<string, float[][][,]>();

        /// <summary>
        /// Contains the weights for each modality
        /// THe order is : inputOptimizedWeights[x,y,z]
        /// </summary>
        private Dictionary<string, float[, ,][]> inputOptimizedWeights = new Dictionary<string, float[, ,][]>();

        /// <summary>
        /// Basic constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="_height"></param>
        /// <param name="_width"></param>
        public CVZ_MMCM_GPU(string name, int _height, int _width, int _layers, OptimizationType _optimizationType = OptimizationType.auto )
            : base(name, _height,_width,_layers)
        {
            desiredOptimizationType = _optimizationType;
            RebuildWeightsMatrix();
        }

        public void RebuildWeightsMatrix()
        {
            if (desiredOptimizationType == OptimizationType.auto)
                currentOptimizationType = shouldUseInputOptimization();
            else
                currentOptimizationType = desiredOptimizationType;

            Console.WriteLine("Using inputOptimization : " + currentOptimizationType);

            activity = new float[Layers][,];

            for(int L=0;L<Layers;L++)
            {
                activity[L] = new float[Width, Height];
            }
             
            System.Random rand = new System.Random();
            if (currentOptimizationType == OptimizationType.input)
            {
                foreach (string key in modalities.Keys)
                {
                    inputOptimizedWeights[key] = new float[Width, Height, Layers][];

                    for (int z = 0; z < Layers; z++)
                    {
                        for (int y = 0; y < Height; y++)
                        {
                            for (int x = 0; x < Width; x++)
                            {
                                inputOptimizedWeights[key][x, y, z] = new float[modalities[key].Size];
                                for (int m = 0; m < modalities[key].Size; m++)
                                {
                                    inputOptimizedWeights[key][x, y, z][m] = (float)(rand.NextDouble());
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (string key in modalities.Keys)
                {
                    mapOptimizedWeights[key] = new float[modalities[key].Size][][,];


                    for (int m = 0; m < modalities[key].Size; m++)
                    {
                        mapOptimizedWeights[key][m] = new float[Layers][,];

                        for (int z = 0; z < Layers; z++)
                        {
                            mapOptimizedWeights[key][m][z] = new float[Width, Height];

                            for (int y = 0; y < Height; y++)
                            {
                                for (int x = 0; x < Width; x++)
                                {
                                    mapOptimizedWeights[key][m][z][x, y] = (float)(rand.NextDouble());
                                }
                            }
                        }
                    }
                }
            }

        }

        OptimizationType shouldUseInputOptimization()
        {
            int totalInputSize = 0;
            int totalMapSize = Layers * Width * Height;
            foreach (string key in modalities.Keys)
            {
                totalInputSize += modalities[key].Size;
            }

            if (totalInputSize > totalMapSize)
                return OptimizationType.input;
            else
                return OptimizationType.map;
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
            mapOptimizedWeights.Add(m.name, null);
            inputOptimizedWeights.Add(m.name, null);
            RebuildWeightsMatrix();
        }

        /// <summary>
        /// Remove a modality
        /// </summary>
        /// <param name="modalityName"></param>
        public override void RemoveModality(string modalityName)
        {
            mapOptimizedWeights.Remove(modalityName);
            inputOptimizedWeights.Remove(modalityName);
            base.RemoveModality(modalityName);
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
            foreach (KeyValuePair<string, IModality> m in modalities)
            {
                for (int i = 0; i < m.Value.Size; i++)
                {
                    if (currentOptimizationType == OptimizationType.input)
                        m.Value.PredictedValue[i] = inputOptimizedWeights[m.Key][xWin, yWin, zWin][i];
                    else
                        m.Value.PredictedValue[i] = mapOptimizedWeights[m.Key][i][zWin][xWin, yWin];
                }
            }

            //Train the map if needed
            if (LearningRate > 0)
            {
                AdaptWeights(yWin, xWin, zWin);
            }

            base.ComputePredictedValues();
        }

        private void ResetMapActivity()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    for (int z = 0; z < Layers; z++)
                    {
                        activity[z] [x, y] = 0.0f;
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
            float influenceSum = 0;
            foreach (KeyValuePair<string, float> influence in modalitiesInfluence)
            {
                influenceSum += influence.Value;
            }

            if (HierarchicalModalitySet)
                influenceSum += (float)feedbackInfluence;

            if (currentOptimizationType == OptimizationType.input)
            #region inputOptimized
            {
                for (int x = 0; x < Width; x++)
                {
                    for (int y = 0; y < Height; y++)
                    {
                        for (int z = 0; z < Layers; z++)
                        {
                            FPA sumError = new FPA(0.0f, 1);
                            foreach (KeyValuePair<string, IModality> m in modalities)
                            {
                                FPA wArray = new FPA(inputOptimizedWeights[m.Key][x, y, z]);
                                FPA pArray = new FPA(m.Value.PerceivedValue);
                                FPA errorArray = PA.Subtract(pArray,wArray);
                                FPA modError = PA.Sum(errorArray);
                                modError = PA.Multiply(modError, (float)modalitiesInfluence[m.Key]);
                                modError = PA.Divide(modError, (float) m.Value.Size);
                                sumError = PA.Add(modError, sumError);
                                wArray.Dispose();
                                pArray.Dispose();
                                errorArray.Dispose();
                                modError.Dispose();
                            }
                            GPUTarget evalTarget = new GPUTarget();
                            float[] value = evalTarget.ToArray1D(sumError);
                            evalTarget.Dispose();
                            //Console.WriteLine("Error value for " + x + " " + y + " " + z + " == " + value.ToString());
                            activity[z][x, y] = value.Single();
                            sumError.Dispose();
                        }
                    }
                }

            }
            #endregion
            #region Not inputOptimized
            else
            {
                Parallel.For(0, Layers, delegate(int L)
                {

                    FPA layerError = new FPA(0.0f, new int[] { Width, Height });
                    foreach (KeyValuePair<string, IModality> m in modalities)
                    {
                        FPA layerModalityError = new FPA(0.0f, new int[] { Width, Height });
                        for (int modalityComponent = 0; modalityComponent < m.Value.Size; modalityComponent++)
                        {
                            FPA errorArray = new FPA(mapOptimizedWeights[m.Key][modalityComponent][L]);
                            errorArray = PA.Subtract((float)m.Value.PerceivedValue[modalityComponent],errorArray);
                            layerModalityError = PA.Add(errorArray, layerModalityError);
                            errorArray.Dispose();
                        }
                        layerModalityError = PA.Divide(PA.Multiply(layerModalityError, (float)modalitiesInfluence[m.Key]), (float)m.Value.Size);
                        layerError = PA.Add(layerError, layerModalityError);
                        layerModalityError.Dispose();
                    }

                    GPUTarget evalTarget = new GPUTarget();
                    activity[L] = evalTarget.ToArray2D(layerError);
                    evalTarget.Dispose();
                });
            }
            #endregion

            //Move toward the feedback            
            if (HierarchicalModalitySet)
            {
                throw new NotImplementedException("Hierarchical modality handling not implemented");
            }

            //Search the winner node (which has the minimum activity)                 
            xWin = 0;
            yWin = 0;
            zWin = 0;
            for(int L=0;L<Layers;L++)
            {      
                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        if (activity[L][x, y] < activity[zWin][xWin, yWin])
                        {
                            yWin = y;
                            xWin = x;
                            zWin = L;
                        }
                    }
                }
            }

            lastWinX = xWin;
            lastWinY = yWin;
            lastWinZ = zWin;
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
            if (currentOptimizationType == OptimizationType.input)
                for (int i = 0; i < pred.Count(); i++)
                {
                    pred[i] = inputOptimizedWeights[modalityName][xWin, yWin, zWin][i];
                }
            else
                for (int i = 0; i < pred.Count(); i++)
                {
                    pred[i] = mapOptimizedWeights[modalityName][i][zWin][xWin, yWin];
                }
            return pred;
        }

        private float[,] GetDistance(int currentLayer, int winningLayer, int xWin, int yWin)
        {
            float[,] distances = new float[Width, Height];
            for(int x=0;x<Width;x++)
                for(int y=0;y<Height;y++)
                {
                    float d = (float)Math.Sqrt(Math.Pow(x - xWin, 2.0) + Math.Pow(y - yWin, 2.0) + Math.Pow(currentLayer - winningLayer, 2.0));
                    float fd = (float) MathFunctions.GaussianBell(d, Sigma);
                    distances[x, y] = fd / (float) MathFunctions.GaussianBell(0, Sigma);
                }
            return distances;
        }

        private float GetDistance(int x, int y, int z, int xWin, int yWin, int zWin)
        {
            float d = (float)Math.Sqrt(Math.Pow(x - xWin, 2.0) + Math.Pow(y - yWin, 2.0) + Math.Pow(z - zWin, 2.0));
            float fd = (float)MathFunctions.GaussianBell(d, Sigma);
            fd /= (float)MathFunctions.GaussianBell(0, Sigma);
            return fd;
        }

        private void AdaptWeights(int xWin, int yWin, int zWin)
        {
            if (currentOptimizationType == OptimizationType.input)
            #region inputOptimized
            {
                GPUTarget evalTarget = new GPUTarget();

                for (int x = 0; x < Width; x++)
                {
                    for (int y = 0; y < Height; y++)
                    {
                        for (int z = 0; z < Layers; z++)
                        {
                            float distance = GetDistance(x, y, z, xWin, yWin, zWin);
                            foreach (KeyValuePair<string, IModality> m in modalities)
                            {
                                FPA wArray = new FPA(inputOptimizedWeights[m.Key][x, y, z]);
                                FPA pArray = new FPA(m.Value.PerceivedValue);
                                FPA errorArray = PA.Subtract(pArray,wArray);
                                FPA deltaArray = PA.Multiply(errorArray, LearningRate*distance);
                                wArray = PA.Add(wArray, deltaArray);
                                wArray = PA.Max(0.0f, PA.Min(1.0f, wArray));

                                inputOptimizedWeights[m.Key][x,y,z] = evalTarget.ToArray1D(wArray);
                                wArray.Dispose();
                                pArray.Dispose();
                                errorArray.Dispose();
                                deltaArray.Dispose();
                            }
                        }
                    }
                }
                evalTarget.Dispose();
            }
            #endregion
            #region Not inputOptimized
            else
            {
                for (int L = 0; L < Layers; L++)
                {
                    FPA distance2WinnerArray = new FPA(GetDistance(L, zWin, xWin, yWin));
                    GPUTarget evalTarget = new GPUTarget();

                    foreach (KeyValuePair<string, IModality> m in modalities)
                    {
                        for (int modalityComponent = 0; modalityComponent < m.Value.Size; modalityComponent++)
                        {
                            FPA wArray = new FPA(mapOptimizedWeights[m.Key][modalityComponent][L]);
                            FPA errorArray = PA.Subtract((float)m.Value.PerceivedValue[modalityComponent],wArray);
                            FPA deltaArray = PA.Multiply(distance2WinnerArray, PA.Multiply(errorArray, LearningRate));
                            wArray = PA.Add(wArray, deltaArray);
                            wArray = PA.Max(0.0f, PA.Min(1.0f, wArray));
                            mapOptimizedWeights[m.Key][modalityComponent][L] = evalTarget.ToArray2D(wArray);

                            wArray.Dispose();
                            errorArray.Dispose();
                            deltaArray.Dispose();
                        }
                    }

                    distance2WinnerArray.Dispose();
                    evalTarget.Dispose();
                }
            }
            #endregion

        }

        #region GUI

        public System.Drawing.Bitmap GetVisualization(int layer)
        {
            //float time1 = Time.now();

            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(Width,Height);

            float[][,] scaled = (float[][,])activity.Clone();
            float max = float.NegativeInfinity;
            float min = float.PositiveInfinity;

            //Scale for visualization
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    scaled[layer][x, y] = (float)Math.Pow(activity[layer][x, y], 2.0);
                    min = Math.Min(scaled[layer][x, y], min);
                    max = Math.Max(scaled[layer][x, y], max);
                }
            }
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    scaled[layer][x, y] = (float)((scaled[layer][x, y] - min) / (max - min));
                }
            }

            //Build the bmp
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    System.Drawing.Color color = HelpersLib.ColorInterpolator.InterpolateBetween(Color.Blue, Color.Green, (float)scaled[layer][x, y]);

                    bmp.SetPixel(x, y, color);

                    if (x == lastWinX && y == lastWinY && layer == lastWinZ)
                    {
                        bmp.SetPixel(x, y, Color.Red);
                    }
                }
            }
            //float time2 = Time.now();
            //Console.WriteLine("Visualization computed in " + (time2 - time1).ToString());
            return bmp;
        }

        #endregion GUI

                #region Serialization
        //Deserialization constructor.

        //public CVZ_MMCM_Accelerated(SerializationInfo info, StreamingContext ctxt)
        //    :base(info,ctxt)
        //{
        //    //Get the values from info and assign them to the appropriate properties
        //    Height = (int)info.GetValue("Height", typeof(int));
        //    Width = (int)info.GetValue("Width", typeof(int));
        //    Layers = (int)info.GetValue("Layers", typeof(int));

        //    RebuildWeightsMatrix();

        //    int modalitiesCount = (int)info.GetValue("ModalitiesCount", typeof(int));
        //    for (int i = 0; i < modalitiesCount; i++)
        //    {
        //        string modalityName = (string)info.GetValue("ModalityName" + i, typeof(string));
        //        float[][][,] w = (float[][][,])info.GetValue("ModalityWeights" + i, typeof(float[][][,]));
        //        mapOptimizedWeights.Add(modalityName, w);
        //    }
        //}

        ////Serialization function.
        //public override void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        //{
        //    base.GetObjectData(info, ctxt);
        //    info.AddValue("Height", Height);
        //    info.AddValue("Width", Width);
        //    info.AddValue("Layers", Layers);
        //    int i = 0;
        //    foreach (KeyValuePair<string, float[][][,]> w in mapOptimizedWeights)
        //    {
        //        info.AddValue("ModalityWeights"+i,w.Value);
        //        i++;
        //    }
        //}
        #endregion Serialization
    }
}
