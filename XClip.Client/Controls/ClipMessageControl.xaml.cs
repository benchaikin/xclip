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
using XClip.Core;

namespace XClip.Client.Controls
{
    /// <summary>
    /// Interaction logic for ClipControl.xaml
    /// </summary>
    public partial class ClipMessageControl : UserControl
    {
        private Dictionary<string, Func<IClipRenderer>> _renderMappings;

        public ClipMessageControl()
        {
            _renderMappings = new Dictionary<string, Func<IClipRenderer>>();
            _renderMappings.Add(DataFormats.Rtf, () => new TextClipRenderer());
            _renderMappings.Add(DataFormats.Text, () => new TextClipRenderer());
            _renderMappings.Add(DataFormats.Dib, () => new ImageClipRenderer());

            InitializeComponent();
            DataContextChanged += ClipMessageControl_DataContextChanged;
        }

        void ClipMessageControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Clip message = e.NewValue as Clip;
            if (message == null) return;

            IClipRenderer renderer = new DefaultClipRenderer();
            ClipObject clipObject = message.ClipObjects.FirstOrDefault();

            foreach (var mapping in _renderMappings)
            {
                if (message.ClipObjects.Any(clip => clip.Format == mapping.Key))
                {
                    clipObject = message.ClipObjects.First(clip => clip.Format == mapping.Key);
                    renderer = mapping.Value();
                    
                    break;
                }
            }

            _clipContainer.Content = renderer;
            renderer.LoadClipObject(clipObject);
        }
    }
}
