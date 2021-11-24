using Coursework1.Core;
using Coursework1.UI.View;
using System.Windows.Controls;
using System.Windows.Input;

namespace Coursework1.UI.View_Models
{
    /// <summary>
    /// Represents Main Window view model with its controls.
    /// </summary>
    public class MainWindowViewModel : BaseViewModel
    {
        //Commands for the menu buttons
        public new ICommand HomeCommand { get; private set; }
        public new ICommand NewMessageCommand { get; private set; }
        public new ICommand ImportMessagesCommand { get; private set; }
        public new ICommand ReadMessagesCommand { get; private set; }
        public new ICommand StatisticsCommand { get; private set; }
        //Current side user control
        public new UserControl ContentControlBinding { get; private set; }
        /// <summary>
        /// Represents Main Window view model with its controls.
        /// </summary>
        public MainWindowViewModel()
        {
            HomeCommand = new RelayCommand(HomeClick);
            NewMessageCommand = new RelayCommand(NewMessageClick);
            ImportMessagesCommand = new RelayCommand(ImportMessageClick);
            ReadMessagesCommand = new RelayCommand(ReadMessageClick);
            StatisticsCommand = new RelayCommand(StatisticsClick);

            ContentControlBinding = new HomeView();
        }
        /// <summary>
        /// Switch to the view placed in parameter.
        /// </summary>
        /// <param name="parameter">View to change to.</param>
        public new void ChangeView(object parameter)
        {
            ContentControlBinding = (UserControl)parameter;
            OnChanged(nameof(ContentControlBinding));
        }
        /// <summary>
        /// Switch to the Home View.
        /// </summary>
        private void HomeClick()
        {
            ChangeView(new HomeView());
        }
        /// <summary>
        /// Switch to the New Message View.
        /// </summary>
        private void NewMessageClick()
        {
            ChangeView(new NewMessageView());
        }
        /// <summary>
        /// Switch to the Import Message View.
        /// </summary>
        private void ImportMessageClick()
        {
            ChangeView(new ImportMessagesView());
        }
        /// <summary>
        /// Switch to Read Message View.
        /// </summary>
        private void ReadMessageClick()
        {
            ChangeView(new ReadMessagesView());
        }
        /// <summary>
        /// Switch to the Statistics View.
        /// </summary>
        private void StatisticsClick()
        {
            ChangeView(new StatisticsView());
        }
    }
}