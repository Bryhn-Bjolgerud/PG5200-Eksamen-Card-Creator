using Eksamen_PG5200_Card_Creator.Classes;
using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
    /// Interaction logic for CardPreviewWindow.xaml
    /// </summary>
    public partial class CardPreviewWindow : Window
    {
        private Card m_card;
        public CardPreviewWindow(Card card)
        {
            InitializeComponent();
            m_card = card;

            BitmapImage controlImageSource = new BitmapImage();
            using (MemoryStream ms = new MemoryStream(m_card.cardImage))
            {
                controlImageSource.BeginInit();
                controlImageSource.StreamSource = ms;
                controlImageSource.CacheOption = BitmapCacheOption.OnLoad;
                controlImageSource.EndInit();
            }

            cardImage.Source = controlImageSource;
        }

        private void deleteCardButton_Click(object sender, RoutedEventArgs e)
        {
            using (SQLiteConnection connection = new SQLiteConnection(App.cardsDatabasePath))
            {
                connection.CreateTable<Card>();
                connection.Delete(m_card);
            }

            Close();
        }

        private void exportCardButton_Click(object sender, RoutedEventArgs e)
        {
            string jsonData = JsonConvert.SerializeObject(m_card);
            File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/" + m_card.cardName + m_card.Id + ".json", jsonData);
            MessageBox.Show("Your .json file is added to your 'My Documents' folder");
        }
    }
}
