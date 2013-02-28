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
    public class YarpModalityImageFloat:IModality
    {
        private int w, h, padding;
        private string mapName;
	   private bool isBlockingRead;

        [NonSerialized] private BufferedPortImageFloat portReal;
        [NonSerialized] private BufferedPortImageFloat portPredicted;
        [NonSerialized] private BufferedPortImageFloat portPerceived;
        public string portRealName { get { return portReal.getName().c_str(); } }
        public string portpredictedName { get { return portPredicted.getName().c_str(); } }
        public string portPerceivedName { get { return portPerceived.getName().c_str(); } }
        /// <summary>
        /// Basic constructor for a modality.
        /// </summary>
        /// <param name="name">Name of the modality. It will be used to open the corresponding
        /// yarp port</param>
        /// <param name="size">Number of components of this modality</param>
        public YarpModalityImageFloat(string _mapName, string _name, int _w, int _h, int _padding = 0, bool isBlocking = false):
            base(_name, _w * _h )
        {
            w = _w;
            h = _h;
            padding = _padding;
            mapName = _mapName;
		  isBlockingRead = isBlocking;
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

            if (img != null )
            {
                RealValue = Image2Vector(Crop(img));
            }
        }

        /// <summary>
        /// Write the predicted value on the yarp port.
        /// </summary>
        public override void WritePredictedValue()
        {
            ImageFloat img = portPredicted.prepare();
            Vector2Image(PredictedValue, ref img);
            portPredicted.write();
        }

        /// <summary>
        /// Write the perceived value on the yarp port.
        /// </summary>
        public override void WritePerceivedValue()
        {
            ImageFloat img = portPerceived.prepare();
            Vector2Image(PerceivedValue, ref img);
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

        /// <summary>
        /// Self explicit
        /// </summary>
        /// <param name="v"></param>
        /// <param name="img"></param>
        protected void Vector2Image(float[] v, ref ImageFloat img)
        {
            double t0 = Time.now();
            img.resize(w, h);
            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    img.setPixel(i, j, (float)v[j * w + i] * 255.0f );
                }
            }
            double t1 = Time.now();

            //Console.WriteLine("Vector2img : " + (t1 - t0).ToString());
        }

        /// <summary>
        /// Self explicit
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        protected float[] Image2Vector(ImageFloat img)
        {
            double t0 = Time.now();

            ImageFloat img2 = new ImageFloat();
            img2.copy(img, w, h);

            float[] v = new float[size];

            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    float px = img2.getPixel(i, j) /255.0f;
                    v[j * w + i] = px;
                }
            }
            double t1 = Time.now();
            //Console.WriteLine("Image2Vector : " + (t1 - t0).ToString());

            return v;
        }

        public override System.Drawing.Bitmap getAsBmp(float[] rawValues)
        {
            ImageFloat img = new ImageFloat();
            Vector2Image(rawValues, ref img);
            return HelpersLib.ImageManipulation.toBmp(img);
        }

        public YarpModalityImageFloat(SerializationInfo info, StreamingContext ctxt)
            :base(info,ctxt)
        {
            mapName = (string)info.GetValue("mapName", typeof(string));
            w = (int)info.GetValue("w", typeof(int));
            h = (int)info.GetValue("h", typeof(int));
            padding = (int)info.GetValue("padding", typeof(int));
		  isBlockingRead = (bool)info.GetValue("isBlocking", typeof(bool));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            base.GetObjectData(info, ctxt);
            info.AddValue("mapName", mapName);
            info.AddValue("w", w);
            info.AddValue("h", h);
		  info.AddValue("padding", padding);
		  info.AddValue("isBlocking", isBlockingRead);
        }
    }
}
