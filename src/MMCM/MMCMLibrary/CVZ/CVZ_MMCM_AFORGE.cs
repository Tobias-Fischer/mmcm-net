using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using AForge;
using AForge.Neuro;
using AForge.Neuro.Learning;

namespace MMCMLibrary
{
    //MMCM_AFORGE
    //There is one SOM for each modality
    //All the SOMs act as input to a meta-SOM (winner of each SOM is one component of the metaSOM input vector)
    //At each step the metaSOM predict the winner of each SOM.
    //This predicted winner is use to predict the modalities values
    //
    //todo: now all the SOMs have the same influence for the metaSOM. I have to find a way to use the influence...
    //todo: the metaSOM doesn't perceive in an enactive way. It's not mandatory, but it may destroy the temporal sensitivity of the architecture

    public class CVZ_MMCM_AFORGE:IConvergenceZone
    {
        #region Properties
        private int height = 0, width = 0;

        DistanceNetwork metaMap = null;
        SOMLearning metaTrainer = null;
        private Dictionary<string, DistanceNetwork> maps;
        private Dictionary<string, SOMLearning> trainers;

        private System.Random rand = new System.Random();
        private float learningRate = 0.5f;

        /// <summary>
        /// Learning rate in  [0-1].
        /// Use negative value for no learning
        /// </summary>
        public float LearningRate
        {
            get { return learningRate; }
            set { learningRate = value; }
        }

        private float radius = 15;
        /// <summary>
        /// Neighborhood size. 
        /// </summary>
        public float Radius
        {
            get { return radius; }
            set { if (value > 0) radius = value; else throw new Exception("Neighborhood can't be negative"); }
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
        public CVZ_MMCM_AFORGE(string name, int _height, int _width)
            : base(name)
        {
            height = _height;
            width = _width;
            maps = new Dictionary<string, DistanceNetwork>();
            trainers = new Dictionary<string, SOMLearning>();
        }

        /// <summary>
        /// Connect a modality
        /// </summary>
        /// <param name="modalityName">Modality name</param>
        /// <param name="size">Modality vector size</param>
        /// <param name="influence">Modality influence on the map</param>
        public override void AddModality(IModality m, float influence)
        {
            //Null the meta map, it has to be reinitialized
            metaMap = null;
            metaTrainer = null;

            //Create the network and the trainer
            maps.Add(m.name, new DistanceNetwork(m.Size, height * width));
            trainers.Add(m.name, new SOMLearning(maps[m.name]));

            // Randomize weights of network
			Neuron.RandRange = new Range( 0, 1 );

			// randomize net
			maps[m.name].Randomize( );
            
            base.AddModality(m, influence);
        }

        /// <summary>
        /// Remove a modality
        /// </summary>
        /// <param name="modalityName"></param>
        public override void RemoveModality(string modalityName)
        {
            //Null the meta map, it has to be reinitialized
            metaMap = null;
            metaTrainer = null;

            maps.Remove(modalityName);
            base.RemoveModality(modalityName);
        }

        public void InitialiseMetaMap(int metaHeight, int metaWidth)
        {
            metaMap = new DistanceNetwork(maps.Count, metaHeight * metaWidth);
            metaTrainer = new SOMLearning(metaMap);
            // Randomize weights of network
            Neuron.RandRange = new Range(0, height*width);
            metaMap.Randomize( );
        }

        static double[] convertFloatsToDoubles(float[] input)
        {
            if (input == null)
            {
                return null; // Or throw an exception - your choice
            }
            double[] output = new double[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                output[i] = input[i];
            }
            return output;
        }


        /// <summary>
        /// Calculate and update the predicted values for all modalities
        /// that are connected to the map.
        /// </summary>
        protected override void ComputePredictedValues()
        {
            if (metaMap == null || metaTrainer == null)
            {
                throw new Exception("The meta map, or meta trainer is not initialized. Call InitialiseMetaMap() first.");
            }

            //Record the winners vector
            float[] winners = new float[maps.Count];
            float[] predWinners = new float[maps.Count];

            //Activate and train all the modalities map
            int modIndex = 0;
            foreach(String modName in modalities.Keys)
            {
                if (learningRate>0)
                {
                    trainers[modName].LearningRate = learningRate;
                    trainers[modName].LearningRadius = radius;
                    trainers[modName].Run(convertFloatsToDoubles(modalities[modName].GetRealValue));
                }

                maps[modName].Compute(convertFloatsToDoubles(modalities[modName].GetRealValue));
                winners[modIndex] = maps[modName].GetWinner();
                modIndex++;
            }

            //Activate and train the metaMap
            if (learningRate > 0)
            {
                metaTrainer.LearningRate = learningRate;
                metaTrainer.LearningRadius = radius;
                metaTrainer.Run(convertFloatsToDoubles(winners));
            }

            metaMap.Compute(convertFloatsToDoubles(winners));
            int metaWinner = metaMap.GetWinner();
            Neuron metaWinnerNeuron = metaMap.Layers[0].Neurons[metaWinner];

            //Predict the winners values
            for (int i = 0; i < metaMap.InputsCount; i++)
            {
                predWinners[i] = (float)metaWinnerNeuron.Weights[i];
            }

            //Predicts modalities values
            modIndex = 0;
            foreach (String modName in modalities.Keys)
            {
                Neuron winnerNeuron = maps[modName].Layers[0].Neurons[(int)predWinners[modIndex]];
                for (int i = 0; i < modalities[modName].Size; i++)
                {
                    modalities[modName].PredictedValue[i] = (float)winnerNeuron.Weights[i];
                }
                modIndex++;
            }
            base.ComputePredictedValues();
        }

    }
}
