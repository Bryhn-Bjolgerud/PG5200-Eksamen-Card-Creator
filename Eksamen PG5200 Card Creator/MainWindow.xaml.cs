﻿using GalaSoft.MvvmLight.Command;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Eksamen_PG5200_Card_Creator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void makeCard_Click(object sender, RoutedEventArgs e)
        {
            manaCard.Text = manaValue.Text;
            damageCard.Text = damageValue.Text;
            healthCard.Text = healthValue.Text;
        }

        private void cardClassType_SelectionChanged(object sender, RoutedEventArgs e)
        {
            switch (cardClassType.SelectedItem.ToString())
            {
                case "System.Windows.Controls.ComboBoxItem: Death Knight":
                    changeBaseCard("deathKnight");
                    break;
                case "System.Windows.Controls.ComboBoxItem: Demon Hunter":
                    changeBaseCard("demonHunter");
                    break;
                case "System.Windows.Controls.ComboBoxItem: Druid":
                    changeBaseCard("druid");
                    break;
                case "System.Windows.Controls.ComboBoxItem: Hunter":
                    changeBaseCard("hunter");
                    break;
                case "System.Windows.Controls.ComboBoxItem: Mage":
                    changeBaseCard("mage");
                    break;
                case "System.Windows.Controls.ComboBoxItem: Neutral":
                    changeBaseCard("neutral");
                    break;
                case "System.Windows.Controls.ComboBoxItem: Paladin":
                    changeBaseCard("paladin");
                    break;
                case "System.Windows.Controls.ComboBoxItem: Priest":
                    changeBaseCard("priest");
                    break;
                case "System.Windows.Controls.ComboBoxItem: Rogue":
                    changeBaseCard("rogue");
                    break;
                case "System.Windows.Controls.ComboBoxItem: Shaman":
                    changeBaseCard("shaman");
                    break;
                case "System.Windows.Controls.ComboBoxItem: Warlock":
                    changeBaseCard("warlock");
                    break;
                case "System.Windows.Controls.ComboBoxItem: Warrior":
                    changeBaseCard("warrior");
                    break;
            }
        }

        private void changeBaseCard(String cardClass)
        {
            cardDisplay.Source = new BitmapImage(new Uri("Resources/classBaseCards/" + cardClass + "BaseCard.png", UriKind.Relative));
        }

        private void NameValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (nameValue.Text == "")
            {
                ImageBrush placeholderImage = new ImageBrush()
                {
                    Stretch = Stretch.None,
                    AlignmentX = AlignmentX.Left
                };
                placeholderImage.ImageSource = new BitmapImage(new Uri("Resources/TextboxPlaceholderImages/cardName.png", UriKind.RelativeOrAbsolute));
                nameValue.Background = placeholderImage;
            }
            else
            {
                nameValue.Background = null;
            }
        }

        private void ManaValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (manaValue.Text == "")
            {
                ImageBrush placeholderImage = new ImageBrush()
                {
                    Stretch = Stretch.None,
                    AlignmentX = AlignmentX.Left
                };
                placeholderImage.ImageSource = new BitmapImage(new Uri("Resources/TextboxPlaceholderImages/manaCost.png", UriKind.RelativeOrAbsolute));
                manaValue.Background = placeholderImage;
            }
            else
            {
                manaValue.Background = null;
            }
        }

        private void DamageValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (damageValue.Text == "")
            {
                ImageBrush placeholderImage = new ImageBrush()
                {
                    Stretch = Stretch.None,
                    AlignmentX = AlignmentX.Left
                };
                placeholderImage.ImageSource = new BitmapImage(new Uri("Resources/TextboxPlaceholderImages/damageDealt.png", UriKind.RelativeOrAbsolute));
                damageValue.Background = placeholderImage;
            }
            else
            {
                damageValue.Background = null;
            }
        }

        private void HealthValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (healthValue.Text == "")
            {
                ImageBrush placeholderImage = new ImageBrush()
                {
                    Stretch = Stretch.None,
                    AlignmentX = AlignmentX.Left
                };
                placeholderImage.ImageSource = new BitmapImage(new Uri("Resources/TextboxPlaceholderImages/health.png", UriKind.RelativeOrAbsolute));
                healthValue.Background = placeholderImage;
            }
            else
            {
                healthValue.Background = null;
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

            bool? error = openFilePrompt.ShowDialog();

            if (error == true)
            {
                string filePath = openFilePrompt.FileName;
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

        private void manaValue_GotFocus(object sender, RoutedEventArgs e)
        {
            manaValue.BorderBrush = Brushes.Gray;
            manaValue.TextAlignment = TextAlignment.Left;
            manaValue.Text = "";
        }

        private void manaValue_LostFocus(object sender, RoutedEventArgs e)
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

       

        //----------------------------------------------------------------------------------------------

    }
}