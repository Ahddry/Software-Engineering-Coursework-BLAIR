using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private void HomeClick()
        {
            ContentControlBinding = new HomeView();
            OnChanged(nameof(ContentControlBinding));
        }

        private void NewMessageClick()
        {
            ContentControlBinding = new NewMessageView();
            OnChanged(nameof(ContentControlBinding));
        }

        private void ImportMessageClick()
        {
            ContentControlBinding = new ImportMessagesView();
            OnChanged(nameof(ContentControlBinding));
        }

        private void ReadMessageClick()
        {
            ContentControlBinding = new ReadMessagesView();
            OnChanged(nameof(ContentControlBinding));
        }

    }
}