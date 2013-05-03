using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace CVZ_Core
{
    public static class ColorInterpolator
    {
        delegate byte ComponentSelector(Color color);
        static ComponentSelector _redSelector = color => color.R;
        static ComponentSelector _greenSelector = color => color.G;
        static ComponentSelector _blueSelector = color => color.B;

        public static Color InterpolateBetween(
            Color endPoint1,
            Color endPoint2,
            Color endPoint3,
            double lambda)
        {
            if (lambda < 0)
                lambda = 0;
            if (lambda > 1)
                lambda = 1;

            if (lambda < 0.5)
                return InterpolateBetween(endPoint1, endPoint2, lambda*2);
            else
                return InterpolateBetween(endPoint2, endPoint3, lambda / 2.0);
        }

        public static Color InterpolateBetween(
            Color endPoint1,
            Color endPoint2,
            double lambda)
        {
            if (lambda < 0)
                lambda = 0;
            if (lambda > 1)       
                lambda = 1;

            Color color = Color.FromArgb(
                InterpolateComponent(endPoint1, endPoint2, lambda, _redSelector),
                InterpolateComponent(endPoint1, endPoint2, lambda, _greenSelector),
                InterpolateComponent(endPoint1, endPoint2, lambda, _blueSelector)
            );

            return color;
        }

        static byte InterpolateComponent(
            Color endPoint1,
            Color endPoint2,
            double lambda,
            ComponentSelector selector)
        {
            return (byte)(selector(endPoint1)
                + (selector(endPoint2) - selector(endPoint1)) * lambda);
        }
    }

    /*
    How do you modify this if color B is not the midpoint between color A and color C? The easiest way is the following. 
     * If the parameter (what I call "lambda") is less than 0.5, multiply lambda  by two and return the interpolated color between color A and color B. 
     * If the parameter is greater than 0.5, multiply lambda  by two and subtract one (this maps [0.5, 1] onto [0, 1]) and return the interpolated color between color B 
     * and color C.
    If you don't like the requirement that color B live on the line between color A and color C, 
     * then you can use exactly the modification that I just described to do a piecewise-linear interpolation between the colors.
    Finally, you did not specify if you want to interpolate the so-called alpha value (the 'A' in "ARGB"). 
     * The above code is easily modified to handle this situation too. Add one more ComponentSelector defined as color => color.A, 
     * use InterpolateComponent to interpolate this value and use the Color.FromArgb(int, int, int, int) overload of Color.FromArgb.
     
    */
}
