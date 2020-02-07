using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Semestrálka
{
    /// <summary>
    /// Validace podle délky řetězce
    /// </summary>
    public class StringLengthValidationRule : ValidationRule
    {
        public int MinLength { get; set; }
        public int MaxLength { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var strValue = Convert.ToString(value);

            if (strValue.Length < MinLength)
                return new ValidationResult(false, $"Value is too short (has {strValue.Length}, min limit is {MinLength})");
            else if (strValue.Length > MaxLength)
                return new ValidationResult(false, $"Value is too long (has {strValue.Length}, max limit is {MaxLength})");
            else
                return new ValidationResult(true, null);
        }
    }

    /// <summary>
    /// Validace podle obsahu řetězce
    /// </summary>
    public class StringContentValidationRule : ValidationRule
    {
        public string Chars { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var strValue = Convert.ToString(value);
            var regex = new Regex($"^[{Chars}]*$");

            if (!regex.IsMatch(strValue))
                return new ValidationResult(false, $"Value contains disallowed characters (allowed are {Chars})");
            else
                return new ValidationResult(true, null);
        }
    }

    /// <summary>
    /// Validace čísel podle minima a maxima
    /// </summary>
    public class NumberValidationRule : ValidationRule
    {
        public int Min { get; set; }
        public int Max { get; set; }
        public NumberBinder MaxBound { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var strValue = Convert.ToString(value);
            int intValue;
            try
            {
                intValue = int.Parse(strValue);
            }
            catch (Exception)
            {
                return new ValidationResult(false, $"Value should be number");
            }

            var max = MaxBound == null ? Max : MaxBound.Value;

            if (intValue < Min)
                return new ValidationResult(false, $"Value is too small (is {intValue}, min is {Min})");
            else if (intValue > max)
                return new ValidationResult(false, $"Value is too large (is {intValue}, max is {max})");
            else
                return new ValidationResult(true, null);
        }
    }

    /// <summary>
    /// Validace času podle formátu
    /// </summary>
    public class TimeValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var strValue = Convert.ToString(value);
            var regex = new Regex(@"^([01]\d|2[0-3]):[0-5]\d$");

            if (!regex.IsMatch(strValue))
                return new ValidationResult(false, $"Value should be time (e.g. 14:30)");
            else
                return new ValidationResult(true, null);
        }
    }

    /// <summary>
    /// Validace času tak, aby se čas nenacházel v době jiné rezervace
    /// </summary>
    public class OverlapValidationRule : ValidationRule
    {
        public string Mode { get; set; }
        public OverlapValidationBinder ReservationsBound { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var time = Convert.ToString(value);

            var conflictingReservations = Mode == "from"
                ? ReservationsBound.Value.Where(reservation => string.Compare(reservation.TimeFrom, time) <= 0 && string.Compare(reservation.TimeTo, time) > 0).ToList()
                : ReservationsBound.Value.Where(reservation => string.Compare(reservation.TimeFrom, time) < 0 && string.Compare(reservation.TimeTo, time) >= 0).ToList();

            if (conflictingReservations.Count() > 0)
                return new ValidationResult(false, $"Conflict with reservation {conflictingReservations[0].TimeFrom} - {conflictingReservations[0].TimeTo}");
            else
                return new ValidationResult(true, null);
        }
    }

    public class OverlapValidationBinder : DependencyObject
    {
        public static readonly DependencyProperty Property = DependencyProperty.Register("Value", typeof(IEnumerable<Reservation>), typeof(OverlapValidationBinder), new FrameworkPropertyMetadata(null));

        public IEnumerable<Reservation> Value
        {
            get { return (IEnumerable<Reservation>) GetValue(Property); }
            set { SetValue(Property, value); }
        }
    }

    /// <summary>
    /// Validace časů tak, aby se uvnitř nové rezervace nenacházela jiná již existující
    /// </summary>
    public class FromToOverlapValidationRule : ValidationRule
    {
        public OverlapValidationBinder ReservationsBound { get; set; }
        public StringBinder FromBound { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var timeTo = Convert.ToString(value);
            var timeFrom = FromBound.Value;

            if (string.Compare(timeFrom, timeTo) > 0)
                return new ValidationResult(false, $"Time from {timeFrom} must be earlier than time to {timeTo}");

            var conflictingReservations = ReservationsBound.Value.Where(reservation => string.Compare(reservation.TimeFrom, timeFrom) >= 0 && string.Compare(reservation.TimeTo, timeTo) <= 0).ToList();

            if (conflictingReservations.Count() > 0)
                return new ValidationResult(false, $"Conflict with reservation {conflictingReservations[0].TimeFrom} - {conflictingReservations[0].TimeTo}");
            else
                return new ValidationResult(true, null);
        }
    }

    public class StringBinder : DependencyObject
    {
        public static readonly DependencyProperty Property = DependencyProperty.Register("Value", typeof(string), typeof(StringBinder), new FrameworkPropertyMetadata(null));

        public string Value
        {
            get { return (string) GetValue(Property); }
            set { SetValue(Property, value); }
        }
    }

    public class NumberBinder : DependencyObject
    {
        public static readonly DependencyProperty Property = DependencyProperty.Register("Value", typeof(int), typeof(NumberBinder), new FrameworkPropertyMetadata(0));

        public int Value
        {
            get { return (int) GetValue(Property); }
            set { SetValue(Property, value); }
        }
    }

    public class BindingProxy : Freezable
    {
        protected override Freezable CreateInstanceCore()
        {
            return new BindingProxy();
        }

        public object Data
        {
            get { return GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        public static readonly DependencyProperty DataProperty = DependencyProperty.Register("Data", typeof(object), typeof(BindingProxy), new PropertyMetadata(null));
    }
}
