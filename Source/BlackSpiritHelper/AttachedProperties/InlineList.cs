using BlackSpiritHelper.Core;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Xml;

namespace BlackSpiritHelper
{
    /// <summary>
    /// This attached property is for creating a <see cref="TextBlock"/> to be able to bind text with inline tags.
    /// </summary>
    class InlineList : BaseAttachedProperty<InlineList, string>
    {
        public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            // Get the text block.
            var tb = (sender as TextBlock);

            // Clear current textBlock
            tb.ClearValue(TextBlock.TextProperty);
            tb.Inlines.Clear();
            // Create new formatted text
            string formattedText = (string)e.NewValue ?? string.Empty;
            string @namespace = "http://schemas.microsoft.com/winfx/2006/xaml/presentation";
            formattedText = $@"<Span xml:space=""preserve"" xmlns=""{@namespace}"">{formattedText}</Span>";
            // Inject to inlines
            using (var xmlReader = XmlReader.Create(new StringReader(formattedText)))
            {
                try
                {
                    var result = (Span)System.Xaml.XamlServices.Load(xmlReader);
                    tb.Inlines.Add(result);
                }
                catch (Exception ex)
                {
                    // Error in XAML markdown
                    tb.Inlines.Add(formattedText);
                    IoC.Logger.Log($"{ex.GetType().ToString()}: {ex.Message}", LogLevel.Error);
                }
            }
        }
    }
}
