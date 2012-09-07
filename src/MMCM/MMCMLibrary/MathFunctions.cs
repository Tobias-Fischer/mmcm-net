using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMCMLibrary
{
    class MathFunctions
    {
        public static float EuclideanDistance(float[] a, float[] b)
        {
            float value = 0;
            for (int i = 0; i < a.Count() && i < b.Count(); i++)
            {
                value += (float)Math.Pow(a[i] - b[i], 2.0);
            }
            value = (float)Math.Sqrt(value);
            return value;
        }

        public static float GaussianBell(float x, float sigma)
        {
            return
                (float)(
                    1 / (Math.Sqrt(2 * Math.PI)) *
                    Math.Exp(-Math.Pow(x, 2.0) / (2 * Math.Pow(sigma, 2.0)))
                );
        }

        public static float MexicanHat(float x, float sigma)
        {
            return
                (float)(
                    1/(Math.Sqrt(2*Math.PI*Math.Pow(sigma,3.0))) *
                    (1 - Math.Pow(x,2.0) / Math.Pow(sigma,2.0))  *
                    Math.Exp( -Math.Pow(x,2.0) / (2*Math.Pow(sigma,2.0)))
                );
        }

    }
}
