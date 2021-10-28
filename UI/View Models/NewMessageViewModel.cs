using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Coursework1.Core;
using Coursework1.UI.View;

namespace Coursework1.UI.View_Models
{
    public class NewMessageViewModel : MainWindowViewModel
    {
        #region Button
        public string SimpleButtonText { get; private set; }
        public string AdvancedButtonText { get; private set; }

        public new ICommand SimpleButtonCommand { get; private set; }
        public new ICommand AdvancedButtonCommand { get; private set; }
        public new UserControl ContentControlBinding { get; private set; }
        #endregion

        public NewMessageViewModel()
        {
            SimpleButtonText = "Simple";
            AdvancedButtonText = "Advanced";

            SimpleButtonCommand = new RelayCommand(SimpleButtonClick);
            AdvancedButtonCommand = new RelayCommand(AdvancedButtonClick);
        }

        private void SimpleButtonClick()
        {
            ContentControlBinding = new NMSimpleView();
            OnChanged(nameof(ContentControlBinding));
        }

        private void AdvancedButtonClick()
        {
            ContentControlBinding = new NMAdvancedView();
            OnChanged(nameof(ContentControlBinding));
        }
    }
}
