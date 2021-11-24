using Coursework1.Core;
using Coursework1.UI.View;
using System.Windows.Controls;
using System.Windows.Input;

namespace Coursework1.UI.View_Models
{
    public class HomeViewModel : BaseViewModel
    {
        public ICommand CreditButtonCommand { get; private set; }
        public new UserControl ContentControlBinding { get; private set; }
        public HomeViewModel()
        {
            CreditButtonCommand = new RelayCommand(CreditButtonClick);
        }

        public void CreditButtonClick()
        {
            ContentControlBinding = new CreditsView();
            OnChanged(nameof(ContentControlBinding));
        }
    }
}
