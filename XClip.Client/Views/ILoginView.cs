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
        string Username { get; }
        string Password { get; }
        string ErrorMessage { set; }
        bool IsProcessing { set; }
    }
}
