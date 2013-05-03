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
    /// A simple class that wrap a BufferedPortBottle so it is used for float[]
    /// </summary>
    public class YarpModalityVector : IModality
    {
        private string autoconnect_source;
        string mapName;
        private float[] minBounds, maxBounds;
        private bool isBlockingRead;

        //Vectors defining the boundaries of the input spaces. Used to scale it in [0,1]

        [NonSerialized]
        private float[] forcedValue = null;
        [NonSerialized]
        private BufferedPortBottle portReal;
        [NonSerialized]
        private BufferedPortBottle portPredicted;
        [NonSerialized]
        private BufferedPortBottle portPerceived;
        public string portRealName { get { return portReal.getName().c_str(); } }
        public string portpredictedName { get { return portPredicted.getName().c_str(); } }
        public string portPerceivedName { get { return portPerceived.getName().c_str(); } }

        public static string getRealPortName(string mapName, string modalityName)
        {
            return "/" + mapName + "/" + modalityName + "/real:i";
        }

        public static string getPredictedPortName(string mapName, string modalityName)
        {
            return "/" + mapName + "/" + modalityName + "/predicted:o";
        }

        public static string getPerceivedPortName(string mapName, string modalityName)
        {
            return "/" + mapName + "/" + modalityName + "/perceived:o";
        }

        /// <summary>
        /// Basic constructor for a modality.
        /// </summary>
        /// <param name="name">Name of the modality. It will be used to open the corresponding
        /// yarp port</param>
        /// <param name="size">Number of components of this modality</param>
        public YarpModalityVector(string _mapName, string _name, int _size, float[] minBounds = null, float[] maxBounds = null, bool isBlocking = false, string _autoconnect_source = null) :
            base(_name, _size)
        {

            mapName = _mapName;
            isBlockingRead = isBlocking;
            if (minBounds != null && maxBounds != null)
            {
                this.minBounds = minBounds;
                this.maxBounds = maxBounds;
            }
            else
            {
                this.minBounds = new float[_size];
                this.maxBounds = new float[_size];
                for (int i = 0; i < _size; i++)
                {
                    this.minBounds[i] = 0;
                    this.maxBounds[i] = 1;
                }
            }
            autoconnect_source = _autoconnect_source;
            Initialise();
        }

        public override void Initialise()
        {
            portReal = new BufferedPortBottle();
            portPredicted = new BufferedPortBottle();
            portPerceived = new BufferedPortBottle();
            portReal.open("/" + mapName + "/" + name + "/real:i");
            portPredicted.open("/" + mapName + "/" + name + "/predicted:o");
            portPerceived.open("/" + mapName + "/" + name + "/perceived:o");
            if (autoconnect_source != null)
                Network.connect(autoconnect_source, YarpModalityVector.getRealPortName(mapName, name));
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
        /// </summary>
        public override void ReadRealValue()
        {
            if (forcedValue != null && forcedValue.Count() == size)
            {
                RealValue = forcedValue;
                forcedValue = null;
                return;
            }
            Bottle b = portReal.read(isBlockingRead);

            if (b != null)
            {
                int a = b.size();
                if (b.size() == size)
                {
                    for (int i = 0; i < size; i++)
                    {
                        RealValue[i] = (float)(b.get(i).asDouble() - minBounds[i]) / (maxBounds[i] - minBounds[i]);
                    }
                }
            }
        }

        /// <summary>
        /// Force the next read value
        /// </summary>
        public void ForceNextRealValue(float[] forcedValue)
        {
            if (forcedValue.Count() != size)
                throw new Exception("The size of the forced value should match the size of the modality");
            this.forcedValue = forcedValue;
        }

        /// <summary>
        /// Write the predicted value on the yarp port.
        /// </summary>
        public override void WritePredictedValue()
        {
            Bottle b = portPredicted.prepare();
            b.clear();

            for (int i = 0; i < size; i++)
            {
                b.addDouble((PredictedValue[i] * (maxBounds[i] - minBounds[i])) + minBounds[i]);
            }
            portPredicted.write();
        }

        /// <summary>
        /// Write the perceived value on the yarp port.
        /// </summary>
        public override void WritePerceivedValue()
        {
            Bottle b = portPerceived.prepare();
            b.clear();

            for (int i = 0; i < size; i++)
            {
                b.addDouble((PerceivedValue[i] * (maxBounds[i] - minBounds[i])) + minBounds[i]);
            }
            portPerceived.write();
        }

        public YarpModalityVector(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
        {
            mapName = (string)info.GetValue("mapName", typeof(string));
            minBounds = (float[])info.GetValue("minBounds", typeof(float[]));
            minBounds = (float[])info.GetValue("maxBounds", typeof(float[]));
            isBlockingRead = (bool)info.GetValue("isBlocking", typeof(bool));
            autoconnect_source = (string)info.GetValue("autoconnect", typeof(string)); isBlockingRead = (bool)info.GetValue("isBlocking", typeof(bool));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            base.GetObjectData(info, ctxt);
            info.AddValue("mapName", mapName);
            info.AddValue("minBounds", minBounds);
            info.AddValue("maxBounds", maxBounds);
            info.AddValue("isBlocking", isBlockingRead);
            info.AddValue("autoconnect", autoconnect_source);
        }
    }
}
