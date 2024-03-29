﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Drawing;
using System.Runtime.Serialization;
using CVZ_Core;

namespace CVZ_Core.Modalities
{
    [Serializable]
    /// <summary>
    /// A simple class that wrap a BufferedPortBottle so it is used for float[]
    /// </summary>
    public class YarpModalityImageFloat : IModality
    {
        private string autoconnect_source;
        private int w, h, padding;
        private string mapName;
        private bool isBlockingRead;

        [NonSerialized]
        private BufferedPortImageFloat portReal;
        [NonSerialized]
        private BufferedPortImageFloat portPredicted;
        [NonSerialized]
        private BufferedPortImageFloat portPerceived;
        public string portRealName { get { return portReal.getName().c_str(); } }
        public string portpredictedName { get { return portPredicted.getName().c_str(); } }
        public string portPerceivedName { get { return portPerceived.getName().c_str(); } }
        /// <summary>
        /// Basic constructor for a modality.
        /// </summary>
        /// <param name="name">Name of the modality. It will be used to open the corresponding
        /// yarp port</param>
        /// <param name="size">Number of components of this modality</param>
        public YarpModalityImageFloat(string _mapName, string _name, int _w, int _h, int _padding = 0, bool isBlocking = false, string _autoconnect_source = null) :
            base(_name, _w * _h)
        {
            w = _w;
            h = _h;
            padding = _padding;
            mapName = _mapName;
            isBlockingRead = isBlocking;
            autoconnect_source = _autoconnect_source;
            Initialise();
        }

        public override void Initialise()
        {
            portReal = new BufferedPortImageFloat();
            portPredicted = new BufferedPortImageFloat();
            portPerceived = new BufferedPortImageFloat();
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
        /// Convert the image into a vector.
        /// </summary>
        public override void ReadRealValue()
        {
            ImageFloat img = portReal.read(isBlockingRead);

            if (img != null)
            {
                RealValue = ImageManipulation.Image2Vector(Crop(img),w,h);
            }
        }

        /// <summary>
        /// Write the predicted value on the yarp port.
        /// </summary>
        public override void WritePredictedValue()
        {
            ImageFloat img = portPredicted.prepare();
		  ImageManipulation.Vector2Image(PredictedValue, ref img, w, h);
            portPredicted.write();
        }

        /// <summary>
        /// Write the perceived value on the yarp port.
        /// </summary>
        public override void WritePerceivedValue()
        {
            ImageFloat img = portPerceived.prepare();
		  ImageManipulation.Vector2Image(PerceivedValue, ref img, w,h);
            portPerceived.write();
        }

        protected ImageFloat Crop(ImageFloat img)
        {
            ImageFloat img2 = new ImageFloat();
            img2.resize(img.width() - 2 * padding, img.height() - 2 * padding);
            for (int x = padding; x < img.width() - padding; x++)
            {
                for (int y = padding; y < img.height() - padding; y++)
                {
                    img2.setPixel(x - padding, y - padding, img.getPixel(x, y));
                }
            }
            return img2;
        }


        public override System.Drawing.Bitmap GetAsBmp(float[] rawValues)
        {
            ImageFloat img = new ImageFloat();
		    ImageManipulation.Vector2Image(rawValues, ref img,w, h);
            return img.toBmp();
        }

        public YarpModalityImageFloat(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
        {
            mapName = (string)info.GetValue("mapName", typeof(string));
            w = (int)info.GetValue("w", typeof(int));
            h = (int)info.GetValue("h", typeof(int));
            padding = (int)info.GetValue("padding", typeof(int));
            isBlockingRead = (bool)info.GetValue("isBlocking", typeof(bool));
            autoconnect_source = (string)info.GetValue("autoconnect", typeof(string));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            base.GetObjectData(info, ctxt);
            info.AddValue("mapName", mapName);
            info.AddValue("w", w);
            info.AddValue("h", h);
            info.AddValue("padding", padding);
            info.AddValue("isBlocking", isBlockingRead);
            info.AddValue("autoconnect", autoconnect_source);
        }
    }
}
