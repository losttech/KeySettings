namespace LostTech.App
{
    using System;
    using System.ComponentModel;
    using System.Globalization;

    public sealed class KeyStrokeTypeConverter: TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) {
            if (sourceType == typeof(string))
                return true;

            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture,
            object value) {
            switch (value) {
            case string str:
                return KeyStrokeValueConverter.Instance.ParseStroke(str, culture)
                    ?? throw new NotSupportedException("String does not represent a valid keystroke");
            default:
                return base.ConvertFrom(context, culture, value);
            }
        }

        public override bool IsValid(ITypeDescriptorContext context, object value) {
            return value == null
                   || (value is string str
                       && KeyStrokeValueConverter.Instance.ParseStroke(str) != null);
        }
    }
}
