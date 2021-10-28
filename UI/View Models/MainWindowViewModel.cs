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
    public class MainWindowViewModel : BaseViewModel
    {
        public new ICommand HomeCommand { get; private set; }
        public new ICommand NewMessageCommand { get; private set; }
        public new ICommand ImportMessagesCommand { get; private set; }
        public new ICommand ReadMessagesCommand { get; private set; }

        public new UserControl ContentControlBinding { get; private set; }

        public MainWindowViewModel()
        {
            HomeCommand = new RelayCommand(HomeClick);
            NewMessageCommand = new RelayCommand(NewMessageClick);
            ImportMessagesCommand = new RelayCommand(ImportMessageClick);
            ReadMessagesCommand = new RelayCommand(ReadMessageClick);

            ContentControlBinding = new HomeView();
        }

        public new void ChangeView(object parameter)
        {
            ContentControlBinding = (UserControl)parameter;
            OnChanged(nameof(ContentControlBinding));
        }

        private void HomeClick()
        {
            ChangeView(new HomeView());
        }

        private void NewMessageClick()
        {
            ChangeView(new NewMessageView());
        }

        private void ImportMessageClick()
        {
            ChangeView(new ImportMessagesView());
        }

        private void ReadMessageClick()
        {
            ChangeView(new ReadMessagesView());
        }

        public void SimpleButtonTamere()
        {
            ContentControlBinding = new HomeView();
            OnChanged(nameof(ContentControlBinding));
            //SimpleButtonText = "TA MERE";
        }
    }
}