using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Drawing;
using System.Runtime.Serialization;

namespace MMCMLibrary.Modalities
{
    [Serializable]
    /// <summary>
    /// A simple class that wrap a BufferedPortBottle so it is used for float[]
    /// </summary>
    public class YarpModalitySound:IModality
    {
        private int bufferSize;
	   private string mapName;
	   private bool isBlockingRead;

        [NonSerialized] private BufferedPortSound portReal;
        [NonSerialized] private BufferedPortSound portPredicted;
        [NonSerialized] private BufferedPortSound portPerceived;
        public string portRealName { get { return portReal.getName().c_str(); } }
        public string portpredictedName { get { return portPredicted.getName().c_str(); } }
        public string portPerceivedName { get { return portPerceived.getName().c_str(); } }
        /// <summary>
        /// Basic constructor for a modality.
        /// </summary>
        /// <param name="name">Name of the modality. It will be used to open the corresponding
        /// yarp port</param>
        /// <param name="size">Number of components of this modality</param>
        public YarpModalitySound(string _mapName, string _name, int _bufferSize, bool isBlocking = false):
            base(_name, _bufferSize )
        {
		  mapName = _mapName;
		  isBlockingRead = isBlocking; 
            Initialise();
        }

        public override void Initialise()
        {
            portReal = new BufferedPortSound();
            portPredicted = new BufferedPortSound();
            portPerceived = new BufferedPortSound();
            portReal.open("/" + mapName + "/" + name + "/real:i");
            portPredicted.open("/" + mapName + "/" + name + "/predicted:o");
            portPerceived.open("/" + mapName + "/" + name + "/perceived:o");
            base.Initialise();
        }

        public override void Dispose()
        {
            portReal.close();
            portReal.Dispose();
            portPredicted.close();
            portPredicted.Dispose();
            portPerceived.close();
            portPerceived.Dispose();

            base.Dispose();
        }

        /// <summary>
        /// Read the real value on the yarp port.
        /// Convert the image into a vector.
        /// </summary>
        public override void ReadRealValue()
        {
		  Sound s = portReal.read(isBlockingRead);

            if (s != null )
            {
                RealValue = Sound2Vector(s);
            }
        }

        /// <summary>
        /// Write the predicted value on the yarp port.
        /// </summary>
        public override void WritePredictedValue()
        {
            Sound snd = portPredicted.prepare();
            Vector2Sound(PredictedValue,ref snd);
            portPredicted.write();
        }

        /// <summary>
        /// Write the perceived value on the yarp port.
        /// </summary>
        public override void WritePerceivedValue()
        {
		  Sound snd = portPredicted.prepare();
		  Vector2Sound(PerceivedValue, ref snd);
		  portPredicted.write();
        }

        /// <summary>
        /// Self explicit
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        protected float[] Sound2Vector(Sound snd)
        {
            float[] v = new float[size];
		  
            for (int i = 0; i < snd.getSamples(); i++)
            {
			 v[i] = snd.getSafe(i) / 255.0f;
            }
            return v;
        }

        public void Vector2Sound(float[] rawValues, ref Sound snd)
        {
            for (int i = 0; i < rawValues.Length; i++)
            {
			 snd.setSafe(i,(int)rawValues[i]*255);
            }
        }

        public YarpModalitySound(SerializationInfo info, StreamingContext ctxt)
            :base(info,ctxt)
        {
		  mapName = (string)info.GetValue("mapName", typeof(string));
		  isBlockingRead = (bool)info.GetValue("isBlocking", typeof(bool));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            base.GetObjectData(info, ctxt);
		  info.AddValue("mapName", mapName);
		  info.AddValue("isBlocking", isBlockingRead);
        }
    }
}
