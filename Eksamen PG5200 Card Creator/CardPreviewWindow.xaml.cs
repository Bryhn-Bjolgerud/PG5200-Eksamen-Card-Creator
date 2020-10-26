using Eksamen_PG5200_Card_Creator.Classes;
using Newtonsoft.Json;
using SQLite;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Eksamen_PG5200_Card_Creator
{
    /// <summary>
    /// Interaction logic for CardPreviewWindow.xaml
    /// </summary>
    public partial class CardPreviewWindow : Window
    {
        private Card m_card;
        public CardPreviewWindow(Card card)
        {
            InitializeComponent();
            m_card = card;
            cardImage.Source = convertByteToImage(m_card.cardImage);
        }

        /// <summary>
        /// Deletes the card that is currently being previewed from the database. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteCardButton_Click(object sender, RoutedEventArgs e)
        {
            using (SQLiteConnection connection = new SQLiteConnection(App.cardsDatabasePath))
            {
                connection.CreateTable<Card>();
                connection.Delete(m_card);
            }
            Close();
        }

        /// <summary>
        /// Exporting the card to a json file using the jsonSerializer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exportCardButton_Click(object sender, RoutedEventArgs e)
        {
            string jsonData = JsonConvert.SerializeObject(m_card);
            File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/" + m_card.cardName + m_card.Id + ".json", jsonData);
            MessageBox.Show("Your .json file is added to your 'My Documents' folder");
        }

        /// <summary>
        /// Converting an array of bytes to a BitmapImage.
        /// </summary>
        /// <param name="imageBytes">The byte representation of the image.</param>
        /// <returns>The converted BitmapImage</returns>
        private BitmapImage convertByteToImage(byte [] imageBytes)
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
    }
}
