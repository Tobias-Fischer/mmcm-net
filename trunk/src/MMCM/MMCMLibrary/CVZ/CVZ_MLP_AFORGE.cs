using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using AForge;
using AForge.Neuro;
using AForge.Neuro.Learning;

namespace MMCMLibrary
{
    // MLP_AFORGE
    // Uses a simple MLP + back-propagation trained in an auto-associative way
    // The input and the output layers contain all the modalities
    //
    // todo: now all the modalities have the same influence. If the input are binary we can we can set the influence effect to scale them on [0-0,5] or [0,5-1]
    // because a 0,5 input won't have influence. However, it's not possible if inputs are continuous on [0-1]. Random noise could be a solution...
    // todo: the enactive perception can be implemented either by the enactionFactor and reflection or by using a temporal network (build a new class).
    // Or maybe even by using both...

    public class CVZ_MLP_AFORGE:IConvergenceZone
    {
        #region Properties

        private float learningRate = 0.1f;
        private float momentum = 0.1f;
        private float sigmoidAlphaValue = 2.0f;

        ActivationNetwork network;
        BackPropagationLearning teacher;
        /// <summary>
        /// Store the training set of the network. It's built online.
        /// </summary>
        List<float[]> memoryBuffer;
        int maxMemoryBufferSize;

        private System.Random rand = new System.Random();

        /// <summary>
        /// Learning rate in  [0-1].
        /// Use negative value for no learning
        /// </summary>
        public float LearningRate
        {
            get { return learningRate; }
            set { learningRate = value; }
        }

        #endregion Properties

        protected override float[] feedForward()
        {
            throw new NotImplementedException();
        }

        protected override int hierarchicalModalityDesiredSize()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Basic constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="_height"></param>
        /// <param name="_width"></param>
        public CVZ_MLP_AFORGE(string name)
            : base(name)
        {
        }

        /// <summary>
        /// Connect a modality
        /// </summary>
        /// <param name="modalityName">Modality name</param>
        /// <param name="size">Modality vector size</param>
        /// <param name="influence">Modality influence on the map</param>
        public override void AddModality(IModality m, float influence)
        {
            network = null;
            teacher = null;
            base.AddModality(m, influence);
        }

        /// <summary>
        /// Remove a modality
        /// </summary>
        /// <param name="modalityName"></param>
        public override void RemoveModality(string modalityName)
        {
            network = null;
            teacher = null;
            base.RemoveModality(modalityName);
        }

        public void InitialiseNetwork(int[] layersSize, int memoryBufferSize)
        {
            int sumInputs = 0;
            foreach(IModality m in modalities.Values)
            {
                sumInputs += m.Size;
            }
            List<int> listLayer = layersSize.ToList();
            listLayer.Add(sumInputs);

            network = new ActivationNetwork(
                    (IActivationFunction)new SigmoidFunction(sigmoidAlphaValue),
                sumInputs, listLayer.ToArray());

            teacher = new BackPropagationLearning(network);

            // set learning rate and momentum
            teacher.LearningRate = learningRate;
            teacher.Momentum = momentum;

            // Prepare the memory buffer
            maxMemoryBufferSize = memoryBufferSize;
            memoryBuffer = new List<float[]>();
        }

        /// <summary>
        /// Calculate and update the predicted values for all modalities
        /// that are connected to the map.
        /// </summary>
        protected override void ComputePredictedValues()
        {
            if (network == null || teacher == null)
            {
                throw new Exception("The network is not initialized. Call Initialize first.");
            }
            
            List<float> inputOutput = new List<float>();
            foreach (IModality m in modalities.Values)
            {
                inputOutput.AddRange(m.GetRealValue);
            }
            if (LearningRate > 0)
            {
                RefreshMemoryBuffer(inputOutput.ToArray());
                float error = (float)teacher.RunEpoch(memoryBuffer.ToArray(), memoryBuffer.ToArray());
            }

            float[] totalPredict = network.Compute(inputOutput.ToArray());
            int component = 0;
            foreach (IModality m in modalities.Values)
            {
                for(int componentModality = 0; componentModality < m.Size ; componentModality++)
                {
                    m.PredictedValue[componentModality] = totalPredict[component];
                    component++;
                }
            }

            base.ComputePredictedValues();
        }

        /// <summary>
        /// Simply store the most recent events
        /// todo : remove a random example from the memory instead of the first one
        /// todo : remove the "best known" example from the memory ?
        /// </summary>
        /// <param name="newExample"></param>
        private void RefreshMemoryBuffer(float[] newExample)
        {
            if (memoryBuffer.Count == maxMemoryBufferSize)
                memoryBuffer.RemoveAt(0);
            memoryBuffer.Add(newExample);  
        }
    }
}
