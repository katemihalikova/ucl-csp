using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Semestrálka
{
    /// <summary>
    /// Konverter pro operaci AND nad dalšími boolean convertery
    /// </summary>
    public class BooleanAndConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo language)
        {
            foreach (var value in values)
            {
                if ((value is bool) && (bool) value == false)
                    return false;
            }
            return true;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo language)
        {
            //converting back makes little sense since boolean values will not map back to objects.
            throw new NotImplementedException();
        }
    }
}
