using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using XClip.Core;

namespace XClip.Client.Controls
{
    /// <summary>
    /// Interaction logic for TextClipRenderer.xaml
    /// </summary>
    public partial class TextClipRenderer : UserControl, IClipRenderer
    {
        public TextClipRenderer()
        {
            InitializeComponent();
        }

        public void LoadClipObject(ClipObject clipObject)
        {
            string text = new DataObjectConverter().GetClipObjectData(clipObject).ToString();
            TextRange range;

            using (MemoryStream stream = new MemoryStream(ASCIIEncoding.ASCII.GetBytes(text)))
            {
                range = new TextRange(_text.Document.ContentStart, _text.Document.ContentEnd);
                range.Load(stream, clipObject.Format);
            }
        }
    }
}
