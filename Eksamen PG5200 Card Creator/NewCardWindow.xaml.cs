using Eksamen_PG5200_Card_Creator.Classes;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Eksamen_PG5200_Card_Creator
{
    /// <summary>
    /// Interaction logic for NewCardWindow.xaml
    /// </summary>
    public partial class NewCardWindow : Window
    {
        private List<CardType> m_cardTypes;
        private CardType m_selectedType;
        private List<TextBox> m_cardValueTextboxes = new List<TextBox>();
        private bool m_statsAppliedToCard;


        public NewCardWindow()
        {
            InitializeComponent();

            //Since a user can create new types, we need to make the comboBoxItems based on what is in the database.
            using (SQLiteConnection connection = new SQLiteConnection(App.cardTypesDatabasePath))
            {
                connection.CreateTable<CardType>();
                m_cardTypes = connection.Table<CardType>().ToList();
                foreach (CardType ct in m_cardTypes)
                {
                    cardTypeComboBox.Items.Add(ct.cardType);
                }
            }

            m_cardValueTextboxes.AddRange(new List<TextBox>
            {
                nameValue,
                manaValue,
                damageValue,
                healthValue,
                abilityValue

            });

            cardTypeComboBox.SelectedIndex = 6;
        }

       

        /// <summary>
        /// Displaying the correct base card depending on what type of card is chosen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CardTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            m_selectedType = m_cardTypes.Find(x => x.cardType.Equals(cardTypeComboBox.SelectedItem.ToString()));

            if (m_selectedType != null)
            {
                BitmapImage cardDisplaySrc = new BitmapImage();
                using (MemoryStream ms = new MemoryStream(m_selectedType.typeImage))
                {
                    cardDisplaySrc.BeginInit();
                    cardDisplaySrc.StreamSource = ms;
                    cardDisplaySrc.CacheOption = BitmapCacheOption.OnLoad;
                    cardDisplaySrc.EndInit();
                }
                cardDisplay.Source = cardDisplaySrc;
                ResetAllTextBoxAndBlockValues();
            }
        }

        /// <summary>
        /// Resets all the textboxes and textblocks to their default state.
        /// </summary>
        private void ResetAllTextBoxAndBlockValues()
        {
            SharedMethodsForWindows.ChangeTextBox(nameValue, Brushes.Gray, TextAlignment.Left, "Enter name:");
            SharedMethodsForWindows.ChangeTextBox(manaValue, Brushes.Gray, TextAlignment.Left, "Enter manacost:");
            SharedMethodsForWindows.ChangeTextBox(damageValue, Brushes.Gray, TextAlignment.Left, "Enter damage:");
            SharedMethodsForWindows.ChangeTextBox(healthValue, Brushes.Gray, TextAlignment.Left, "Enter health:");
            SharedMethodsForWindows.ChangeTextBox(abilityValue, Brushes.Gray, TextAlignment.Left, "Enter card ability:");

            nameCard.Text = "";
            manaCard.Text = "";
            damageCard.Text = "";
            healthCard.Text = "";
            abilityCard.Text = "";
            userSelectedImage.Source = null;
        }

        private void ResetTextbox_GotFocus(object sender, RoutedEventArgs e)
        {
            SharedMethodsForWindows.ChangeTextBox(e.Source as TextBox, Brushes.Green, TextAlignment.Left, "");
        }

        /// <summary>
        /// Checking if what the user typed in the textbox is within our rules. Changing the textbox accordingly.
        /// </summary>
        /// <param name="tb">The textbox we want to check.</param>
        /// <param name="selectedType">The type of card being created.</param>
        public static void CheckAndChangeNewCardTextBox(TextBox tb, CardType selectedType)
        {
            string defaultText = "";
            string wrongInputText = "";
            int maxCost = 0;

            switch (tb.Name)
            {
                case "nameValue":
                    defaultText = "Enter name:";
                    break;
                case "manaValue":
                    defaultText = "Enter manacost:";
                    wrongInputText = "Manacost has to be a number between 0 - " + selectedType.maxManaCost.ToString();
                    maxCost = selectedType.maxManaCost;
                    break;
                case "damageValue":
                    defaultText = "Enter damage:";
                    wrongInputText = "Damage has to be a number between 0 - " + selectedType.maxDamage.ToString();
                    maxCost = selectedType.maxDamage;
                    break;
                case "healthValue":
                    defaultText = "Enter health:";
                    wrongInputText = "Health has to be a number between 0 - " + selectedType.maxHealth.ToString();
                    maxCost = selectedType.maxHealth;
                    break;
                case "abilityValue":
                    defaultText = "Enter card ability:";
                    break;
            }

            if (tb.Text == "")
            {
                SharedMethodsForWindows.ChangeTextBox(tb, App.yellowBrush, TextAlignment.Left, defaultText);

            }
            //If the textbox being checked is one of the numerical stats, we validate the input.
            else if (tb.Name != "nameValue" && tb.Name != "abilityValue")
            {
                if (!App.isNumbers.IsMatch(tb.Text) || tb.Text.Length > 2 || Int64.Parse(tb.Text) > maxCost)
                {
                    SharedMethodsForWindows.ChangeTextBox(tb, Brushes.Red, TextAlignment.Right, wrongInputText);
                }
            }
        }

        /// <summary>
        /// Each time a textBox loses focus, we check if what the user typed in it is valid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangeTextBoxBasedOnInput_LostFocus(object sender, RoutedEventArgs e)
        {
            CheckAndChangeNewCardTextBox(e.Source as TextBox, m_selectedType);
        }

        /// <summary>
        /// Checking every textbox to see if their values are default or not.
        /// </summary>
        /// <returns>A bool representing whether or not the user have set any values in the textbox.</returns>
        private bool IsCardValuesNotDefault()
        {
            bool cardReady = true;

            foreach (TextBox tb in m_cardValueTextboxes)
            {
                if (tb == nameValue)
                {
                    if (nameValue.Text == "Enter name:")
                    {
                        cardReady = false;
                    }
                }
                else if (tb == abilityValue)
                {
                    if (abilityValue.Text == "Enter card ability:")
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
                m_statsAppliedToCard = true;
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

        /// <summary>
        /// All the code for dragging the image inside the portrait frame... Need refactoring.
        /// </summary>
        private bool m_isMoving;
        private Point? m_imagePosition;
        private double m_deltaX;
        private double m_deltaY;
        private TranslateTransform m_currentTT;

        /// <summary>
        /// Saving the position of the mouse relative to the images "coordinate system".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void userSelectedImageMoving_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (m_imagePosition == null)
            {
                m_imagePosition = userSelectedImage.TransformToAncestor(userSelectedImageContainer).Transform(new Point(0, 0));
            }
            var mousePosition = Mouse.GetPosition(userSelectedImageContainer);
            m_deltaX = mousePosition.X - m_imagePosition.Value.X;
            m_deltaY = mousePosition.Y - m_imagePosition.Value.Y;
            m_isMoving = true;
        }

        private void userSelectedImageMoving_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            m_currentTT = userSelectedImage.RenderTransform as TranslateTransform;
            m_isMoving = false;
        }

        /// <summary>
        /// Moving the image based on the mouses position. Have to include the position relative to the Grid the image is within, to account for the different "coordiante systems". 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void userSelectedImageMoving_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (!m_isMoving)
            {
                return;
            }

            var mousePoint = Mouse.GetPosition(userSelectedImageContainer);

            /* '?:' is a conditional operator. Works kinda like an if statement. Before '?' is the statement. After it is the two expressions seperated with ':'. The first of them is executed if the statement is 'true.
                var x;
                if(bool)
                {
                    x = a;
                } else {
                    x = b;
                }

            */
            var offsetX = (m_currentTT == null ? m_imagePosition.Value.X : m_imagePosition.Value.X - m_currentTT.X) + m_deltaX - mousePoint.X;
            var offsetY = (m_currentTT == null ? m_imagePosition.Value.Y : m_imagePosition.Value.Y - m_currentTT.Y) + m_deltaY - mousePoint.Y;

            userSelectedImage.RenderTransform = new TranslateTransform(-offsetX, -offsetY);
        }

        private void userSelectedImageMoving_MouseEnter(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.SizeAll;
        }

        private void userSelectedImageMoving_MouseLeave(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
            m_isMoving = false;
        }

        /// <summary>
        /// Taking a screengrab of an area and turning it into a .png.
        /// </summary>
        /// <returns>The contents of the screengrabbed area as an image.</returns>
        private BitmapEncoder CreatePNGFromCanvas()
        {
            //Every card image is of equal size, which is the size of the Canvas XAML tag.
            Rect canvasAsRectangle = new Rect(userCreatedCard.Margin.Left, userCreatedCard.Margin.Top, userCreatedCard.ActualWidth, userCreatedCard.ActualHeight);
            RenderTargetBitmap canvasAsBitmap = new RenderTargetBitmap((int)canvasAsRectangle.Right, (int)canvasAsRectangle.Bottom, 60, 70, PixelFormats.Default);

            canvasAsBitmap.Render(userCreatedCard);

            //encode as PNG
            BitmapEncoder pngEncoder = new PngBitmapEncoder();
            pngEncoder.Frames.Add(BitmapFrame.Create(canvasAsBitmap));
            return pngEncoder;
        }

        /// <summary>
        /// Creating a new card object and setting it's values corresponding to what the user typed. Then adding it to the database. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveCard_Click(object sender, RoutedEventArgs e)
        {
            if (m_statsAppliedToCard)
            {
                Card newCard = new Card()
                {
                    cardName = nameCard.Text,
                    cardType = cardTypeComboBox.SelectedItem.ToString(),
                    manaCost = manaCard.Text,
                    damage = damageCard.Text,
                    health = healthCard.Text,
                    cardAbility = abilityCard.Text,
                    cardImageAsBytes = SharedMethodsForWindows.ConvertImageToBytes(CreatePNGFromCanvas())
                };

                using (SQLiteConnection connection = new SQLiteConnection(App.cardsDatabasePath))
                {
                    connection.CreateTable<Card>();
                    connection.Insert(newCard);
                }

                m_statsAppliedToCard = false;
                ResetAllTextBoxAndBlockValues();
                cardTypeComboBox.SelectedIndex = 6;
                MessageBox.Show("Your card was created successfully!");
            }
            else
            {
                MessageBox.Show("Give the card some stats!");
            }
        }
    }
}
