using Coursework1.Core;
using Coursework1.UI.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Coursework1.UI.View_Models
{
    public class NMSimpleViewModel : BaseViewModel
    {
        public ICommand SMSCommand { get; private set; }
        public ICommand EmailCommand { get; private set; }
        public ICommand TweetCommand { get; private set; }
        public new UserControl ContentControlBinding { get; private set; }
        public NMSimpleViewModel()
        {
            SMSCommand = new RelayCommand(SMSButtonClick);
            EmailCommand = new RelayCommand(EmailButtonClick);
            TweetCommand = new RelayCommand(TweetButtonClick);
        }

        public void SMSButtonClick()
        {
            ContentControlBinding = new SMSCreationView();
            OnChanged(nameof(ContentControlBinding));
        }

        public void EmailButtonClick()
        {
            ContentControlBinding = new EmailCreationView();
            OnChanged(nameof(ContentControlBinding));
        }

        public void TweetButtonClick()
        {
            ContentControlBinding = new TweetCreationView();
            OnChanged(nameof(ContentControlBinding));
        }
    }
}
