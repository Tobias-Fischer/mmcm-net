using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using nullpointer.Metaphone;

namespace MMCMLibrary.Modalities
{
    [Serializable]
    /// <summary>
    /// A simple class that wrap a BufferedPortBottle so it is used for strings
    /// </summary>
    public class YarpModalityString:IModality
    {
        string mapName;

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
        /// Basic constructor for a string modality. Size of this modality is forced to 4 to use the minimum size of a metaphone.
        /// </summary>
        /// <param name="name">Name of the modality. It will be used to open the corresponding
        /// yarp port</param>
        public YarpModalityString(string _mapName, string _name):
            base(_name, 4)
        {

            mapName = _mapName;
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
            Bottle b = portReal.read(false);

            if (b != null)
            {
                RealValue = stringToVector(b.get(0).asString().c_str());
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
            b.addString(vectorToString(PredictedValue));
            portPredicted.write();
        }

        /// <summary>
        /// Write the perceived value on the yarp port.
        /// </summary>
        public override void WritePerceivedValue()
        {
            Bottle b = portPerceived.prepare();
            b.clear();
            b.addString(vectorToString(PerceivedValue));
            portPerceived.write();
        }

        #region Metaphone handling
        List<char> possiblePhones = new List<char> { '0', 'B', 'F', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'R', 'S', 'T', 'W', 'X', 'Y', 'A', 'E', 'I', 'O', 'U' };
        private float phoneToFloat(char p)
        {
            return ((float)possiblePhones.IndexOf(p) / (float)possiblePhones.Count);
        }

        private char floatToPhone(float d)
        {
            double step = 1.0 / possiblePhones.Count;
            int index = (int)(d / step);
            double rest = d % step;
            if (rest < step / 2.0 || possiblePhones.Count == index)
                return possiblePhones[index];
            else
                return possiblePhones[index + 1];
        }

        private float[] stringToVector(string s)
        {
            ShortDoubleMetaphone metaphone = new ShortDoubleMetaphone(s);

            float[] v = new float[size];
            for (int i = 0; i < metaphone.PrimaryKey.Length; i++)
            {
                v[i] = phoneToFloat(metaphone.PrimaryKey[i]);
            }
            return v;
        }

        public string vectorToString(float[] v)
        {
            string s = "";
            for (int i = 0; i < v.Length; i++)
            {
                char c = floatToPhone(v[i]);
                s = s + c;
            }
            return s;
        }
        
        #endregion

        public override System.Windows.Forms.Control GetControl(bool useImageFormat = false)
        {
            if (useImageFormat)
            {
                ImageModalityCtrl ctrl = new ImageModalityCtrl();
                ctrl.LinkToModality(this);
                return ctrl;
            }
            else
            {
                StringModalityControl ctrl = new StringModalityControl();
                ctrl.LinkToModality(this);
                return ctrl;
            }
        }

        #region Serialization
        public YarpModalityString(SerializationInfo info, StreamingContext ctxt)
            :base(info,ctxt)
        {
            mapName = (string)info.GetValue("mapName", typeof(string));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            base.GetObjectData(info, ctxt);
            info.AddValue("mapName", mapName);
        }
        #endregion
    }
}
