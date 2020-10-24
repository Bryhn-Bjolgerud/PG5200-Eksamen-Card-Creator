using Newtonsoft.Json;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using Eksamen_PG5200_Card_Creator.Classes;
using System.Collections.Generic;
using System.Linq;
using SQLite;

namespace Eksamen_PG5200_Card_Creator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Card> cards;

        public MainWindow()
        {
            InitializeComponent();
            cards = new List<Card>();
            ReadDatabase();
        }

        void ReadDatabase()
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.cardsDatabasePath))
            {
                conn.CreateTable<Card>();
                cards = (conn.Table<Card>().ToList()).OrderBy(c => c.cardName).ToList();
            }

            if (cards != null)
            {
                cardsListView.ItemsSource = cards;
            }
        }

        private void createNewCard_Click(object sender, RoutedEventArgs e)
        {
            NewCardWindow newCard = new NewCardWindow();
            newCard.ShowDialog();

            ReadDatabase();
        }

        private void importImage_Click(object sender, RoutedEventArgs e)
        {

            Microsoft.Win32.OpenFileDialog openFilePrompt = new Microsoft.Win32.OpenFileDialog();
            openFilePrompt.Filter = "Json files (*.json)|*.json|Text files (*.txt)|*.txt";

            bool? error = openFilePrompt.ShowDialog();

            if (error == true)
            {
                string importedJsonData = File.ReadAllText(openFilePrompt.FileName);
                Card newCard = JsonConvert.DeserializeObject<Card>(importedJsonData);
                using (SQLiteConnection connection = new SQLiteConnection(App.cardsDatabasePath))
                {
                    connection.CreateTable<Card>();
                    connection.Insert(newCard);
                }
            }

            ReadDatabase();
        }

        private void cardsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Card selectedCard = (Card)cardsListView.SelectedItem;

            if (selectedCard != null)
            {
                CardPreviewWindow cardDetailsWindow = new CardPreviewWindow(selectedCard);
                cardDetailsWindow.ShowDialog();
            }
        }

        private void searchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox searchTextBox = sender as TextBox;

            var filteredList = cards.Where(c => c.cardName.ToLower().Contains(searchTextBox.Text.ToLower())).ToList();

            cardsListView.ItemsSource = filteredList;
        }

        private void createNewType_Click(object sender, RoutedEventArgs e)
        {
            NewTypeWindow newType = new NewTypeWindow();
            newType.ShowDialog();
        }

    }
}