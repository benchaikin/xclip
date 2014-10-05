using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XClip.Client.Properties;
using XClip.Client.Views;

namespace XClip.Client.Controllers
{
    public class OptionsController : IOptionsController
    {
        private readonly IOptionsView _view;

        public OptionsController(IOptionsView view)
        {
            _view = view;
            _view.Cancel += OnCancel;
            _view.Save += OnSave;
        }

        public void Show()
        {
            _view.ShowWindow();
            _view.Model = Settings.Default;
        }

        private void OnCancel()
        {
            Settings.Default.Reload();
        }

        private void OnSave()
        {
            Settings.Default.Save();
            _view.CloseWindow();
        }
    }
}
