using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using MMCMLibrary.Modalities;
using System.IO;


namespace MMCMLibrary
{

    [Serializable]
    public abstract class IConvergenceZone : ISerializable
    {
        /// <summary>
        /// The name of the CVZ. It will be used to identify it on the yarp network.
        /// </summary>
        public string name;

	   [NonSerialized]
	   private StreamWriter errorLog = null;
	   [NonSerialized]
	   private double starLoggingTime = -1.0;
	   /// <summary>
	   /// Start recording the error of each modality at each step.
	   /// Calling this function successively without calling stop will raise an exception.
	   /// </summary>
	   /// <param name="fileName"></param>
	   public void startLoggingError(string fileName)
	   {
		  if (errorLog == null)
		  {
			 errorLog = File.CreateText("C:/logMMCM/" + fileName);
			 errorLog.AutoFlush = true;
			 errorLog.Write("time\t");
			 foreach (KeyValuePair<string, IModality> k in modalities)
			 {
				errorLog.Write(k.Key + "\t");
			 }

			 if (hierarchicalModality != null)
			 {
				errorLog.Write("hierarchical \t");
			 }
			 errorLog.WriteLine();
		  }
		  else
			 throw new Exception("errorLog StreamWriter is already opened");
	   }

	   /// <summary>
	   /// Stop logging error and save the buffer to the HD.
	   /// </summary>
	   public void stopLoggingError()
	   {
		  if (errorLog != null)
		  {
			 errorLog.Close();
			 errorLog = null;
			 starLoggingTime = -1.0;
		  }
	   }

        /// <summary>
        /// A list of all the modalities.
        /// Since it is in a dictionary it can be accessed by name.
        /// </summary>
        public Dictionary<string, IModality> modalities;
        public Dictionary<string, float> modalitiesInfluence;
	   public YarpModalityVector hierarchicalModality;
        public bool HierarchicalModalitySet { get { return hierarchicalModality != null; } }
        public float feedbackInfluence;
        public int HierarchicalModalitySize
        {
            get
            {
                if (hierarchicalModality == null)
                    return 0;
                else
                    return hierarchicalModality.Size;
            }
        }

        /// <summary>
        /// To implement : return the size desired by the model as its FF/FB connection
        /// </summary>
        protected abstract int hierarchicalModalityDesiredSize();

        /// <summary>
        /// To implement : compute the value that will be sent to connected modality as its real value
        /// </summary>
        protected abstract float[] feedForward();

        /// <summary>
        /// Get the feedback incoming into the yarp port of prediction.
        /// It can be used to modulate the zone predictions.
        /// </summary>
        protected float[] FeedBack
        {
            get
            {
                return hierarchicalModality.GetRealValue;
            }
        }

        /// <summary>
        /// Set the hierarchical modality of this CVZ
        /// </summary>
        /// <param name="modalityName"></param>
        /// <param name="size"></param>
        public void CreateHierarchicalModality(int size, float influence, bool isSynchrone)
        {
            if (hierarchicalModalityDesiredSize() != size)
                throw new Exception("Incompatible size with this model. It should be " + hierarchicalModalityDesiredSize());

            if (HierarchicalModalitySet)
            {
                Console.WriteLine("IConvergenceZone.CreateHierarchicalModality(): Warning, modality already created.");
                return;
            }

            feedbackInfluence = influence;
		  hierarchicalModality = new YarpModalityVector(name, "hierarchical", size, null, null, isSynchrone);

		  if (errorLog != null)
		  {
			 stopLoggingError();
			 Console.WriteLine("AddHierarchicalModality: logging of error stopped");
		  }
        }

        /// <summary>
        /// Try to establish the yarp connections for maps running on the same computer.
        /// Connections are crossed
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public bool ConnectHierarchicalModality(YarpModalityVector target)
        {
            return (
            Network.connect(hierarchicalModality.portpredictedName, target.portRealName)
                &&
            Network.connect(target.portpredictedName, hierarchicalModality.portRealName));
        }

        /// <summary>
        /// Try to establish the yarp connections for maps running on the same computer.
        /// Connections are crossed
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public bool ConnectHierarchicalModality(string targetMapName, string targetModalityName)
        {
            return (
            Network.connect(hierarchicalModality.portpredictedName, YarpModalityVector.getRealPortName(targetMapName,targetModalityName))
                &&
            Network.connect(YarpModalityVector.getPredictedPortName(targetMapName, targetModalityName), hierarchicalModality.portRealName));
        }

        /// <summary>
        /// Add a modality to this CVZ
        /// </summary>
        /// <param name="modalityName"></param>
        /// <param name="size"></param>
        public virtual void AddModality(IModality m, float influence)
        {
            modalities.Add(m.name, m);
            modalitiesInfluence.Add(m.name, influence);
		  if (errorLog != null)
		  {
			 stopLoggingError();
			 Console.WriteLine("AddModality: logging of error stopped");
		  }
        }

