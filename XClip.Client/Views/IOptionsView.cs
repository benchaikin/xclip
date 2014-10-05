using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XClip.Client.Properties;

namespace XClip.Client.Views
{
    public interface IOptionsView : IView
    {
        event Action Save;
        event Action Cancel;
        Settings Model { get; set; }
    }
}
