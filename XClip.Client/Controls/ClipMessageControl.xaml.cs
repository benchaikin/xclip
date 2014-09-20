using System;
using System.Collections.Generic;
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
using XClip.Client.Communications;
using XClip.Client.Core;

namespace XClip.Client.Controls
{
    /// <summary>
    /// Interaction logic for ClipControl.xaml
    /// </summary>
    public partial class ClipMessageControl : UserControl
    {
        public ClipMessageControl()
        {
            InitializeComponent();
            DataContextChanged += ClipMessageControl_DataContextChanged;
        }

        void ClipMessageControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var renderMappings = new Dictionary<string, Func<IClipRenderer>>();
            renderMappings.Add(DataFormats.Rtf, () => new TextClipRenderer());
            renderMappings.Add(DataFormats.Text, () => new TextClipRenderer());
            renderMappings.Add(DataFormats.Dib, () => new ImageClipRenderer());


            ClipMessage message = e.NewValue as ClipMessage;
            if (message == null) return;

            IClipRenderer renderer = new DefaultClipRenderer();
            ClipObject clipObject = message.Payload.FirstOrDefault();

            foreach (var mapping in renderMappings)
            {
                if (message.Payload.Any(clip => clip.Format == mapping.Key))
                {
                    clipObject = message.Payload.First(clip => clip.Format == mapping.Key);
                    renderer = mapping.Value();
                    
                    break;
                }
            }

            _clipContainer.Content = renderer;
            renderer.LoadClipObject(clipObject);
        }
    }
}
