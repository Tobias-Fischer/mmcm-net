using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace HelpersLib
{
    public static class ImageManipulation
    {
        public static Bitmap toBmp(ImageRgb img)
        {
            int w = img.width();
            int h = img.height();
            Bitmap bmp = new Bitmap(w, h, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            try
            {
                unsafe
                {
                    BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, w, h),
                        System.Drawing.Imaging.ImageLockMode.WriteOnly,
                        System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                    byte[] rawData = new byte[bmpData.Stride * bmpData.Height];
                    Marshal.Copy(img.getRawImage(), rawData, 0, rawData.Length);

                    Marshal.Copy(rawData, 0, bmpData.Scan0, rawData.Length);

                    bmp.UnlockBits(bmpData);
                }
            }
            catch (Exception e)
            {
                Console.Out.WriteLine("Exception while accessing image: " + e);
            }

            return bmp;
        }

        public static Bitmap toBmp(ImageFloat img)
        {
            int w = img.width();
            int h = img.height();
            Bitmap bmp = new Bitmap(w, h, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            for(int x=0;x<w;x++)
                for (int y = 0; y < h; y++)
                {
                    float px = img.getPixel(x,y);
                    Color color = Color.FromArgb((int)px, (int)px, (int)px);
                    bmp.SetPixel(x, y, color);
                }
            return bmp;
        }
    }
}
