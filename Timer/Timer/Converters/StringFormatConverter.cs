using Microsoft.UI.Xaml.Data;

namespace Timer.Converters;
public sealed class StringFormatConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        var format = parameter as string;
        return !string.IsNullOrEmpty(format) ? string.Format(format, value) : value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
}
