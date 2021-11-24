using Coursework1.Core;
using Coursework1.UI.View;
using System.Windows.Controls;
using System.Windows.Input;

namespace Coursework1.UI.View_Models
{
    /// <summary>
    /// Intermediary view model that acts as a menu.
    /// </summary>
    public class NMSimpleViewModel : BaseViewModel
    {
        public ICommand SMSCommand { get; private set; }
        public ICommand EmailCommand { get; private set; }
        public ICommand TweetCommand { get; private set; }
        public new UserControl ContentControlBinding { get; private set; }
        /// <summary>
        /// Intermediary view model that acts as a menu.
        /// </summary>
        public NMSimpleViewModel()
        {
            SMSCommand = new RelayCommand(SMSButtonClick);
            EmailCommand = new RelayCommand(EmailButtonClick);
            TweetCommand = new RelayCommand(TweetButtonClick);
        }
        /// <summary>
        /// Open the SMS creation view
        /// </summary>
        public void SMSButtonClick()
        {
            ContentControlBinding = new SMSCreationView();
            OnChanged(nameof(ContentControlBinding));
        }
        /// <summary>
        /// Opens the Email creation view
        /// </summary>
        public void EmailButtonClick()
        {
            ContentControlBinding = new EmailCreationView();
            OnChanged(nameof(ContentControlBinding));
        }
        /// <summary>
        /// Opens the Tweet creation view
        /// </summary>
        public void TweetButtonClick()
        {
            ContentControlBinding = new TweetCreationView();
            OnChanged(nameof(ContentControlBinding));
        }
    }
}
