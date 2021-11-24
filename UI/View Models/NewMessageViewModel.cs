using Coursework1.Core;
using Coursework1.UI.View;
using System.Windows.Controls;
using System.Windows.Input;

namespace Coursework1.UI.View_Models
{
    /// <summary>
    /// Intermediary view model to choose between two others.
    /// </summary>
    public class NewMessageViewModel : MainWindowViewModel
    {
        #region Button
        public string SimpleButtonText { get; private set; }
        public string AdvancedButtonText { get; private set; }

        public new ICommand SimpleButtonCommand { get; private set; }
        public new ICommand AdvancedButtonCommand { get; private set; }
        public new UserControl ContentControlBinding { get; private set; }
        #endregion
        /// <summary>
        /// Intermediary view model to choose between two others.
        /// </summary>
        public NewMessageViewModel()
        {
            SimpleButtonText = "Simple";
            AdvancedButtonText = "Advanced";

            SimpleButtonCommand = new RelayCommand(SimpleButtonClick);
            AdvancedButtonCommand = new RelayCommand(AdvancedButtonClick);
        }
        /// <summary>
        /// Switch to the New Message Simple View.
        /// </summary>
        private void SimpleButtonClick()
        {
            ContentControlBinding = new NMSimpleView();
            OnChanged(nameof(ContentControlBinding));
        }
        /// <summary>
        /// Switch to the New Message Advanced View.
        /// </summary>
        private void AdvancedButtonClick()
        {
            ContentControlBinding = new NMAdvancedView();
            OnChanged(nameof(ContentControlBinding));
        }
    }
}
