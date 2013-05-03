using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace CVZ_Core.Modalities
{
    [Serializable]
    /// <summary>
    /// An abstract class that implement the interface of a modality
    /// </summary>
    public class IModality
    {
        #region Properties

        public string name;
        protected int size;

        /// <summary>
        /// The signal perceived (mix of reality + prediction)
        /// </summary>
        private float[] perceivedValue;

        /// <summary>
        /// The real value, sent by the world or by any sensor
        /// </summary>
        private float[] realValue;

        /// <summary>
        /// The predicted value, obtained by the convergence zone
        /// </summary>
        private float[] predictedValue;

        /// <summary>
        /// Read only access to the perceived value
        /// </summary>
        public float[] PerceivedValue
        {
            get
            {
                return perceivedValue;
            }
        }

        /// <summary>
        /// Read only access to the size
        /// </summary>
        public int Size
        {
            get
            {
                return size;
            }
        }
        /// <summary>
        /// Read/write only access for the real value
        /// </summary>
        protected float[] RealValue
        {
            get
            {
                return realValue;
            }
            set
            {
                realValue = value;
            }
        }

	   /// <summary>
	   /// Calculation of current real error vector (real - predicted)
	   /// </summary>
	   public float[] RealErrorValue()
	   {
		  float[] error = new float[size];
		  for (int i = 0; i < size; i++)
			 error[i] = Math.Abs(realValue[i] - predictedValue[i]);
		  return error;
	   }

	   /// <summary>
	   /// Calculation of current perceived error vector (perceived - predicted)
	   /// </summary>
	   public float[] PerceivedErrorValue()
	   {
		  float[] error = new float[size];
		  for (int i = 0; i < size; i++)
			 error[i] = Math.Abs(perceivedValue[i] - predictedValue[i]);
		  return error;
	   }
	   
	   /// <summary>
	   /// Calculation of current mean of perceived error (perceived - predicted)/size
	   /// </summary>
	   public float PerceivedErrorMean()
	   {
		  float error = 0.0f;
		  for (int i = 0; i < size; i++)
			 error += Math.Abs(perceivedValue[i] - predictedValue[i]);
		  return error/(float)size;
	   }
	   
	   /// <summary>
	   /// Calculation of current mean of real error (real - predicted)/size
	   /// </summary>
	   public float RealErrorMean()
	   {
		  float error = 0.0f;
		  for (int i = 0; i < size; i++)
			 error += Math.Abs(realValue[i] - predictedValue[i]);
		  return error / (float)size;
	   }

        /// <summary>
        /// Read/Write access for the predicted value
        /// </summary>
        public float[] PredictedValue
        {
            get
            {
                return predictedValue;
            }

            set
            {
                predictedValue = value;
            }
        }

        /// <summary>
        /// Read only access for the real value
        /// </summary>
        public float[] GetRealValue
        {
            get
            {
                return realValue;
            }
        }

	   //Deprecated, added functions to deal with different kinds of errors
	   ///// <summary>
	   ///// Read only access for the prediction error
	   ///// </summary>
	   //public float GetPredictionError
	   //{
	   //    get
	   //    {
	   //        float error = 0.0f;
	   //        for (int i = 0; i < size; i++)
	   //            error += Math.Abs(realValue[i] - predictedValue[i]);
	   //        error /= size;
	   //        return error;
	   //    }
	   //}
        #endregion Properties

        /// <summary>
        /// Basic constructor for a modality.
        /// </summary>
        /// <param name="name">Name of the modality. It will be used to open the corresponding
        /// <param name="size">Number of components of this modality</param>
        protected IModality(string _name, int _size)
        {
            name = _name;
            size = _size;
            realValue = new float[size];
            predictedValue = new float[size];
            perceivedValue = new float[size];
        }

        protected IModality() { }

        public virtual void Initialise()
        {

        }

        public virtual void Dispose()
        {

        }

        /// <summary>
        /// Read the real value.
        /// </summary>
        public virtual void ReadRealValue() { }

        /// <summary>
        /// Write the predicted value.
        /// </summary>
        public virtual void WritePredictedValue() { }

        /// <summary>
        /// Write the perceived value.
        /// </summary>
        public virtual void WritePerceivedValue() { }

        /// <summary>
        /// Write the error value.
        /// </summary>
        public virtual void WriteErrorValue() { }

        /// <summary>
        /// Refresh the perceived value by mixing real and predicted values.
        /// </summary>
        /// <param name="enactionFactor"> 
        /// Value in [0-1] that defines 
        /// the ration between reality and prediction
        /// </param>
        public void EnactivePerception(float enactionFactor)
        {
            for (int i = 0; i < size; i++)
            {
                perceivedValue[i] = (1 - enactionFactor) * realValue[i] + enactionFactor * predictedValue[i];
            }
        }

	   public virtual System.Drawing.Bitmap GetAsBmp(float[] value)
	   {
		  return new System.Drawing.Bitmap(0, 0);
	   }

        public IModality(SerializationInfo info, StreamingContext ctxt)
        {
            name = (string)info.GetValue("ModName", typeof(string));
            size = (int)info.GetValue("Size", typeof(int));
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("ModName", name);
            info.AddValue("Size", size);
        }
    }
}
