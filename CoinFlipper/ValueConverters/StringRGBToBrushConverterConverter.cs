using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace CoinFlipper
{
    /// <summary>
    /// A converter that takes in an RGB string and converts it to a WPF brush
    /// </summary>
    public class StringRGBToBrushConverterConverter : BaseValueConverter<StringRGBToBrushConverterConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (SolidColorBrush)(new BrushConverter().ConvertFrom($"#{value}"));
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
