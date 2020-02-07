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
    /// Boolean konverter, zda je hodnota null nebo ne
    /// </summary>
    public class ObjectNotNullConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo language)
        {
            return !object.Equals(value, null);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo language)
        {
            //converting back makes little sense since your boolean value will not map back to an object.
            throw new NotImplementedException();
        }
    }
}
