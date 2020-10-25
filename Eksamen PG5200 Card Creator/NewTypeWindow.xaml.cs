using Eksamen_PG5200_Card_Creator.Classes;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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

        private bool isTypeReady()
        {
            bool cardReady = true;

            foreach (TextBox tb in cardValueTextboxes)
            {
                if (tb.Text.Length > 2)
                {
                    cardReady = false;
                }
            }

            if (typeValue.Text == "Set type name: ")
            {
                cardReady = false;
            }
            return cardReady;
        }


        private void changeTextBox(RoutedEventArgs e, Brush br, TextAlignment ta, string txt)
        {
            TextBox tb = e.Source as TextBox;
            tb.BorderBrush = br;
            tb.TextAlignment = ta;
            tb.Text = txt;
        }

        private void resetTextbox_GotFocus(object sender, RoutedEventArgs e)
        {
            changeTextBox(e, Brushes.Gray, TextAlignment.Left, "");
        }

        private void typeValue_LostFocus(object sender, RoutedEventArgs e)
        {
            if (typeValue.Text == "")
            {
                changeTextBox(e, App.yellowBrush, TextAlignment.Left, "Set type name: ");
            }
        }

        private void manaValue_LostFocus(object sender, RoutedEventArgs e)
        {
            if (manaValue.Text == "")
            {
                changeTextBox(e, App.yellowBrush, TextAlignment.Left, "Set max manacost: ");
            }
            else
            {
                if (!App.isChars.IsMatch(manaValue.Text))
                {
                    changeTextBox(e, Brushes.Red, TextAlignment.Right, "Max manacost must be a number!");
                }
            }
        }

        private void damageValue_LostFocus(object sender, RoutedEventArgs e)
        {
            if (damageValue.Text == "")
            {
                changeTextBox(e, App.yellowBrush, TextAlignment.Left, "Set max damage: ");
            }
            else
            {
                if (!App.isChars.IsMatch(damageValue.Text))
                {
                    changeTextBox(e, Brushes.Red, TextAlignment.Right, "Max damage must be a number!");
                }
            }
        }

        private void healthValue_LostFocus(object sender, RoutedEventArgs e)
        {
            if (healthValue.Text == "")
            {
                changeTextBox(e, App.yellowBrush, TextAlignment.Left, "Set max health: ");
            }
            else
            {
                if (!App.isChars.IsMatch(healthValue.Text))
                {
                    changeTextBox(e, Brushes.Red, TextAlignment.Right, "Max health must be a number!");
                }
            }
        }

        private void uploadImage_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFilePrompt = new Microsoft.Win32.OpenFileDialog();
            openFilePrompt.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";

            bool? error = openFilePrompt.ShowDialog();

            if (error == true)
            {
                userSelectedImage.Source = new BitmapImage(new Uri(openFilePrompt.FileName, UriKind.Absolute));
            }
        }

        private void createType_Click(object sender, RoutedEventArgs e)
        {
            CardType newType = new CardType();
            byte[] imageBytes;
            BitmapEncoder pngEncoder = new PngBitmapEncoder();
            if (userSelectedImage.Source != null)
            {
                pngEncoder.Frames.Add(BitmapFrame.Create((BitmapSource)userSelectedImage.Source));

                if (isTypeReady())
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        pngEncoder.Save(ms);
                        imageBytes = ms.ToArray();
                    }

                    newType = new CardType()
                    {
                        cardType = typeValue.Text,
                        maxManaCost = Int32.Parse(manaValue.Text),
                        maxDamage = Int32.Parse(damageValue.Text),
                        maxHealth = Int32.Parse(healthValue.Text),
                        typeImage = imageBytes
                    };

                    using (SQLiteConnection connection = new SQLiteConnection(App.cardTypesDatabasePath))
                    {
                        connection.CreateTable<CardType>();
                        connection.Insert(newType);
                    }
                    Close();
                }
                MessageBox.Show("You chosen invalid values for the type parameters!");
                return;
            }
            else
            {
                MessageBox.Show("You have not selected an image for your type!");
                return;
            }
        }
    }
}
