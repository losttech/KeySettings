namespace LostTech.App
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Windows.Input;
    using LostTech.App.Input;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class StrokeStingConversions
    {
        [TestMethod]
        public void ToStringEncodesPlus()
        {
            var stroke = new KeyStroke(Key.Add);
            Assert.AreEqual("Add", stroke.ToString());
            stroke.Modifiers = ModifierKeys.Alt;
            Assert.AreEqual("Alt+Add", stroke.ToString());
        }

        [TestMethod]
        public void ConverterCanParse()
        {
            var stroke = KeyStrokeValueConverter.Instance.ParseStroke("Ctrl+Alt+P+Plus");
            Assert.AreEqual(ModifierKeys.Alt | ModifierKeys.Control, stroke.Modifiers);
            CollectionAssert.AreEquivalent(new[] {Key.P, Key.OemPlus}, stroke.Keys);
        }
    }
}
