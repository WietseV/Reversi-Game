using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using Model.Reversi;

namespace View
{
    public class OwnerToColorConverter : IValueConverter
    {
        //coverter to get the color from a player
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Player owner = value as Player;
            if (owner == Player.BLACK)
            {
                return Brushes.Black;
            }
            if (owner == Player.WHITE)
            {
                return Brushes.White;
            }
            else { return Brushes.Transparent;}
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
