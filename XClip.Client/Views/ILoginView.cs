using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XClip.Client.Views
{
    public interface ILoginView : IView
    {
        event Action CredentialsSubmitted;
        string Username { get; set; }
        string Password { get; }
        string ErrorMessage { set; }
        string RegisterUrl { set; }
        bool IsProcessing { set; }
    }
}
