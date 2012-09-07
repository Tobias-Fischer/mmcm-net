using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization;

namespace MMCMLibrary.Modalities
{
    [Serializable()]
    /// <summary>
    /// A simple class that wrap a BufferedPortBottle so it is used for float[]
    /// </summary>
    public class RandomModality : IModality
    {
        [NonSerialized]
        System.Random rand;

        /// <summary>
        /// Basic constructor for a modality.
        /// </summary>
        /// <param name="name">Name of the modality. It will be used to open the corresponding
        /// file ( nameReal.txt )</param>
        /// <param name="size">Number of components of this modality</param>
        public RandomModality(string _mapName, string _name, int _size) :
            base(_name, _size)
        {
            Initialise();
        }

        public override void Initialise()
        {
            rand = new System.Random();
            base.Initialise();
        }

        public override void Dispose()
        {

        }

        /// <summary>
        /// Read the real value from the file.
        /// </summary>
        public override void ReadRealValue()
        {
            for (int i = 0; i < size; i++)
            {
                RealValue[i] = (float)rand.NextDouble();
            }
        }

        /// <summary>
        /// Write the predicted value to the file.
        /// </summary>
        public override void WritePredictedValue()
        {
        }

        /// <summary>
        /// Write the perceived value to the file.
        /// </summary>
        public override void WritePerceivedValue()
        {
        }

        /// <summary>
        /// Write the perceived value.
        /// </summary>
        public override void WriteErrorValue() 
        {
        }        
        
        public RandomModality(SerializationInfo info, StreamingContext ctxt)
            :base(info,ctxt)
        {
        }
    }
}
