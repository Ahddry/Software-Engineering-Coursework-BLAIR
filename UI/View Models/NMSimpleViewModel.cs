using Coursework1.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Coursework1.UI.View_Models
{
    public class NMSimpleViewModel : BaseViewModel
    {
        public string rien { get; set; }
        public ICommand SMSCommand { get; private set; }
        public ICommand EmailCommand { get; private set; }
        public ICommand TweetCommand { get; private set; }
        public NMSimpleViewModel()
        {
            SMSCommand = new RelayCommand(SMSButtonClick);
            EmailCommand = new RelayCommand(EmailButtonClick);
            TweetCommand = new RelayCommand(TweetButtonClick);
            rien = "Jvais changer";
        }

        public void SMSButtonClick()
        {
            rien = "SMS";
        }

        public void EmailButtonClick()
        {
            rien = "Email";
        }

        public void TweetButtonClick()
        {
            rien = "Tweet";
        }
    }
}
