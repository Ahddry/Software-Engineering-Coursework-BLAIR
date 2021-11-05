using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Coursework1.Core;
using Coursework1.Models;
using Microsoft.Win32;

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

        public ICommand SaveButtonCommand { get; private set; }
        #endregion

        public NMAdvancedViewModel()
        {
            HeaderTextBlock = "Header";
            BodyTextBlock = "Body";

            HeaderTextBox = string.Empty;
            BodyTextBox = string.Empty;

            SaveButtonText = "Save Message";

            SaveButtonCommand = new RelayCommand(SaveButtonClick);
        }

        private void SaveButtonClick()
        {
            MessageBox.Show($"Header :\n{HeaderTextBox}\nBody : \n{BodyTextBox}");
            if (string.IsNullOrWhiteSpace(HeaderTextBox) || string.IsNullOrWhiteSpace(BodyTextBox)) 
                MessageBox.Show("Message Header and Body can't be empty");
            else
            {
                switch (HeaderTextBox[0])
                {
                    case 'S':
                        SMS sms = new(HeaderTextBox, BodyTextBox);
                        sms.WriteToJSON();
                        HeaderTextBox = string.Empty;
                        BodyTextBox = string.Empty;
                        MessageBox.Show("SMS saved !", "Saved", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    case 'E':
                        Email email = new(HeaderTextBox, BodyTextBox);
                        email.WriteToJSON();
                        HeaderTextBox = string.Empty;
                        BodyTextBox = string.Empty;
                        MessageBox.Show("Email saved !", "Saved", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    case 'T':
                        Tweet tweet = new(HeaderTextBox, BodyTextBox);
                        tweet.WriteToJSON();
                        HeaderTextBox = string.Empty;
                        BodyTextBox = string.Empty;
                        MessageBox.Show("Tweet saved !", "Saved", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    default:
                        MessageBox.Show("ERROR:\nUnable to recognize that header as a valid message header.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                }
            }
        }
    }
}
