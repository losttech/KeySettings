namespace LostTech.App
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Input;

    public sealed class KeyGestureValueConverter : IValueConverter
    {
        readonly KeyGestureConverter converter = new KeyGestureConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // hardcoding here, as converter.CanConvertTo for some stupid reason requires instance of ITypeDescriptorContext
            if (targetType != typeof(string))
                throw new NotSupportedException();

            return this.converter.ConvertTo(null, culture, value, targetType);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return DependencyProperty.UnsetValue;

            if (!this.converter.CanConvertFrom(value.GetType()))
                throw new NotSupportedException();

            return this.converter.ConvertFrom(null, culture, value);
        }
    }
}
