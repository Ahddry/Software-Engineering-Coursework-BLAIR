using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace Coursework1.UI.View_Models
{
    /// <summary>
    /// Represents the base of all the view models with their shared controls
    /// </summary>
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void ChangeView(object parameter)
        {
            ContentControlBinding = (UserControl)parameter;
            OnChanged(nameof(ContentControlBinding));
        }
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        /// <param name="name">Name of the value</param>
        public void OnChanged(string name)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
        //Had to put this command here as well and overwrite them afterward, otherwise the would not work
        public ICommand HomeCommand { get; private set; }
        public ICommand NewMessageCommand { get; private set; }
        public ICommand ImportMessagesCommand { get; private set; }
        public ICommand ReadMessagesCommand { get; private set; }
        public ICommand StatisticsCommand { get; private set; }
        public ICommand SimpleButtonCommand { get; private set; }
        public ICommand AdvancedButtonCommand { get; private set; }

        public UserControl ContentControlBinding { get; private set; } //Current user control
    }
}
