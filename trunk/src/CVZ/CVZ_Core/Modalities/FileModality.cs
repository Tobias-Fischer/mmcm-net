using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization;

namespace CVZ_Core.Modalities
{
    [Serializable]
    /// <summary>
    /// A simple class that wrap a BufferedPortBottle so it is used for float[]
    /// </summary>
    public class FileModality:IModality
    {
        [NonSerialized]
        private StreamReader fileReal;
        private StreamWriter filePredicted;
        private StreamWriter filePerceived;
        private StreamWriter fileError;

        /// <summary>
        /// Basic constructor for a modality.
        /// </summary>
        /// <param name="name">Name of the modality. It will be used to open the corresponding
        /// file ( nameReal.txt )</param>
        /// <param name="size">Number of components of this modality</param>
        public FileModality(string _mapName, string _name, int _size):
            base(_name, _size)
        {
            Initialise();
        }

        public override void Initialise()
        {
            fileReal = File.OpenText(name + "Real.txt");
            filePredicted = File.CreateText(name + "Predicted.txt");
            filePerceived = File.CreateText(name + "Perceived.txt");
            fileError = File.CreateText(name + "Error.txt");
            base.Initialise();
        }

        public override void Dispose()
        {
            fileReal.Close();
            filePredicted.Close();
            filePerceived.Close();
            fileError.Close();
        }

        /// <summary>
        /// Read the real value from the file.
        /// </summary>
        public override void ReadRealValue()
        {
            if (fileReal.EndOfStream)
            {
                fileReal.BaseStream.Seek(0, SeekOrigin.Begin);
                fileReal.DiscardBufferedData();
                Console.WriteLine("End of file reached.");
            }
            string line = fileReal.ReadLine();
            string[] values = line.Split(' ');

            for (int i = 0; i < size; i++)
            {
                RealValue[i] = (float)Convert.ToDouble( values[i] );
            }
        }

        /// <summary>
        /// Write the predicted value to the file.
        /// </summary>
        public override void WritePredictedValue()
        {
            string values = "";

            for (int i = 0; i < size; i++)
            {
                values += PredictedValue[i].ToString() + '\t';
            }
            filePredicted.WriteLine(values);
        }

        /// <summary>
        /// Write the perceived value to the file.
        /// </summary>
        public override void WritePerceivedValue()
        {
            string values = "";

            for (int i = 0; i < size; i++)
            {
                values += PerceivedValue[i].ToString() + '\t';
            }
            filePerceived.WriteLine(values);
        }

        /// <summary>
        /// Write the perceived value.
        /// </summary>
        public override void WriteErrorValue() 
        {
            string values = "Real : \t";
            for (int i = 0; i < size; i++)
            {
                values += RealValue[i].ToString() + '\t';
            }

            fileError.WriteLine(values);

            values = "Pred : \t";
            for (int i = 0; i < size; i++)
            {
                values += PredictedValue[i].ToString() + '\t';
            }
            fileError.WriteLine(values);

            values = "Err : \t";
            float meanError = 0.0f;
            for (int i = 0; i < size; i++)
            {
                float error = Math.Abs(RealValue[i] - PredictedValue[i]);
                meanError += error;
                values += error.ToString() + '\t';
            }
            values +=") Mean = " + meanError / size;
            fileError.WriteLine(values);
        }

        public FileModality(SerializationInfo info, StreamingContext ctxt)
            :base(info,ctxt)
        {
            fileReal = File.OpenText(name + "Real.txt");
            filePredicted = File.CreateText(name + "Predicted.txt");
            filePerceived = File.CreateText(name + "Perceived.txt");
            fileError = File.CreateText(name + "Error.txt");
        }
    }
}
