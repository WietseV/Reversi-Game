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
    public class PlayerToStringConverter : IValueConverter
    {
        //coverter to get the color from a player
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Player owner = value as Player;
            if (owner == Player.BLACK)
            {
                return GameInfo.GetPlayerInfo(owner).Player1 ;
            }
            if (owner == Player.WHITE)
            {
                return GameInfo.GetPlayerInfo(owner).Player2;
            }
            else { return "";}
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