        public virtual void RemoveModality(string modalityName)
        {
            modalities.Remove(modalityName);
		  modalitiesInfluence.Remove(modalityName); 
		  if (errorLog != null)
		  {
			 stopLoggingError();
			 Console.WriteLine("RemoveModality: logging of error stopped");
		  }
        }

        public IConvergenceZone(string _name)
        {
            modalities = new Dictionary<string, IModality>();
            modalitiesInfluence = new Dictionary<string, float>();
            name = _name;
        }

        public void Dispose()
        {
	       stopLoggingError();

            foreach (KeyValuePair<string, IModality> k in modalities)
            {
                k.Value.Dispose();
            }
        }

        public event EventHandler realityInputRefreshed;
        public event EventHandler perceivedInputRefreshed;
        public event EventHandler predictionOutputRefreshed;

        /// <summary>
        /// Refresh all the real inputs to the CVZ.
        /// </summary>
        private void RefreshRealValues()
        {
            foreach (KeyValuePair<string, IModality> k in modalities)
            {
                k.Value.ReadRealValue();
            }

            if (hierarchicalModality != null)
                hierarchicalModality.ReadRealValue();

            if (realityInputRefreshed != null)
                realityInputRefreshed(this, null);
        }


        /// <summary>
        /// To implement : Compute the predictions of the CVZ and update them
        /// into the modalities.
        /// </summary>
        protected virtual void ComputePredictedValues()
        {
            if (hierarchicalModality != null)
                hierarchicalModality.PredictedValue = feedForward();

            if (predictionOutputRefreshed != null)
                predictionOutputRefreshed(this, null);
        }

        /// <summary>
        /// Refresh all the perceived inputs.
        /// The enactionFactor represent how much the CVZ perception are modified by its predictions.
        /// </summary>
        private void ComputePerceivedValues(float enactionFactor)
        {
            foreach (KeyValuePair<string, IModality> k in modalities)
            {
                k.Value.EnactivePerception(enactionFactor);
            }
            if (hierarchicalModality != null)
                hierarchicalModality.EnactivePerception(enactionFactor);

            if (perceivedInputRefreshed != null)
                perceivedInputRefreshed(this, null);
        }

        /// <summary>
        /// Refresh all the perceived inputs.
        /// The enactionFactor represent how much the CVZ perception are modified by its predictions.
        /// </summary>
        /// <param name="enactionFactor">Specific enaction factor for each modality</param>
        private void ComputePerceivedValues(float[] enactionFactor)
        {
            int i = 0;
            foreach (KeyValuePair<string, IModality> k in modalities)
            {
                k.Value.EnactivePerception(enactionFactor[i]);
                i++;
            }

            if (perceivedInputRefreshed != null)
                perceivedInputRefreshed(this, null);
        }

        /// <summary>
        /// Refresh all the perceived inputs.
        /// EXPERIMENTAL :
        /// Automatically set the enactionFactor by calculating
        /// the error of each modality.
        /// </summary>
        private void ComputePerceivedValues()
        {
            int i = 0;
            foreach (KeyValuePair<string, IModality> k in modalities)
            {
                //Calculate the prediction error
                float[] predicted = k.Value.PredictedValue;
                float[] real = k.Value.GetRealValue;
                float distance = MathFunctions.EuclideanDistance(predicted, real);

                //Scale the distance to a number in [0-1] where
                // 0 = (predicted == real)
                // 1 = (predicted == "opposite" of real)
                distance = distance / predicted.Count();

                //TO DO : put a function (something like an inverse TANH )
                k.Value.EnactivePerception(1 - distance);
                i++;
            }

            if (perceivedInputRefreshed != null)
                perceivedInputRefreshed(this, null);
        }

        /// <summary>
        /// Do a complete step (refresh real values, compute perceived and predict)
        /// The enactionFactor represent how much the CVZ perception are modified by its predictions.
        public void Step(float enactionFactor)
	   {
		  double currentTime;
		  if (errorLog != null)
		  {
			 if (starLoggingTime == -1.0)
				starLoggingTime = currentTime = Time.now();
			 else
				currentTime = Time.now();
			 errorLog.Write(currentTime - starLoggingTime + "\t");
		  }
            RefreshRealValues();
            ComputePerceivedValues(enactionFactor);
            ComputePredictedValues();
            foreach (KeyValuePair<string, IModality> k in modalities)
            {
                k.Value.WritePredictedValue();
                k.Value.WriteErrorValue();
			 k.Value.WritePerceivedValue();
			 if (errorLog != null)
				errorLog.Write(k.Value.RealErrorMean() + "\t");
            }

            if (hierarchicalModality != null)
            {
                hierarchicalModality.WritePredictedValue();
                hierarchicalModality.WriteErrorValue();
			 hierarchicalModality.WritePerceivedValue();
			 if (errorLog != null)
				errorLog.Write(hierarchicalModality.RealErrorMean() + "\t");
		  }
		  if (errorLog != null)
			 errorLog.WriteLine();
        }

