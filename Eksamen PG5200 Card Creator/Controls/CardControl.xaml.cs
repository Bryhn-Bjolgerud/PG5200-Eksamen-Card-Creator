using Eksamen_PG5200_Card_Creator.Classes;
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

        public static readonly DependencyProperty CardProperty = DependencyProperty.Register("Card", typeof(Card), typeof(CardControl), new PropertyMetadata(null, SetImage));

        /// <summary>
        /// Setting the source of the image tag in the control.
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void SetImage(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CardControl control = d as CardControl;

            if (control != null)
            {
                control.cardImage.Source = convertByteToImage((e.NewValue as Card).cardImage);
            }
        }

        /// <summary>
        /// Converting an array of bytes to a BitmapImage.
        /// </summary>
        /// <param name="imageBytes">The byte representation of the image.</param>
        /// <returns>The converted BitmapImage</returns>
        static BitmapImage convertByteToImage(byte[] imageBytes)
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

        public CardControl()
        {
            InitializeComponent();
        }
    }
}