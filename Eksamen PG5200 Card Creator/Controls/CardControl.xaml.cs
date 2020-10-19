using Eksamen_PG5200_Card_Creator.Classes;
using System.Windows;
using System.Windows.Controls;

namespace Eksamen_PG5200_Card_Creator.Controls
{
    /// <summary>
    /// Interaction logic for CardControl.xaml
    /// </summary>
    public partial class CardControl : UserControl
    {

        public Card Card
        {
            get { return (Card)GetValue(CardProperty); }
            set { SetValue(CardProperty, value); }
        }

        public static readonly DependencyProperty CardProperty = DependencyProperty.Register("Card", typeof(Card), typeof(CardControl), new PropertyMetadata(null, SetText));


        private static void SetText(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CardControl control = d as CardControl;

            if (control != null)
            {
                control.cardNameTextBlock.Text = (e.NewValue as Card).cardName;
                control.cardAbilityTextBlock.Text = (e.NewValue as Card).cardAbility;
                control.manaCostTextBlock.Text = (e.NewValue as Card).manaCost;
                control.damageTextBlock.Text = (e.NewValue as Card).damage;
                control.healthTextBlock.Text = (e.NewValue as Card).health;
                control.cardTypeTextBlock.Text = (e.NewValue as Card).cardType;
            }

        }

        public CardControl()
        {
            InitializeComponent();
        }
    }
}
