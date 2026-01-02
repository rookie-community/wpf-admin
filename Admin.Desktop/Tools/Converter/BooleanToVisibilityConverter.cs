using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Admin.Desktop.Tools.Converter
{
    /// <summary>
    /// Bool转Visibility
    /// </summary>
    /// <remarks>
    /// <para>True => <see cref="Visibility.Visible"/></para>
    /// <para>False => <see cref="Visibility.Collapsed"/></para>
    /// </remarks>
    [ValueConversion(typeof(bool), typeof(Visibility), ParameterType = typeof(bool))]
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(Visibility))
                throw new InvalidOperationException("The target must be a boolean");
            var val = (bool)value;
            _ = bool.TryParse($"{parameter}", out bool inverse);
            if (inverse)
            {
                val = !val;
            }
            return val ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Enum.TryParse(value.ToString(), true, out Visibility visibility))
            {
                _ = bool.TryParse($"{parameter}", out bool inverse);
                if (inverse)
                {
                    return visibility != Visibility.Visible;
                }
                return visibility == Visibility.Visible;
            }
            else
            {
                return false;
            }
        }
    }
}
