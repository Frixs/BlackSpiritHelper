using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Represents notification context of notification UI.
    /// List of all notifications and calling notifications can be found in <see cref="IoC.UI"/>
    /// </summary>
    public class NotificationBoxDialogViewModel : BaseViewModel
    {
        #region Private Members

        /// <summary>
        /// Message context
        /// </summary>
        private string mMessage = "Message placeholder!";

        #endregion

        #region Public Properties

        /// <summary>
        /// Notification Title
        /// </summary>
        public string Title { get; set; } = "NOTIFICATION";

        /// <summary>
        /// Should be message formatted?
        /// </summary>
        public bool MessageFormatting { get; set; } = false;

        /// <summary>
        /// Message context
        /// </summary>
        public string Message
        {
            get => mMessage;
            set => mMessage = MessageFormatting ? Format(value) : value;
        }

        /// <summary>
        /// Type of the button layout
        /// </summary>
        public NotificationBoxResult Result { get; set; } = NotificationBoxResult.Ok;

        /// <summary>
        /// Run this action on <see cref="NotificationBoxResult.Ok"/> successful result from notification box.
        /// </summary>
        public Action OkAction { get; set; } = delegate { };

        /// <summary>
        /// Run this action on <see cref="NotificationBoxResult.YesNo"/> positive - Yes - result from notification box.
        /// </summary>
        public Action YesAction { get; set; } = delegate { };

        /// <summary>
        /// Run this action on <see cref="NotificationBoxResult.YesNo"/> negative - No - result from notification box.
        /// </summary>
        public Action NoAction { get; set; } = delegate { };

        #endregion

        #region Command Flags

        private bool mActionCommandFlag { get; set; }

        #endregion

        #region Commands

        /// <summary>
        /// Command to run OK event.
        /// </summary>
        public ICommand OkCommand { get; set; }

        /// <summary>
        /// Command to run OK event.
        /// </summary>
        public ICommand YesCommand { get; set; }

        /// <summary>
        /// Command to run OK event.
        /// </summary>
        public ICommand NoCommand { get; set; }

        /// <summary>
        /// Command to run hyperlink.
        /// </summary>
        public ICommand HyperlinkCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public NotificationBoxDialogViewModel()
        {
            // Create commands.
            CreateCommands();
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Create commands.
        /// </summary>
        private void CreateCommands()
        {
            OkCommand = new RelayCommand(async () => await OkCommandMethodAsync());
            YesCommand = new RelayCommand(async () => await YesCommandMethodAsync());
            NoCommand = new RelayCommand(async () => await NoCommandMethodAsync());
            HyperlinkCommand = new RelayParameterizedCommand(async (parameter) => await HyperlinkCommandCommandMethodAsync(parameter));
        }

        /// <summary>
        /// Open hyperlink
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private async Task HyperlinkCommandCommandMethodAsync(object parameter)
        {
            await RunCommandAsync(() => mActionCommandFlag, async () =>
            {
                try
                {
                    System.Diagnostics.Process.Start((string)parameter);
                }
                catch (Exception)
                {
                    //; Invalid URL
                }

                await Task.Delay(1);
            });
        }

        /// <summary>
        /// NO command routine
        /// </summary>
        /// <returns></returns>
        private async Task NoCommandMethodAsync()
        {
            await RunCommandAsync(() => mActionCommandFlag, async () =>
            {
                NoAction();

                Remove();

                await Task.Delay(1);
            });
        }

        /// <summary>
        /// YES command routine
        /// </summary>
        /// <returns></returns>
        private async Task YesCommandMethodAsync()
        {
            await RunCommandAsync(() => mActionCommandFlag, async () =>
            {
                YesAction();

                Remove();

                await Task.Delay(1);
            });
        }

        /// <summary>
        /// OK command routine
        /// </summary>
        /// <returns></returns>
        private async Task OkCommandMethodAsync()
        {
            await RunCommandAsync(() => mActionCommandFlag, async () =>
            {
                OkAction();

                Remove();

                await Task.Delay(1);
            });
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Remove this instance of notification box from the list of notifications.
        /// </summary>
        private void Remove()
        {
            IoC.UI.NotificationArea.RemoveNotification(this);
        }

        /// <summary>
        /// Format the text by the rules described in patch notes file in the release directory.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private string Format(string text)
        {
            var lines = Regex.Split(text, "\r\n|\r|\n");
            var formattedLines = new List<string>();

            for (int i = 0; i < lines.Length; i++)
            {
                var formattedLine = FormatLineIntoXaml(lines[i]);

                if (!formattedLine.Equals(string.Empty))
                    formattedLines.Add(formattedLine);
            }

            return string.Join(Environment.NewLine, formattedLines);
        }

        /// <summary>
        /// Format string into xaml version.
        /// There is several tags:
        /// Span, Bold, Italic, Underline, Run and LineBreak.
        /// If you want add a new one, check InlineList attacked property definition.
        /// Hypertext has additional binding there. It needs to know all tags which can contain Inlines as childs.
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private string FormatLineIntoXaml(string line)
        {
            // Empty line.
            if (line.Equals(string.Empty))
            {
                return string.Empty;
            }

            // Line break/skip
            if (line.StartsWith("<br/>"))
            {
                line = @"<LineBreak />";
            }
            // Header 2
            else if (line.StartsWith("## "))
            {
                line = line.Replace("## ", "");
                line = $@"<Run Text=""{line}"" FontFamily=""{{StaticResource LatoHeavy}}"" TextDecorations=""Underline"" FontSize=""20"" /><LineBreak />";
            }
            // Header 3
            else if (line.StartsWith("### "))
            {
                line = line.Replace("### ", "");
                line = $@"<LineBreak /><Run Text=""{line}"" FontFamily=""{{StaticResource LatoHeavy}}"" FontSize=""18"" />";
            }
            // Header 4
            else if (line.StartsWith("#### "))
            {
                line = line.Replace("#### ", "");
                line = $@"<LineBreak /><Run Text=""{line}"" FontFamily=""{{StaticResource LatoHeavy}}"" FontSize=""16"" />";
            }

            // Comment - Line comments only!!!
            if (line.StartsWith("<!-- "))
            {
                return string.Empty;
            }

            // Hyperlink
            // Hyperlinks does not fire opening event for internet browser by default.
            // There is additional click event binding in InlineList attached property.
            #region Hyperlink
            Regex regex = new Regex(@"\[(?<text>.*?)\]\((?<link>.*?)\)", RegexOptions.Compiled);
            foreach (Match match in regex.Matches(line))
            {
                string text = match.Groups["text"].Value;
                int text_start = match.Groups["text"].Index;
                string link = match.Groups["link"].Value;

                int len = text.Length + link.Length + 4;

                var aStringBuilder = new StringBuilder(line);
                aStringBuilder.Remove(text_start - 1, len);
                aStringBuilder.Insert(text_start - 1, $@"<Hyperlink NavigateUri=""{link}"" ToolTip=""{link}"" FontFamily=""{{StaticResource LatoHeavy}}"" Foreground=""{{StaticResource RedForegroundAltBrushKey}}"" TextDecorations=""Underline"">{text}</Hyperlink>");

                line = aStringBuilder.ToString();
            }
            #endregion

            // Bold Text
            #region Bold Text
            bool bBold = false;
            for (int i = 0; i < line.Length; i++)
            {
                // Contains star?
                if (line[i].Equals('*'))
                {
                    // Escape.
                    if (i > 0 && line[i - 1].Equals(@"\"))
                    {
                        continue;
                    }

                    // Contains next char star?
                    if (i + 1 < line.Length && line[i + 1].Equals('*'))
                    {
                        var aStringBuilder = new StringBuilder(line);
                        aStringBuilder.Remove(i, 2);
                        if (bBold)
                        {
                            aStringBuilder.Insert(i, @"</Bold>");
                        }
                        else
                        {
                            aStringBuilder.Insert(i, @"<Bold>");
                        }
                        line = aStringBuilder.ToString();

                        bBold = !bBold;
                    }
                }
            }
            // If there is missing end tag
            if (bBold)
            {
                line += @"</Bold>";
            }
            #endregion

            // Underline Text
            #region Underline Text
            bool bUnderline = false;
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i].Equals('_'))
                {
                    // Escape.
                    if (i > 0 && line[i - 1].Equals(@"\"))
                    {
                        continue;
                    }

                    if (i + 1 < line.Length && line[i + 1].Equals('_'))
                    {
                        var aStringBuilder = new StringBuilder(line);
                        aStringBuilder.Remove(i, 2);
                        if (bUnderline)
                        {
                            aStringBuilder.Insert(i, @"</Underline>");
                        }
                        else
                        {
                            aStringBuilder.Insert(i, @"<Underline>");
                        }
                        line = aStringBuilder.ToString();

                        bUnderline = !bUnderline;
                    }
                }
            }
            if (bUnderline)
            {
                line += @"</Underline>";
            }
            #endregion

            // Italic Text
            #region Italic Text
            bool bItalic = false;
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i].Equals('_'))
                {
                    // Escape.
                    if (i > 0 && line[i - 1].Equals(@"\"))
                    {
                        continue;
                    }

                    var aStringBuilder = new StringBuilder(line);
                    aStringBuilder.Remove(i, 1);
                    if (bItalic)
                    {
                        aStringBuilder.Insert(i, @"</Italic>");
                    }
                    else
                    {
                        aStringBuilder.Insert(i, @"<Italic>");
                    }
                    line = aStringBuilder.ToString();

                    bItalic = !bItalic;
                }
            }
            if (bItalic)
            {
                line += @"</Italic>";
            }
            #endregion

            return line;
        }

        #endregion
    }
}
