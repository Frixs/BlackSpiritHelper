using BlackSpiritHelper.Core;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

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
            try
            {
                var result = (Span)System.Xaml.XamlServices.Parse(formattedText);
                SetHyperlinks(result.Inlines);
                tb.Inlines.Add(result);
            }
            catch (Exception ex)
            {
                // Error in XAML markdown
                tb.Inlines.Add(formattedText);
                IoC.Logger.Log($"{ex.GetType().ToString()}: {ex.Message}", LogLevel.Error);
            }
        }

        /// <summary>
        /// Additionally set click event for hypertexts.
        /// </summary>
        /// <param name="inlines"></param>
        private void SetHyperlinks(InlineCollection inlines)
        {
            foreach (Inline inline in inlines)
            {
                var type = inline.GetType();
                if (type.Equals(typeof(Hyperlink)))
                    ((Hyperlink)inline).Click += new RoutedEventHandler(Hyperlink_Click);

                // Find hypertexts in sub-inlines - recurse.
                var ins = GetInlines(inline);
                if (ins != null)
                    SetHyperlinks(ins);
            }
        }

        /// <summary>
        /// Check the inline type and return child inline collection or null bysed on it.
        /// </summary>
        /// <param name="inline"></param>
        /// <returns></returns>
        private InlineCollection GetInlines(Inline inline)
        {
            var type = inline.GetType();

            if (type.Equals(typeof(Span)))
                return ((Span)inline).Inlines;

            else if (type.Equals(typeof(Bold)))
                return ((Bold)inline).Inlines;

            else if (type.Equals(typeof(Underline)))
                return ((Underline)inline).Inlines;

            else if (type.Equals(typeof(Italic)))
                return ((Italic)inline).Inlines;

            return null;
        }

        void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo(((Hyperlink)sender).NavigateUri.AbsoluteUri));
            e.Handled = true;
        }
    }
}
