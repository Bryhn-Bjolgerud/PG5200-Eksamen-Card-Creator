using Eksamen_PG5200_Card_Creator.Classes;
using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for NewCardWindow.xaml
    /// </summary>
    public partial class NewCardWindow : Window
    {
        List<CardType> cardTypes;
        public NewCardWindow()
        {
            InitializeComponent();

            using (SQLiteConnection connection = new SQLiteConnection(App.cardsDatabasePath))
            {
                connection.CreateTable<CardType>();
                cardTypes = connection.Table<CardType>().ToList();
                foreach(CardType ct in cardTypes)
                {
                    cardTypeComboBox.Items.Add(ct.cardType);
                }
            }
        }

        private void makeCard_Click(object sender, RoutedEventArgs e)
        {
            abilityCard.Text = abilityValue.Text;

            if (manaValue.Text == "Mana cost has to be a number between 0-10" || manaValue.Text == "Enter mana: " || damageValue.Text == "Damage cannot exceed 25" || damageValue.Text == "Enter attack: " || nameValue.Text == "Enter name: " || healthValue.Text == "Health cannot exceed 25" || healthValue.Text == "Enter health: ")
            {
                MessageBox.Show("Before you can make card, please make sure you entered a valid input in all the boxes above!");
            }
            else
            {
                nameCard.Text = nameValue.Text;
                manaCard.Text = manaValue.Text;
                damageCard.Text = damageValue.Text;
                healthCard.Text = healthValue.Text;
            }
        }

        private void cardTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CardType selectedType = cardTypes.Find(x => x.cardType.Equals(cardTypeComboBox.SelectedItem.ToString()));
            
            if(selectedType != null)
            {
                BitmapImage cardDisplaySrc = new BitmapImage();
                using (MemoryStream ms = new MemoryStream(selectedType.typeImage))
                {
                    cardDisplaySrc.BeginInit();
                    cardDisplaySrc.StreamSource = ms;
                    cardDisplaySrc.CacheOption = BitmapCacheOption.OnLoad;
                    cardDisplaySrc.EndInit();
                }
                cardDisplay.Source = cardDisplaySrc; 
            }
        }

        private void NameValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }
        
        private void NameValue_GotFocus(object sender, RoutedEventArgs e)
        {
            nameValue.BorderBrush = Brushes.Gray;
            nameValue.TextAlignment = TextAlignment.Left;
            nameValue.Text = "";
        }

        private void NameValue_LostFocus(object sender, RoutedEventArgs e)
        {
            if (nameValue.Text == "")
            {
                nameValue.Text = "Enter name: ";
            }
        }

        private void ManaValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }
        private void manaValue_GotFocus(object sender, RoutedEventArgs e)
        {
            manaValue.BorderBrush = Brushes.Gray;
            manaValue.TextAlignment = TextAlignment.Left;
            manaValue.Text = "";
        }

        private void manaValue_LostFocus(object sender, RoutedEventArgs e)
        {
            if (manaValue.Text == "")
            {
                manaValue.Text = "Enter manacost: ";
            }
            else
            {
                Regex reg = new Regex("^([0-9]|10)$");
                string regCheck = manaValue.Text.ToString();
                if (!reg.IsMatch(regCheck))
                {
                    manaValue.BorderBrush = Brushes.Red;
                    manaValue.TextAlignment = TextAlignment.Right;
                    manaValue.Text = "Mana cost has to be a number between 0-10";
                }
            }
        }

        private void DamageValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }
        private void DamageValue_GotFocus(object sender, RoutedEventArgs e)
        {
            damageValue.BorderBrush = Brushes.Gray;
            damageValue.TextAlignment = TextAlignment.Left;
            damageValue.Text = "";
        }
        private void DamageValue_LostFocus(object sender, RoutedEventArgs e)
        {
            if (damageValue.Text == "")
            {
                damageValue.Text = "Enter attack: ";
            }
            else
            {
                Regex reg = new Regex("^([1]?[0-9]|2[0-5])$");
                string regCheck = damageValue.Text.ToString();
                if (!reg.IsMatch(regCheck))
                {
                    damageValue.BorderBrush = Brushes.Red;
                    damageValue.TextAlignment = TextAlignment.Right;
                    damageValue.Text = "Attack cannot exceed 25";
                }
            }
        }

        private void HealthValue_TextChanged(object sender, TextChangedEventArgs e)
        {
           
        }
        private void HealthValue_GotFocus(object sender, RoutedEventArgs e)
        {
            healthValue.BorderBrush = Brushes.Gray;
            healthValue.TextAlignment = TextAlignment.Left;
            healthValue.Text = "";
        }
        private void HealthValue_LostFocus(object sender, RoutedEventArgs e)
        {
            if (healthValue.Text == "")
            {
                healthValue.Text = "Enter health: ";
            }
            else
            {
                Regex reg = new Regex("^([1]?[1-9]|2[0-5])$");
                string regCheck = healthValue.Text.ToString();
                if (!reg.IsMatch(regCheck))
                {
                    healthValue.BorderBrush = Brushes.Red;
                    healthValue.TextAlignment = TextAlignment.Right;
                    healthValue.Text = "Health cannot exceed 25";
                }
            }
        }

        /// <summary>
        /// Need to do some error checking here maybe throw and catch exception to make sure proper files are uploaded. Not only if we managed to open the file explorer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uploadImage_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFilePrompt = new Microsoft.Win32.OpenFileDialog();
            openFilePrompt.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";

            bool? error = openFilePrompt.ShowDialog();

            if (error == true)
            {
                userSelectedImage.Source = new BitmapImage(new Uri(openFilePrompt.FileName, UriKind.Absolute));
            }
        }

        /// <summary>
        /// All the code for dragging the image inside the portrait frame... Need refactoring.
        /// </summary>

        private bool _isMoving;
        private Point? _buttonPosition;
        private double deltaX;
        private double deltaY;
        private TranslateTransform _currentTT;


        private void userSelectedImageMoving_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_buttonPosition == null)
                _buttonPosition = userSelectedImage.TransformToAncestor(MyGrid).Transform(new Point(0, 0));
            var mousePosition = Mouse.GetPosition(MyGrid);
            deltaX = mousePosition.X - _buttonPosition.Value.X;
            deltaY = mousePosition.Y - _buttonPosition.Value.Y;
            _isMoving = true;
        }

        private void userSelectedImageMoving_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            _currentTT = userSelectedImage.RenderTransform as TranslateTransform;
            _isMoving = false;
        }

        private void userSelectedImageMoving_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (!_isMoving) return;

            var mousePoint = Mouse.GetPosition(MyGrid);

            var offsetX = (_currentTT == null ? _buttonPosition.Value.X : _buttonPosition.Value.X - _currentTT.X) + deltaX - mousePoint.X;
            var offsetY = (_currentTT == null ? _buttonPosition.Value.Y : _buttonPosition.Value.Y - _currentTT.Y) + deltaY - mousePoint.Y;

            userSelectedImage.RenderTransform = new TranslateTransform(-offsetX, -offsetY);
        }

        private void userSelectedImageMoving_MouseEnter(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.SizeAll;
        }

        private void userSelectedImageMoving_MouseLeave(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
            _isMoving = false;
        }

        //-----------------------------------------------------------------------------------------------

       

        //-----------------------------------------------------------------------------------------------
        /// <summary>
        /// Må forklare  hva som skjer her litt ass. OGså endre varianel navn fordi disse er obviously copy pastet
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveImage_Click(object sender, RoutedEventArgs e)
        {
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

            //Usikker på om vi kommer til å trenge denne. Du skal jo egentlig ikke lagre bildet i filsystemet, men i databasen. 
            //File.WriteAllBytes("../../Resources/logo.png", imageBytes);

            Card newCard = new Card()
            {
                cardName = nameCard.Text,
                cardType = cardTypeComboBox.SelectedItem.ToString(),
                manaCost = manaCard.Text,
                damage = damageCard.Text,
                health = healthCard.Text,
                cardAbility = abilityValue.Text,
                cardImage = imageBytes
            };

            using (SQLiteConnection connection = new SQLiteConnection(App.cardsDatabasePath))
            {
                connection.CreateTable<Card>();
                connection.Insert(newCard);
            }
        }

    }
}
