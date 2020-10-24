using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Eksamen_PG5200_Card_Creator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static string cardsDatabaseName = "Cards.db";
        static string cardsFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        public static string cardsDatabasePath = System.IO.Path.Combine(cardsFolderPath, cardsDatabaseName);
 

        public static string cardTypesDatabasePath = "../../Resources/CardTypes.db";
        public static SolidColorBrush yellowBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#f5cf5e");

        
    }
}
