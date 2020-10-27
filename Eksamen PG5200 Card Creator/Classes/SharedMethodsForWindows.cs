using System;
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

        /// <summary>
        /// Checking if what the user typed in the textbox is within our rules. Changing the textbox accordingly.
        /// </summary>
        /// <param name="tb">The textbox we want to check.</param>
        /// <param name="selectedType">The type of card being created.</param>
        public static void CheckAndChangeNewCardTextBox(TextBox tb, CardType selectedType)
        {
            string defaultText = "";
            string wrongInputText = "";
            int maxCost = 0;

            switch (tb.Name)
            {
                case "nameValue":
                    defaultText = "Enter name:";
                    break;
                case "manaValue":
                    defaultText = "Enter manacost:";
                    wrongInputText = "Manacost has to be a number between 0 - " + selectedType.maxManaCost.ToString();
                    maxCost = selectedType.maxManaCost;
                    break;
                case "damageValue":
                    defaultText = "Enter damage:";
                    wrongInputText = "Damage has to be a number between 0 - " + selectedType.maxDamage.ToString();
                    maxCost = selectedType.maxDamage;
                    break;
                case "healthValue":
                    defaultText = "Enter health:";
                    wrongInputText = "Health has to be a number between 0 - " + selectedType.maxHealth.ToString();
                    maxCost = selectedType.maxHealth;
                    break;
                case "abilityValue":
                    defaultText = "Enter card ability:";
                    break;
            }

            if (tb.Text == "")
            {
                ChangeTextBox(tb, App.yellowBrush, TextAlignment.Left, defaultText);

            }
            //If the textbox being checked is one of the numerical stats, we validate the input.
            else if (tb.Name != "nameValue" && tb.Name != "abilityValue")
            {
                if (!App.isNumbers.IsMatch(tb.Text) || tb.Text.Length > 2 || Int64.Parse(tb.Text) > maxCost)
                {
                    ChangeTextBox(tb, Brushes.Red, TextAlignment.Right, wrongInputText);
                }
            }
        }

        public static void CheckAndChangeNewTypeTextBox(TextBox tb)
        {
            string defaultText = "";
            string wrongInputText = "";

            switch (tb.Name)
            {
                case "typeValue":
                    defaultText = "Set typename:";
                    break;
                case "manaValue":
                    defaultText = "Set manacost:";
                    wrongInputText = "Max manacost must be a number between 0 - 99!";
                    break;
                case "damageValue":
                    defaultText = "Set max damage:";
                    wrongInputText = "Max damage must be a number between 0 - 99!";
                    break;
                case "healthValue":
                    defaultText = "Set max health:";
                    wrongInputText = "Max health must be a number between 0 - 99!";
                    break;
            }

            if (tb.Text == "")
            {
                ChangeTextBox(tb, App.yellowBrush, TextAlignment.Left, defaultText);
            }
            else if(tb.Name != "typeValue")
            {
                if (!App.isNumbers.IsMatch(tb.Text) || tb.Text.Length > 2)
                {
                    ChangeTextBox(tb, Brushes.Red, TextAlignment.Right, wrongInputText);
                }
            }
        }
    }
}
