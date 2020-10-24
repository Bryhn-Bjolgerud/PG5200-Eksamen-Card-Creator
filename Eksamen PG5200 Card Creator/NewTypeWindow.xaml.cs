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
        public NewTypeWindow()
        {
            InitializeComponent();
        }

        private void typeValue_GotFocus(object sender, RoutedEventArgs e)
        {
            typeValue.BorderBrush = Brushes.Gray;
            typeValue.TextAlignment = TextAlignment.Left;
            typeValue.Text = "";
        }
        private void typeValue_LostFocus(object sender, RoutedEventArgs e)
        {
            if (typeValue.Text == "")
            {
                typeValue.Text = "Set new type: ";
                typeValue.BorderBrush = App.yellowBrush;
            }

        }
        private void manaValue_GotFocus(object sender, RoutedEventArgs e)
        {
            manaValue.BorderBrush = Brushes.Gray;
            manaValue.TextAlignment = TextAlignment.Left;
            manaValue.Text = "";
        }

        private void manaValue_LostFocus(object sender, RoutedEventArgs e)
        {
            if (manaValue.Text == "")
            {
                manaValue.Text = "Set max mana: ";
                manaValue.BorderBrush = App.yellowBrush;
            }
            else
            {
                Regex reg = new Regex("^([0-9]|10)$");
                string regCheck = manaValue.Text.ToString();
                if (!reg.IsMatch(regCheck))
                {
                    manaValue.BorderBrush = Brushes.Red;
                    manaValue.TextAlignment = TextAlignment.Right;
                    manaValue.Text = "You must set max mana for your type";
                }
            }
        }

        private void damageValue_GotFocus(object sender, RoutedEventArgs e)
        {
            damageValue.BorderBrush = Brushes.Gray;
            damageValue.TextAlignment = TextAlignment.Left;
            damageValue.Text = "";
        }

        private void damageValue_LostFocus(object sender, RoutedEventArgs e)
        {
            if (damageValue.Text == "")
            {
                damageValue.Text = "Set max damage: ";
                damageValue.BorderBrush = App.yellowBrush;
            }
            else
            {
                Regex reg = new Regex("^([0-9])$");
                string regCheck = damageValue.Text.ToString();
                if (!reg.IsMatch(regCheck))
                {
                    damageValue.BorderBrush = Brushes.Red;
                    damageValue.TextAlignment = TextAlignment.Right;
                    damageValue.Text = "You must set max damage for your type";
                }
            }
        }

        private void healthValue_GotFocus(object sender, RoutedEventArgs e)
        {
            healthValue.BorderBrush = Brushes.Gray;
            healthValue.TextAlignment = TextAlignment.Left;
            healthValue.Text = "";
        }
        private void healthValue_LostFocus(object sender, RoutedEventArgs e)
        {
            if (healthValue.Text == "")
            {
                healthValue.Text = "Set max health: ";
                healthValue.BorderBrush = App.yellowBrush;
            }
            else
            {
                Regex reg = new Regex("^([0-9]|10)$");
                string regCheck = healthValue.Text.ToString();
                if (!reg.IsMatch(regCheck))
                {
                    healthValue.BorderBrush = Brushes.Red;
                    healthValue.TextAlignment = TextAlignment.Right;
                    healthValue.Text = "You must set max health for your type";
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
            pngEncoder.Frames.Add(BitmapFrame.Create((BitmapSource)userSelectedImage.Source));

            if (pngEncoder.Frames != null)
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
            } else
            {
                MessageBox.Show("You have not selected an image for your type!");
                return;
            }

            using (SQLiteConnection connection = new SQLiteConnection(App.cardTypesDatabasePath))
            {
                connection.CreateTable<CardType>();
                connection.Insert(newType);
            }
        }
    }

}
