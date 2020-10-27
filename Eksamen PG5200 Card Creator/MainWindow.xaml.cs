using Newtonsoft.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
        //List of all cards in the database
        private List<Card> m_cards = new List<Card>();
        private List<string> m_searchFilters = new List<string>();
        private ComboBoxItem m_selectedFilter = new ComboBoxItem();
        private List<string> m_JsonKeys = new List<string>();
        public MainWindow()
        {
            InitializeComponent();
            ReadDatabase();
            
            //Adding all the search filters to the list.
            for(int i = 0; i < searchFilterComboBox.Items.Count; i++)
            {
                m_searchFilters.Add(searchFilterComboBox.Items[i].ToString());
            }

            m_JsonKeys.AddRange(new List<string>
            {
                "Id",
                "cardName",
                "cardType",
                "cardAbility",
                "manaCost",
                "damage",
                "health",
                "cardImageAsBytes"
            });
        }

        /// <summary>
        /// Read the database and updates the list of cards to the current version of the database. Displays the cards in the ListView ordered alphabetically by the cards name.
        /// </summary>
        private void ReadDatabase()
        {
            using (SQLiteConnection connection = new SQLiteConnection(App.cardsDatabasePath))
            {
                connection.CreateTable<Card>();
                m_cards = (connection.Table<Card>().ToList()).OrderBy(c => c.cardName).ToList();
            }

            if (m_cards != null)
            {
                cardsListView.ItemsSource = m_cards;
            }
        }

        /// <summary>
        /// Creates a new instance of the 'NewCard' window, and opens it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateNewCard_Click(object sender, RoutedEventArgs e)
        {
            NewCardWindow newCard = new NewCardWindow();
            newCard.ShowDialog();

            ReadDatabase();
            searchBox.Text = "";
        }

        /// <summary>
        /// Checking if the imported json data contains all the 'keys' that our system requires it to have.
        /// Not optimal, since if a file contains all the keys, but as values instead, it will pass.
        /// </summary>
        /// <param name="json1">The json data we are checking</param>
        /// <returns>True or false if the data is of the correct format</returns>
        private bool IsImportedJsonFileCorrectFormat(string json) 
        {
            bool isCorrectFormat = true;
            foreach(string str in m_JsonKeys)
            {
                if (!json.Contains(str))
                {
                    isCorrectFormat = false;
                }
            }
            return isCorrectFormat;
        }

        /// <summary>
        /// Opens up the files explorer. Imports the selected card into the database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImportImage_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFilePrompt = new Microsoft.Win32.OpenFileDialog();
            //Creating a filter, so the only files that show up in the explorer are the type that the program supports.
            openFilePrompt.Filter = "Json files (*.json)|*.json|Text files (*.txt)|*.txt";

            bool? error = openFilePrompt.ShowDialog();

            //Checking to see if the user selected a file, or closed the dialog.
            if (error == true)
            {
                string importedJsonData = File.ReadAllText(openFilePrompt.FileName);

                if (IsImportedJsonFileCorrectFormat(importedJsonData))
                {
                    using (SQLiteConnection connection = new SQLiteConnection(App.cardsDatabasePath))
                    {
                        connection.CreateTable<Card>();
                        connection.Insert(JsonConvert.DeserializeObject<Card>(importedJsonData));
                    }
                    ReadDatabase();
                    searchBox.Text = "";
                }
                else
                {
                    MessageBox.Show("That Json files dont have the correct 'keys'. It is not of a format we support.");
                }
            }
        }

        /// <summary>
        /// Updates the ListView to only display the cards that match the filter depending on what filter is chosen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox searchTextBox = sender as TextBox;
            List<Card> filteredList = new List<Card>();

            //A combobox item's ToString gives "System.Windows.Controls.ComboBoxItem: " followed by the name of the item. We only want the name.
            switch (m_selectedFilter.ToString().Substring(38))
            {
                case "Name":
                    filteredList = m_cards.Where(c => c.cardName.ToLower().Contains(searchTextBox.Text.ToLower())).ToList();
                    break;
                case "Mana":
                    filteredList = m_cards.Where(c => c.manaCost.ToLower().Contains(searchTextBox.Text.ToLower())).ToList();
                    break;
                case "Damage":
                    filteredList = m_cards.Where(c => c.damage.ToLower().Contains(searchTextBox.Text.ToLower())).ToList();
                    break;
                case "Health":
                    filteredList = m_cards.Where(c => c.health.ToLower().Contains(searchTextBox.Text.ToLower())).ToList();
                    break;
            }

            cardsListView.ItemsSource = filteredList;
        }

        /// <summary>
        /// Creates a new instance of a 'NewType' window, and opens it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateNewType_Click(object sender, RoutedEventArgs e)
        {
            NewTypeWindow newType = new NewTypeWindow();
            newType.ShowDialog();
        }

        /// <summary>
        /// Sets the selectedfilter to the one chosen by the user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cb = e.Source as ComboBox;
            m_selectedFilter = (ComboBoxItem)cb.SelectedItem;
        }

        /// <summary>
        /// Opens up a preview window that displays the card the user clicked on.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CardsListView_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Card selectedCard = (Card)cardsListView.SelectedItem;

            if (selectedCard != null)
            {
                CardPreviewWindow cardDetailsWindow = new CardPreviewWindow(selectedCard);
                cardDetailsWindow.ShowDialog();
                ReadDatabase();
                searchBox.Text = "";
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