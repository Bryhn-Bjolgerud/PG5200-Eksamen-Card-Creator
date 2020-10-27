using Eksamen_PG5200_Card_Creator.Classes;
using SQLite;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Eksamen_PG5200_Card_Creator
{
    /// <summary>
    /// Interaction logic for NewTypeWindow.xaml
    /// </summary>
    public partial class NewTypeWindow : Window
    {
        List<TextBox> cardValueTextboxes = new List<TextBox>();
        public NewTypeWindow()
        {
            InitializeComponent();
            cardValueTextboxes.Add(manaValue);
            cardValueTextboxes.Add(damageValue);
            cardValueTextboxes.Add(healthValue);
        }

        /// <summary>
        /// Checking every textbox to see if their values are default or not.
        /// </summary>
        /// <returns>A bool representing whether or not the type is setup correct.</returns>
        private bool IsTypeReady()
        {
            bool cardReady = true;

            foreach (TextBox tb in cardValueTextboxes)
            {
                //We dont allow stats to be bigger than 99.
                if (tb.Text.Length > 2)
                {
                    cardReady = false;
                }
            }

            //There are not restrictions for what the name of the type can be. 
            if (typeValue.Text == "Set type name: ")
            {
                cardReady = false;
            }

            return cardReady;
        }

        /// <summary>
        /// Calling the changeTextBox() but with preset values.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResetTextbox_GotFocus(object sender, RoutedEventArgs e)
        {
            SharedMethodsForWindows.ChangeTextBox(e.Source as TextBox, Brushes.Green, TextAlignment.Left, "");
        }

        private void TypeValue_LostFocus(object sender, RoutedEventArgs e)
        {
            if (typeValue.Text == "")
            {
                SharedMethodsForWindows.ChangeTextBox(e.Source as TextBox, App.yellowBrush, TextAlignment.Left, "Set type name: ");
            }
        }

        /// <summary>
        /// All of the Lostfocus() does more or less excatly the same thing so will only comment one of them.
        /// Checking if what the user typed in, is within the rules we set for each type of value.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ManaValue_LostFocus(object sender, RoutedEventArgs e)
        {
            if (manaValue.Text == "")
            {
                SharedMethodsForWindows.ChangeTextBox(e.Source as TextBox, App.yellowBrush, TextAlignment.Left, "Set max manacost: ");
            }
            else
            {
                if (!App.isNumbers.IsMatch(manaValue.Text) || manaValue.Text.Length > 2)
                {
                    SharedMethodsForWindows.ChangeTextBox(e.Source as TextBox, Brushes.Red, TextAlignment.Right, "Max manacost must be a number between 0-99!");
                } 
            }
        }

        private void DamageValue_LostFocus(object sender, RoutedEventArgs e)
        {
            if (damageValue.Text == "")
            {
                SharedMethodsForWindows.ChangeTextBox(e.Source as TextBox, App.yellowBrush, TextAlignment.Left, "Set max damage: ");
            }
            else
            {
                if (!App.isNumbers.IsMatch(damageValue.Text) || damageValue.Text.Length > 2)
                {
                    SharedMethodsForWindows.ChangeTextBox(e.Source as TextBox, Brushes.Red, TextAlignment.Right, "Max damage must be a number between 0-99!");
                }
            }
        }

        private void HealthValue_LostFocus(object sender, RoutedEventArgs e)
        {
            if (healthValue.Text == "")
            {
                SharedMethodsForWindows.ChangeTextBox(e.Source as TextBox, App.yellowBrush, TextAlignment.Left, "Set max health: ");
            }
            else
            {
                if (!App.isNumbers.IsMatch(healthValue.Text) || healthValue.Text.Length > 2)
                {
                    SharedMethodsForWindows.ChangeTextBox(e.Source as TextBox, Brushes.Red, TextAlignment.Right, "Max health must be a number between 0-99!");
                }
            }
        }

        /// <summary>
        /// Opens up the file explorer. Displays the chosen image.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UploadImage_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFilePrompt = new Microsoft.Win32.OpenFileDialog();
            //Setting a filter to only display images.
            openFilePrompt.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";

            bool? error = openFilePrompt.ShowDialog();

            //Checking if the user selected an image.
            if (error == true)
            {
                userSelectedImage.Source = new BitmapImage(new Uri(openFilePrompt.FileName, UriKind.Absolute));
            }
        }

        /// <summary>
        /// Converts an image.source into a .png.
        /// </summary>
        /// <returns>The png file.</returns>
        private BitmapEncoder ConvertUserSelectedImageSourceToPng()
        { 
            BitmapEncoder pngEncoder = new PngBitmapEncoder();
            pngEncoder.Frames.Add(BitmapFrame.Create((BitmapSource)userSelectedImage.Source));
            
            return pngEncoder;
        }

        /// <summary>
        /// Creating a new newType object and setting it's values corresponding to what the user typed. Then adding it to the database. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateType_Click(object sender, RoutedEventArgs e)
        {
            if (userSelectedImage.Source != null)
            { 
                if (IsTypeReady())
                {

                    CardType newType = new CardType()
                    {
                        cardType = typeValue.Text,
                        maxManaCost = Int32.Parse(manaValue.Text),
                        maxDamage = Int32.Parse(damageValue.Text),
                        maxHealth = Int32.Parse(healthValue.Text),
                        typeImage = SharedMethodsForWindows.ConvertImageToBytes(ConvertUserSelectedImageSourceToPng())
                    };

                    using (SQLiteConnection connection = new SQLiteConnection(App.cardTypesDatabasePath))
                    {
                        connection.CreateTable<CardType>();
                        connection.Insert(newType);
                    }
                    Close();
                }
                else
                {
                    MessageBox.Show("You have chosen invalid values for the type parameters!");
                }

            }
            else
            {
                MessageBox.Show("You have not selected an image for your type!");

            }
        }
    }
}
