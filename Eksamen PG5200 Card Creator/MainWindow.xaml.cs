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
        List<Card> cards = new List<Card>();
        List<string> searchFilters = new List<string>();
        ComboBoxItem selectedFilter = new ComboBoxItem();
        public MainWindow()
        {
            InitializeComponent();
            ReadDatabase();
            
            for(int i = 0; i < searchFilterComboBox.Items.Count; i++)
            {
                searchFilters.Add(searchFilterComboBox.Items[i].ToString());
            }
            
        }

        private void ReadDatabase()
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

        private void CreateNewCard_Click(object sender, RoutedEventArgs e)
        {
            NewCardWindow newCard = new NewCardWindow();
            newCard.ShowDialog();

            ReadDatabase();
        }

        private void ImportImage_Click(object sender, RoutedEventArgs e)
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

                ReadDatabase();
            }
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox searchTextBox = sender as TextBox;
            List<Card> filteredList = new List<Card>();

            switch (selectedFilter.ToString().Substring(38))
            {
                case "Name":
                    filteredList = cards.Where(c => c.cardName.ToLower().Contains(searchTextBox.Text.ToLower())).ToList();
                    break;
                case "Mana":
                    filteredList = cards.Where(c => c.manaCost.ToLower().Contains(searchTextBox.Text.ToLower())).ToList();
                    break;
                case "Damage":
                    filteredList = cards.Where(c => c.damage.ToLower().Contains(searchTextBox.Text.ToLower())).ToList();
                    break;
                case "Health":
                    filteredList = cards.Where(c => c.health.ToLower().Contains(searchTextBox.Text.ToLower())).ToList();
                    break;
            }

            cardsListView.ItemsSource = filteredList;
        }

        private void CreateNewType_Click(object sender, RoutedEventArgs e)
        {
            NewTypeWindow newType = new NewTypeWindow();
            newType.ShowDialog();
        }

        private void SearchFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cb = e.Source as ComboBox;
            selectedFilter = (ComboBoxItem)cb.SelectedItem;
        }

        private void CardsListView_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Card selectedCard = (Card)cardsListView.SelectedItem;

            if (selectedCard != null)
            {
                CardPreviewWindow cardDetailsWindow = new CardPreviewWindow(selectedCard);
                cardDetailsWindow.ShowDialog();
                ReadDatabase();
            }
        }

        private void CardsListView_MouseEnter(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        private void CardsListView_MouseLeave(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Arrow;
        }
    }
}