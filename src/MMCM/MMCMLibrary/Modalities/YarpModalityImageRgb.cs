using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace MMCMLibrary.Modalities
{
    [Serializable]
    /// <summary>
    /// A simple class that wrap a BufferedPortBottle so it is used for float[]
    /// </summary>
    public class YarpModalityImageRgb:IModality
    {
        private int w, h;
        private string mapName;
	   private bool isBlockingRead;

        [NonSerialized]
        private BufferedPortImageRgb portReal;
        [NonSerialized]
        private BufferedPortImageRgb portPredicted;
        [NonSerialized]
        private BufferedPortImageRgb portPerceived;
        public string portRealName { get { return portReal.getName().c_str(); } }
        public string portpredictedName { get { return portPredicted.getName().c_str(); } }
        public string portPerceivedName { get { return portPerceived.getName().c_str(); } }

        /// <summary>
        /// Basic constructor for a modality.
        /// </summary>
        /// <param name="name">Name of the modality. It will be used to open the corresponding
        /// yarp port</param>
        /// <param name="size">Number of components of this modality</param>
        public YarpModalityImageRgb(string _mapName, string _name, int _w, int _h, bool isBlocking = false):
            base(_name, _w * _h * 3)
        {
            w = _w;
            h = _h;
		  mapName = _mapName;
		  isBlockingRead = isBlocking;
            Initialise();
        }

        public override void Initialise()
        {
            portReal = new BufferedPortImageRgb();
            portPredicted = new BufferedPortImageRgb();
            portPerceived = new BufferedPortImageRgb();
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
            ImageRgb img = portReal.read(isBlockingRead);

            if (img != null )
            {
                RealValue = Image2Vector(img);
            }
        }

        /// <summary>
        /// Write the predicted value on the yarp port.
        /// </summary>
        public override void WritePredictedValue()
        {
            ImageRgb img = portPredicted.prepare();
            Vector2Image(PredictedValue, ref img);
            portPredicted.write();
        }

        /// <summary>
        /// Write the perceived value on the yarp port.
        /// </summary>
        public override void WritePerceivedValue()
        {
            ImageRgb img = portPerceived.prepare();
            Vector2Image(PerceivedValue, ref img);
            portPerceived.write();
        }

        /// <summary>
        /// Self explicit
        /// </summary>
        /// <param name="v"></param>
        /// <param name="img"></param>
        protected void Vector2Image(float[] v, ref ImageRgb img)
        {
            double t0 = Time.now();
            img.resize(w, h);
            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    img.pixel(i, j).r = (byte)(v[j * w + i] * 255);
                    img.pixel(i, j).g = (byte)(v[j * w + i + 1] * 255);
                    img.pixel(i, j).b = (byte)(v[j * w + i + 2] * 255);
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
        protected float[] Image2Vector(ImageRgb img)
        {
            double t0 = Time.now();

            ImageRgb img2 = new ImageRgb();
            img2.copy(img, w, h);

            float[] v = new float[size];

            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    PixelRgb px = img2.pixel(i, j);
                    v[j * w + i] = px.r / 255.0f; //Scale in [0,1]
                    v[j * w + i + 1] = px.g / 255.0f; //Scale in [0,1]
                    v[j * w + i + 2] = px.b / 255.0f; //Scale in [0,1]
                }
            }
            double t1 = Time.now();
            //Console.WriteLine("Image2Vector : " + (t1 - t0).ToString());

            return v;
        }

        public override System.Drawing.Bitmap getAsBmp(float[] rawValues)
        {
            ImageRgb img = new ImageRgb();
            Vector2Image(rawValues, ref img);
            return HelpersLib.ImageManipulation.toBmp(img);
        }        
        
        public YarpModalityImageRgb(SerializationInfo info, StreamingContext ctxt)
            :base(info,ctxt)
        {
            mapName = (string)info.GetValue("mapName", typeof(string));
            w = (int)info.GetValue("w", typeof(int));
		  h = (int)info.GetValue("h", typeof(int));
		  isBlockingRead = (bool)info.GetValue("isBlocking", typeof(bool));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            base.GetObjectData(info, ctxt);
            info.AddValue("mapName", mapName);
            info.AddValue("w", w);
		  info.AddValue("h", h);
		  info.AddValue("isBlocking", isBlockingRead);
        }
    }
}
