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
        CardType selectedType;
        List<TextBox> cardValueTextboxes = new List<TextBox>();
        bool statsAppliedToCard;
        public NewCardWindow()
        {
            InitializeComponent();

            //Since a user can create new types, we need to make the comboBoxItems based on what is in the database.
            using (SQLiteConnection connection = new SQLiteConnection(App.cardTypesDatabasePath))
            {
                connection.CreateTable<CardType>();
                cardTypes = connection.Table<CardType>().ToList();
                foreach (CardType ct in cardTypes)
                {
                    cardTypeComboBox.Items.Add(ct.cardType);
                }
            }

            cardValueTextboxes.Add(nameValue);
            cardValueTextboxes.Add(manaValue);
            cardValueTextboxes.Add(damageValue);
            cardValueTextboxes.Add(healthValue);
            cardValueTextboxes.Add(abilityValue);

            cardTypeComboBox.SelectedIndex = 6;
        }

        /// <summary>
        /// Checking every textbox to see if their values are default or not.
        /// </summary>
        /// <returns>A bool representing whether or not the user have set any values in the textbox.</returns>
        private bool IsCardValuesNotDefault()
        {
            bool cardReady = true;

            foreach (TextBox tb in cardValueTextboxes)
            {
                if (tb == nameValue)
                {
                    if (nameValue.Text == "Enter name: ")
                    {
                        cardReady = false;
                    }
                }
                else if (tb == abilityValue)
                {
                    if (abilityValue.Text == "Enter card ability: ")
                    {
                        cardReady = false;
                    }
                }
                else
                {
                    //We dont allow stats to be bigger than 99.
                    if (tb.Text.Length > 2)
                    {
                        cardReady = false;
                    }
                }
            }
            return cardReady;
        }

        /// <summary>
        /// If the user has set values for each stat of the card, we transfer those onto the card.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MakeCard_Click(object sender, RoutedEventArgs e)
        {
            if (!IsCardValuesNotDefault())
            {
                MessageBox.Show("Before you can make card, please make sure you entered a valid input in all the boxes above!");
            }
            else
            {
                nameCard.Text = nameValue.Text;
                manaCard.Text = manaValue.Text;
                damageCard.Text = damageValue.Text;
                healthCard.Text = healthValue.Text;
                abilityCard.Text = abilityValue.Text;
                statsAppliedToCard = true;
            }
        }

        /// <summary>
        /// Displaying the correct base card depending on what type of card is chosen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CardTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedType = cardTypes.Find(x => x.cardType.Equals(cardTypeComboBox.SelectedItem.ToString()));

            if (selectedType != null)
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
                PrepareForNextCard();
            }
        }

        private void ChangeTextBox(TextBox tb, Brush br, TextAlignment ta, string txt)
        {
            tb.BorderBrush = br;
            tb.TextAlignment = ta;
            tb.Text = txt;
        }

        private void ResetTextbox_GotFocus(object sender, RoutedEventArgs e)
        {
            ChangeTextBox(e.Source as TextBox, Brushes.Green, TextAlignment.Left, "");
        }


        /// <summary>
        /// All of the Lostfocus() does more or less excatly the same thing so will only comment one of them.
        /// Checking if what the user typed in, is within the rules we set for each type of value.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NameValue_LostFocus(object sender, RoutedEventArgs e)
        {
            if (nameValue.Text == "")
            {
                ChangeTextBox(e.Source as TextBox, App.yellowBrush, TextAlignment.Left, "Enter name: ");
            }
        }

        private void ManaValue_LostFocus(object sender, RoutedEventArgs e)
        {
            if (manaValue.Text == "")
            {
                ChangeTextBox(e.Source as TextBox, App.yellowBrush, TextAlignment.Left, "Enter manacost: ");
            }
            else if (!App.isNumbers.IsMatch(manaValue.Text) || manaValue.Text.Length > 2 || Int64.Parse(manaValue.Text) > selectedType.maxManaCost)
            {
                ChangeTextBox(e.Source as TextBox, Brushes.Red, TextAlignment.Right, "Manacost has to be a number between 0 - " + selectedType.maxManaCost.ToString());
            }

        }

        private void DamageValue_LostFocus(object sender, RoutedEventArgs e)
        {
            if (damageValue.Text == "")
            {
                ChangeTextBox(e.Source as TextBox, App.yellowBrush, TextAlignment.Left, "Enter damage: ");
            }
            else if (!App.isNumbers.IsMatch(damageValue.Text) || damageValue.Text.Length > 2 || Int64.Parse(damageValue.Text) > selectedType.maxDamage)
            {
                ChangeTextBox(e.Source as TextBox, Brushes.Red, TextAlignment.Right, "Damage has to be a number between 0 - " + selectedType.maxDamage.ToString());
            }
        }



        private void HealthValue_LostFocus(object sender, RoutedEventArgs e)
        {
            if (healthValue.Text == "")
            {
                ChangeTextBox(e.Source as TextBox, App.yellowBrush, TextAlignment.Left, "Enter health: ");
            }
            else if (!App.isNumbers.IsMatch(healthValue.Text) || healthValue.Text.Length > 2 || Int64.Parse(healthValue.Text) > selectedType.maxHealth)
            {
                ChangeTextBox(e.Source as TextBox, Brushes.Red, TextAlignment.Right, "Damage has to be a number between 0 - " + selectedType.maxHealth.ToString());
            }
        }

        private void AbilityValue_LostFocus(object sender, RoutedEventArgs e)
        {
            if (abilityValue.Text == "")
            {
                ChangeTextBox(e.Source as TextBox, App.yellowBrush, TextAlignment.Left, "Enter card ability: ");
            }
        }

        private void UploadImage_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFilePrompt = new Microsoft.Win32.OpenFileDialog();
            openFilePrompt.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";

            bool? error = openFilePrompt.ShowDialog();

            if (error == true)
            {
                userSelectedImage.Source = new BitmapImage(new Uri(openFilePrompt.FileName, UriKind.Absolute));
            }
        }


        //-----------------------------------------------------------------------------------------------
        /// <summary>
        /// All the code for dragging the image inside the portrait frame... Need refactoring.
        /// </summary>

        private bool _isMoving;
        private Point? _imagePosition;
        private double deltaX;
        private double deltaY;
        private TranslateTransform _currentTT;


        private void userSelectedImageMoving_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_imagePosition == null)
            {
                _imagePosition = userSelectedImage.TransformToAncestor(userSelectedImageContainer).Transform(new Point(0, 0));
            }
            var mousePosition = Mouse.GetPosition(userSelectedImageContainer);
            deltaX = mousePosition.X - _imagePosition.Value.X;
            deltaY = mousePosition.Y - _imagePosition.Value.Y;
            _isMoving = true;
        }

        private void userSelectedImageMoving_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            _currentTT = userSelectedImage.RenderTransform as TranslateTransform;
            _isMoving = false;
        }

        private void userSelectedImageMoving_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (!_isMoving)
            {
                return;
            }

            var mousePoint = Mouse.GetPosition(userSelectedImageContainer);

            var offsetX = (_currentTT == null ? _imagePosition.Value.X : _imagePosition.Value.X - _currentTT.X) + deltaX - mousePoint.X;
            var offsetY = (_currentTT == null ? _imagePosition.Value.Y : _imagePosition.Value.Y - _currentTT.Y) + deltaY - mousePoint.Y;

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

        /// <summary>
        /// Creating a new card object and setting it's values corresponding to what the user typed. Then adding it to the database. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveCard_Click(object sender, RoutedEventArgs e)
        {
            if (statsAppliedToCard)
            {
                Card newCard = new Card()
                {
                    cardName = nameCard.Text,
                    cardType = cardTypeComboBox.SelectedItem.ToString(),
                    manaCost = manaCard.Text,
                    damage = damageCard.Text,
                    health = healthCard.Text,
                    cardAbility = abilityValue.Text,
                    cardImage = CreateImageFromCanvas()
                };

                using (SQLiteConnection connection = new SQLiteConnection(App.cardsDatabasePath))
                {
                    connection.CreateTable<Card>();
                    connection.Insert(newCard);
                }
                statsAppliedToCard = false;
                PrepareForNextCard();
                cardTypeComboBox.SelectedIndex = 6;
                MessageBox.Show("Your card was created successfully!");
            }
            else
            {
                MessageBox.Show("Give the card some stats!");
            }
        }

        /// <summary>
        /// Converting whats inside the canvas into a byte array.
        /// </summary>
        /// <returns>The image of whats inside the canvas, as a byte array.</returns>
        private byte[] CreateImageFromCanvas()
        {
            byte[] imageBytes;
            Rect canvasAsRectangle = new Rect(userCreatedCard.Margin.Left, userCreatedCard.Margin.Top, userCreatedCard.ActualWidth, userCreatedCard.ActualHeight);
            RenderTargetBitmap canvasAsBitmap = new RenderTargetBitmap((int)canvasAsRectangle.Right, (int)canvasAsRectangle.Bottom, 60, 70, PixelFormats.Default);
            canvasAsBitmap.Render(userCreatedCard);

            //encode as PNG
            BitmapEncoder pngEncoder = new PngBitmapEncoder();
            pngEncoder.Frames.Add(BitmapFrame.Create(canvasAsBitmap));

            //save to memory stream
            using (MemoryStream ms = new MemoryStream())
            {
                pngEncoder.Save(ms);
                imageBytes = ms.ToArray();
            }

            return imageBytes;
        }

        /// <summary>
        /// Resets all the textboxes and textblocks to their default state.
        /// </summary>
        private void PrepareForNextCard()
        {
            ChangeTextBox(nameValue, Brushes.Gray, TextAlignment.Left, "Enter name: ");
            ChangeTextBox(manaValue, Brushes.Gray, TextAlignment.Left, "Enter manacost: ");
            ChangeTextBox(damageValue, Brushes.Gray, TextAlignment.Left, "Enter damage: ");
            ChangeTextBox(healthValue, Brushes.Gray, TextAlignment.Left, "Enter health: ");
            ChangeTextBox(abilityValue, Brushes.Gray, TextAlignment.Left, "Enter card ability: ");


            nameCard.Text = "";
            manaCard.Text = "";
            damageCard.Text = "";
            healthCard.Text = "";
            abilityCard.Text = "";
            userSelectedImage.Source = null;
        }
    }
}