        /// <summary>
        /// Do a complete step (refresh real values, compute perceived and predict)
        /// The enactionFactor[i] represent for each modality how much the CVZ perception is modified by its predictions.
        public void Step(float[] enactionFactor)
	   {
		  double currentTime;
		  if (errorLog != null)
		  {
			 if (starLoggingTime == -1.0)
				starLoggingTime = currentTime = Time.now();
			 else
				currentTime = Time.now();
			 errorLog.Write(currentTime - starLoggingTime + "\t");
		  }
            RefreshRealValues();
            ComputePerceivedValues(enactionFactor);
            ComputePredictedValues();
            foreach (KeyValuePair<string, IModality> k in modalities)
            {
                k.Value.WritePredictedValue();
                k.Value.WriteErrorValue();
			 k.Value.WritePerceivedValue();
			 if (errorLog != null)
				errorLog.Write(k.Value.RealErrorMean() + "\t");
            
            }

            if (hierarchicalModality != null)
            {
                hierarchicalModality.WritePredictedValue();
                hierarchicalModality.WriteErrorValue();
			 hierarchicalModality.WritePerceivedValue();
			 if (errorLog != null)
				errorLog.Write(hierarchicalModality.RealErrorMean() + "\t");
   
            }

		  if (errorLog != null)
			 errorLog.WriteLine() ;
        }

        /// <summary>
        /// Do a complete step (refresh real values, compute perceived and predict)
        /// EXPERIMENTAL :
        /// Automatically set the enactionFactor by calculating
        /// the error of each modality.
        public void Step()
        {
		  double currentTime;
		  if (errorLog != null)
		  {
			 if (starLoggingTime == -1.0)
				starLoggingTime = currentTime = Time.now();
			 else
				currentTime = Time.now();
			 errorLog.Write(currentTime - starLoggingTime + "\t");
		  }
            RefreshRealValues();
            ComputePerceivedValues();
            ComputePredictedValues();
            foreach (KeyValuePair<string, IModality> k in modalities)
            {
                k.Value.WritePredictedValue();
                k.Value.WriteErrorValue();
                k.Value.WritePerceivedValue();

			 if (errorLog != null)
				errorLog.Write(k.Value.RealErrorMean()+"\t");
            }

            if (hierarchicalModality != null)
            {
                hierarchicalModality.WritePredictedValue();
                hierarchicalModality.WriteErrorValue();
			 hierarchicalModality.WritePerceivedValue();

			 if (errorLog != null)
				errorLog.Write(hierarchicalModality.RealErrorMean() + "\t");
		  }
		  if (errorLog != null)
			 errorLog.WriteLine();
        }


        #region Serialization
        //Deserialization constructor.
        public IConvergenceZone(SerializationInfo info, StreamingContext ctxt)
        {
            Network.init();
            modalities = new Dictionary<string, IModality>();
            modalitiesInfluence = new Dictionary<string, float>();

            //Get the values from info and assign them to the appropriate properties
            name = (String)info.GetValue("CVZName", typeof(String));
            int modalitiesCount = (int)info.GetValue("ModalitiesCount", typeof(int));
            for (int i = 0; i < modalitiesCount; i++)
            {            
                Type modalityType = (Type)info.GetValue("ModalityType" + i, typeof(Type));
                object uncastedMod = info.GetValue("Modality" + i, modalityType);
                IModality mod = null;
                if (modalityType == typeof(FileModality))
                    mod = (FileModality)uncastedMod;
                if (modalityType == typeof(YarpModalityImageFloat))
                    mod = (YarpModalityImageFloat)uncastedMod;
                if (modalityType == typeof(YarpModalityImageRgb))
                    mod = (YarpModalityImageRgb)uncastedMod;
                if (modalityType == typeof(RandomModality))
                    mod = (RandomModality)uncastedMod;
                if (modalityType == typeof(YarpModalityVector))
                    mod = (YarpModalityVector)uncastedMod;
                if (modalityType == typeof(YarpModalityString))
                    mod = (YarpModalityString)uncastedMod;

                if (mod == null)
                    throw new Exception("Type " + modalityType.ToString() + " is not handled by IConvergenceZone unserializer");

                mod.Initialise();
                AddModality(mod, 1.0f);
            }
        }

        //Serialization function.

        public virtual void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            //Get the values from info and assign them to the appropriate properties
            info.AddValue("CVZName", name);
            info.AddValue("ModalitiesCount", modalities.Count);
            int i = 0;
            foreach(KeyValuePair<string,IModality> m in modalities)
            {
                info.AddValue("ModalityType" + i, m.Value.GetType());
                info.AddValue("Modality" + i, m.Value);
                i++;
            }
        }
        #endregion Serialization
    }
}
