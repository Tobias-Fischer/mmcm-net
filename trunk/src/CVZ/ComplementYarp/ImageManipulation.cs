using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace CVZ_Core
{
    public static class ImageManipulation
    {

	   /// <summary>
	   /// Self explicit
	   /// </summary>
	   /// <param name="v"></param>
	   /// <param name="img"></param>
	   public static void Vector2Image(float[] v, ref ImageFloat img, int w, int h)
	   {
		  double t0 = Time.now();
		  img.resize(w, h);
		  for (int i = 0; i < w; i++)
		  {
			 for (int j = 0; j < h; j++)
			 {
				img.setPixel(i, j, (float)v[j * w + i] * 255.0f);
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
	   public static float[] Image2Vector(ImageFloat img, int w, int h)
	   {
		  ImageFloat img2 = new ImageFloat();
		  img2.copy(img, w, h);

		  float[] v = new float[w*h];

		  for (int i = 0; i < w; i++)
		  {
			 for (int j = 0; j < h; j++)
			 {
				float px = img2.getPixel(i, j) / 255.0f;
				v[j * w + i] = px;
			 }
		  }
		  //Console.WriteLine("Image2Vector : " + (t1 - t0).ToString());

		  return v;
	   }

	   public static Bitmap toBmp(this ImageRgb img)
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

	   public static Bitmap toBmp(this ImageFloat img)
	   {
		  int w = img.width();
		  int h = img.height();
		  Bitmap bmp = new Bitmap(w, h, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
		  for (int x = 0; x < w; x++)
			 for (int y = 0; y < h; y++)
			 {
				float px = img.getPixel(x, y);
				Color color = Color.FromArgb((int)px, (int)px, (int)px);
				bmp.SetPixel(x, y, color);
			 }
		  return bmp;
	   }

	   public static ImageRgb toImgRgb(this Bitmap bmp)
	   {
		  ImageRgb img = new ImageRgb();
		  img.resize(bmp.Width, bmp.Height);
		  try
		  {
			 unsafe
			 {
				BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, img.width(), img.height()),
				    System.Drawing.Imaging.ImageLockMode.ReadOnly,
				    System.Drawing.Imaging.PixelFormat.Format24bppRgb);

				byte[] rawData = new byte[bmpData.Stride * bmpData.Height];
				Marshal.Copy(bmpData.Scan0, rawData, 0, rawData.Length);

				Marshal.Copy(rawData, 0, img.getRawImage(), rawData.Length);


				bmp.UnlockBits(bmpData);
			 }
		  }
		  catch (Exception e)
		  {
			 Console.Out.WriteLine("Exception while accessing image: " + e);
		  }

		  return img;
	   }
    }
}
