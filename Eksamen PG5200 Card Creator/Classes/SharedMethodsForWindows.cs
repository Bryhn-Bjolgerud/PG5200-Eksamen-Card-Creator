using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Eksamen_PG5200_Card_Creator.Classes
{
    public static class SharedMethodsForWindows
    {

        /// <summary>
        /// Converting an image into a byte array.
        /// </summary>
        /// <returns>The byte array.</returns>
        public static byte[] ConvertImageToBytes(BitmapEncoder image)
        {
            byte[] imageBytes;

            //save to memory stream
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms);
                imageBytes = ms.ToArray();
            }

            return imageBytes;
        }

        /// <summary>
        /// Converting an array of bytes to an BitmapImage.
        /// </summary>
        /// <param name="imageBytes"></param>
        /// <returns>The BitmapImage</returns>
        public static BitmapImage ConvertByteToImage(byte[] imageBytes)
        {
            //Converts the byte array to a BitmapImage.
            BitmapImage controlImageSource = new BitmapImage();
            using (MemoryStream ms = new MemoryStream(imageBytes))
            {
                controlImageSource.BeginInit();
                controlImageSource.StreamSource = ms;
                controlImageSource.CacheOption = BitmapCacheOption.OnLoad;
                controlImageSource.EndInit();
            }

            return controlImageSource;
        }

        /// <summary>
        /// Applying changes to the textbox given as an argument.
        /// </summary>
        /// <param name="tb">The textbox that will be changed.</param>
        /// <param name="br">The brush the border will be set to.</param>
        /// <param name="ta">The type of text alignment to be used.</param>
        /// <param name="txt">The new text of the textbox.</param>
        public static void ChangeTextBox(TextBox tb, Brush br, TextAlignment ta, string txt)
        {
            tb.BorderBrush = br;
            tb.TextAlignment = ta;
            tb.Text = txt;
        }
    }
}
