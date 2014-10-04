using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XClip.Client.Views
{
    public interface IView
    {
        void ShowWindow();
        void CloseWindow();
    }
}
