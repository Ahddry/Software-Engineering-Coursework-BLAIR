using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Coursework1.UI.View_Models
{
    public class NMAdvancedViewModel : BaseViewModel
    {
        #region TextBlock Content
        public string HeaderTextBlock { get; private set; }
        public string BodyTextBlock { get; private set; }
        #endregion

        #region TextBox Content
        public string HeaderTextBox { get; set; }
        public string BodyTextBox { get; set; }
        #endregion

        #region Button
        public string SaveButtonText { get; private set; }
        public string BackButtonText { get; private set; }

        public ICommand SaveButtonCommand { get; private set; }
        public ICommand BackButtonCommand { get; private set; }
        #endregion

        public NMAdvancedViewModel()
        {
            HeaderTextBlock = "Header";
            BodyTextBlock = "Body";

            HeaderTextBox = string.Empty;
            BodyTextBox = string.Empty;

            SaveButtonText = "Save Message";
            BackButtonText = "Go back";

            //SaveButtonCommand = new RelayCommand(SaveButtonClick);
            //BackButtonCommand = new RelayCommand(BackButtonClick);
        }

        private void SaveButtonClick()
        {

        }

        private void BackButtonClick()
        {

        }
    }
}
