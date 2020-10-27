using Eksamen_PG5200_Card_Creator.Classes;
using Newtonsoft.Json;
using SQLite;
using System;
using System.IO;
using System.Windows;

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
            cardImage.Source = SharedMethodsForWindows.ConvertByteToImage(m_card.cardImageAsBytes);
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
    }
}
