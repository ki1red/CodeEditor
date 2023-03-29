using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Lab_1.AuxiliaryClasses
{
    public class ColoredTextBox : RichTextBox
    {
        private Dictionary<string, Color> _replacer;
        private string _cachedText;

        public Dictionary<string, Color> TextHighlighter
        {
            get { return _replacer; }
            set
            {
                if (value != null)
                {
                    foreach (var pair in value)
                    {
                        if (pair.Key == null || pair.Value == null)
                        {
                            throw new ArgumentException("Dictionary contains null key or value");
                        }
                    }
                }

                _replacer = value;
                PainterText();
            }
        }

        public string Text
        {
            get { return _cachedText ?? GetClearText(); }
            set
            {
                value = value ?? "";

                Document.Blocks.Clear();
                Document.Blocks.Add(new Paragraph(new Run(value)));
                _cachedText = null;
                PainterText();
            }
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);

            _cachedText = null;

            PainterText();
        }

        private string GetClearText()
        {
            if (_cachedText == null)
            {
                var textRange = new TextRange(base.Document.ContentStart, base.Document.ContentEnd);
                _cachedText = textRange.Text;
            }

            return _cachedText;
        }

        private void PainterText()
        {
            Document.Blocks.Clear();
            var para = new Paragraph();

            if (TextHighlighter != null)
            {
                var text = GetClearText();
                var currentStartIndex = 0;

                foreach (var pair in TextHighlighter)
                {
                    var searchIndex = text.IndexOf(pair.Key, currentStartIndex);

                    if (searchIndex >= 0)
                    {
                        var beforeText = text.Substring(currentStartIndex, searchIndex - currentStartIndex);
                        para.Inlines.Add(beforeText);

                        var run = new Run(pair.Key)
                        {
                            Foreground = new SolidColorBrush(pair.Value)
                        };
                        para.Inlines.Add(run);

                        currentStartIndex = searchIndex + pair.Key.Length;
                    }
                }

                var afterText = text.Substring(currentStartIndex);
                para.Inlines.Add(afterText);
            }
            else
            {
                para.Inlines.Add(GetClearText());
                Document.Foreground = Brushes.Black;
            }

            Document.Blocks.Add(para);
        }
    }


}
