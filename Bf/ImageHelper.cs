using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Bf
{
    public static class ImageHelper
    {
        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.AssumeLinear;
                graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                graphics.SmoothingMode = SmoothingMode.None;
                graphics.PixelOffsetMode = PixelOffsetMode.HighSpeed;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        public static ulong[] BitmapToBfMemFrame(Bitmap image, int bitsPerWord)
        {
            if (image.Width % bitsPerWord != 0)
            {
                throw new Exception("image.Width % bitsPerWord != 0");
            }

            var w = image.Width;
            var h = image.Height;

            var words = new ulong[h * (w / bitsPerWord)];

            var data = image.LockBits(new Rectangle(0, 0, w, h),
                ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            unsafe
            {
                byte* ptr = (byte*) data.Scan0;
                for (int i = 0; i < h * (w / bitsPerWord); i++)
                {
                    for (int j = 0; j < bitsPerWord; j++)
                    {
                        var pxIdx = i * bitsPerWord + j;
                        //var a = *(ptr + 0);
                        var b = *(ptr + (pxIdx * 3) + 0);
                        var g = *(ptr + (pxIdx * 3) + 1);
                        var r = *(ptr + (pxIdx * 3) + 2);
                        if (r > 20 && g > 20 && b > 20)
                        {
                            words[i] |= (ulong) 1 << j;
                        }
                    }
                }
            }

            image.UnlockBits(data);
            return words;
        }
    }
}