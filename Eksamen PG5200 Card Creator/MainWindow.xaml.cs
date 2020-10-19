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
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.databasePath))
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

        }
    }
}