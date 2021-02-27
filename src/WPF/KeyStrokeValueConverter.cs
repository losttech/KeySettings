namespace LostTech.App
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Input;
    using LostTech.App.Input;

    public sealed class KeyStrokeValueConverter : IValueConverter
    {
        readonly KeyConverter keyConverter = new KeyConverter();
        readonly ModifierKeysConverter modifierConverter = new ModifierKeysConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // hardcoding here, as converter.CanConvertTo for some stupid reason requires instance of ITypeDescriptorContext
            if (targetType != typeof(string) && targetType != typeof(object))
                throw new NotSupportedException();

            if (value == null)
                return string.Empty;

            if (value is KeyStroke stroke)
                return stroke.ToString(culture);

            throw new NotSupportedException();
        }

        public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return DependencyProperty.UnsetValue;

            if (!(value is string keyString))
                throw new NotSupportedException();

            if (keyString == string.Empty)
                return null;

            return this.ParseStroke(keyString, culture) ?? Binding.DoNothing;
        }

        public KeyStroke? ParseStroke(string keyString, CultureInfo? culture = null)
        {
            string keys = keyString;
            ModifierKeys modifiers = ModifierKeys.None;
            for (int plusIndex = keyString.IndexOf('+');
                plusIndex >= 0 && plusIndex < keyString.Length;
                plusIndex = keyString.IndexOf('+', plusIndex + 1)) {
                string modifiersSubstring = keyString.Substring(0, plusIndex);
                if (this.modifierConverter.IsValid(modifiersSubstring)) {
                    modifiers = (ModifierKeys)this.modifierConverter.ConvertFromString(null, culture, modifiersSubstring);
                    keys = keyString.Substring(plusIndex + 1);
                }
                else
                    break;
            }
            var stroke = new KeyStroke {Modifiers = modifiers};
            foreach (string key in keys.Split('+')) {
                if (!this.keyConverter.IsValid(key))
                    return null;
                stroke.Keys.Add((Key)this.keyConverter.ConvertFromString(null, culture, key));
            }
            return stroke;
        }

        public static KeyStrokeValueConverter Instance { get; } = new KeyStrokeValueConverter();
    }
}
