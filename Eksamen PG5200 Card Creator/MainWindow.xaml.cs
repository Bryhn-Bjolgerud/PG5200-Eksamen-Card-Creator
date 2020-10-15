using GalaSoft.MvvmLight.Command;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Eksamen_PG5200_Card_Creator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ImageBrush placeholderImage = new ImageBrush()
        {
            Stretch = Stretch.None,
            AlignmentX = AlignmentX.Left
        };

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MakeCard_Click(object sender, RoutedEventArgs e)
        {
            ManaCard.Text = ManaValue.Text;
            DamageCard.Text = DamageValue.Text;
            HealthCard.Text = HealthValue.Text;
        }

        private void cardClassType_SelectionChanged(object sender, RoutedEventArgs e)
        {
            switch (cardClassType.SelectedItem.ToString())
            {
                case "System.Windows.Controls.ComboBoxItem: Death Knight":
                    changeBaseCard("deathKnight");
                    break;
                case "System.Windows.Controls.ComboBoxItem: Demon Hunter":
                    changeBaseCard("demonHunter");
                    break;
                case "System.Windows.Controls.ComboBoxItem: Druid":
                    changeBaseCard("druid");
                    break;
                case "System.Windows.Controls.ComboBoxItem: Hunter":
                    changeBaseCard("hunter");
                    break;
                case "System.Windows.Controls.ComboBoxItem: Mage":
                    changeBaseCard("mage");
                    break;
                case "System.Windows.Controls.ComboBoxItem: Neutral":
                    changeBaseCard("neutral");
                    break;
                case "System.Windows.Controls.ComboBoxItem: Paladin":
                    changeBaseCard("paladin");
                    break;
                case "System.Windows.Controls.ComboBoxItem: Priest":
                    changeBaseCard("priest");
                    break;
                case "System.Windows.Controls.ComboBoxItem: Rogue":
                    changeBaseCard("rogue");
                    break;
                case "System.Windows.Controls.ComboBoxItem: Shaman":
                    changeBaseCard("shaman");
                    break;
                case "System.Windows.Controls.ComboBoxItem: Warlock":
                    changeBaseCard("warlock");
                    break;
                case "System.Windows.Controls.ComboBoxItem: Warrior":
                    changeBaseCard("warrior");
                    break;
            }
        }

        private void changeBaseCard(String cardClass)
        {
            cardDisplay.Source = new BitmapImage(new Uri("Resources/classBaseCards/" + cardClass + "BaseCard.png", UriKind.Relative)); 
        }

        private void NameValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (NameValue.Text == "")
            {
                placeholderImage.ImageSource = new BitmapImage(new Uri("Resources/TextboxPlaceholderImages/cardName.png", UriKind.RelativeOrAbsolute));
                NameValue.Background = placeholderImage;
            }
            else
            {
                NameValue.Background = null;
            }
        }

        private void ManaValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ManaValue.Text == "")
            {

                placeholderImage.ImageSource = new BitmapImage(new Uri("Resources/TextboxPlaceholderImages/manaCost.png", UriKind.RelativeOrAbsolute));
                NameValue.Background = placeholderImage;
            }
            else
            {
                ManaValue.Background = null;
            }
        }

        private void DamageValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (DamageValue.Text == "")
            {

                placeholderImage.ImageSource = new BitmapImage(new Uri("Resources/TextboxPlaceholderImages/damageDealt.png", UriKind.RelativeOrAbsolute));
                NameValue.Background = placeholderImage;
            }
            else
            {
                DamageValue.Background = null;
            }
        }

        private void HealthValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (HealthValue.Text == "")
            {

                placeholderImage.ImageSource = new BitmapImage(new Uri("Resources/TextboxPlaceholderImages/health.png", UriKind.RelativeOrAbsolute));
                NameValue.Background = placeholderImage;
            }
            else
            {
                HealthValue.Background = null;
            }
        }
    }
}