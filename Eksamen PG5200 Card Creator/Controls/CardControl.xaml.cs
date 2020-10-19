using Eksamen_PG5200_Card_Creator.Classes;
using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Eksamen_PG5200_Card_Creator.Controls
{
    /// <summary>
    /// Interaction logic for CardControl.xaml
    /// </summary>
    public partial class CardControl : UserControl
    {

        public Card Card
        {
            get { return (Card)GetValue(CardProperty); }
            set { SetValue(CardProperty, value); }
        }

        public static readonly DependencyProperty CardProperty = DependencyProperty.Register("Card", typeof(Card), typeof(CardControl), new PropertyMetadata(null, SetText));


        private static void SetText(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CardControl control = d as CardControl;

            if (control != null)
            {
                byte[] imageBytes = Convert.FromBase64String((e.NewValue as Card).cardImageBase64);
                BitmapImage controlImageSource = new BitmapImage();
                using (MemoryStream ms = new MemoryStream(imageBytes))
                {
                    controlImageSource.BeginInit();
                    controlImageSource.StreamSource = ms;
                    controlImageSource.CacheOption = BitmapCacheOption.OnLoad;
                    controlImageSource.EndInit();
                }

                control.cardImage.Source = controlImageSource;
            }
        }
        public CardControl()
        {
            InitializeComponent();
        }
    }
}