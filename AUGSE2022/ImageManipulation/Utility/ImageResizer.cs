using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

namespace AUGSE2022
{
    public class ImageResizer
    {
        public static Image Resize(Image source, int maxSize = 1920)
        {
            int sizeWidth = source.Width;
            int sizeHeight = source.Height;

            if (source.Width > source.Height)
            {
                int adjWidth = Math.Min(source.Width, maxSize);
                decimal scale = (decimal)adjWidth / (decimal)sizeWidth;
                sizeHeight = (int)Math.Truncate((decimal)source.Height * scale);
                sizeWidth = adjWidth;
            }
            else
            {
                int adjHeight = Math.Min(source.Height, maxSize);
                decimal scale = (decimal)adjHeight / (decimal)sizeHeight;
                sizeWidth = (int)Math.Truncate((decimal)source.Width * scale);
                sizeHeight = adjHeight;
            }

            Image img = ResizeImageKeepAspectRatio(source, sizeWidth, sizeHeight);
            return img;
        }

        /// <summary>
        /// Resize an image keeping its aspect ratio (cropping may occur).
        /// </summary>
        /// <param name="source"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        private static Image ResizeImageKeepAspectRatio(Image source, int width, int height)
        {
            // Credit to this method: Alex Domenici
            // https://alex.domenici.net/archive/resize-and-crop-an-image-keeping-its-aspect-ratio-with-c-sharp#:~:text=When%20we%20have%20to%20resize%20an%20image%20to,onto%20the%20image%20referenced%20by%20the%20Graphics%20instance.

            Image result = null;

            try
            {
                if (source.Width != width || source.Height != height)
                {
                    // Resize image
                    float sourceRatio = (float)source.Width / source.Height;

                    using (var target = new Bitmap(width, height))
                    {
                        using (var g = System.Drawing.Graphics.FromImage(target))
                        {
                            g.CompositingQuality = CompositingQuality.HighQuality;
                            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            g.SmoothingMode = SmoothingMode.HighQuality;

                            // Scaling
                            float scaling;
                            float scalingY = (float)source.Height / height;
                            float scalingX = (float)source.Width / width;
                            if (scalingX < scalingY) scaling = scalingX; else scaling = scalingY;

                            int newWidth = (int)(source.Width / scaling);
                            int newHeight = (int)(source.Height / scaling);

                            // Correct float to int rounding
                            if (newWidth < width) newWidth = width;
                            if (newHeight < height) newHeight = height;

                            // See if image needs to be cropped
                            int shiftX = 0;
                            int shiftY = 0;

                            // Draw image
                            g.DrawImage(source, -shiftX, -shiftY, newWidth, newHeight);
                        }

                        result = new Bitmap(target);
                    }
                }
                else
                {
                    // Image size matched the given size
                    result = new Bitmap(source);
                }
            }
            catch (Exception)
            {
                result = null;
            }

            return result;
        }

        public static byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }

    }
}
