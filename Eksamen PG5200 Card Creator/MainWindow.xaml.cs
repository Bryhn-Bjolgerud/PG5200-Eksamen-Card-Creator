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
            /*
            Rect rect = new Rect(canvas.Margin.Left, canvas.Margin.Top, canvas.ActualWidth, canvas.ActualHeight);
            RenderTargetBitmap rtb = new RenderTargetBitmap((int)rect.Right, (int)rect.Bottom, 60, 70, PixelFormats.Default);
            rtb.Render(canvas);

            //encode as PNG
            BitmapEncoder pngEncoder = new PngBitmapEncoder();
            pngEncoder.Frames.Add(BitmapFrame.Create(rtb));

            //save to memory stream
            MemoryStream ms = new MemoryStream();
            pngEncoder.Save(ms);
            ms.Close();

            //convert to png and base64 string
            byte[] imageBytes = ms.ToArray();
            imageAsBase64 = Convert.ToBase64String(imageBytes);

            //Usikker på om vi kommer til å trenge denne. Du skal jo egentlig ikke lagre bildet i filsystemet, men i databasen. 
            //File.WriteAllBytes("../../Resources/logo.png", imageBytes);

            Card newCard = new Card()
            {
                cardName = nameCard.Text,
                cardType = cardClassType.SelectedItem.ToString(),
                manaCost = manaCard.Text,
                damage = damageCard.Text,
                health = healthCard.Text,
                cardAbility = abilityValue.Text,
                cardImageBase64 = Convert.ToBase64String(imageBytes)
            };

            using (SQLiteConnection connection = new SQLiteConnection(App.databasePath))
            {
                connection.CreateTable<Card>();
                connection.Insert(newCard);
            }

            "Json files (*.json)|*.json|Text files (*.txt)|*.txt";
            */
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
    }
}