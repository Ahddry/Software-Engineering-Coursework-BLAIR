using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            bool isHeaderOk = true;
            if (string.IsNullOrWhiteSpace(HeaderTextBox) || string.IsNullOrWhiteSpace(BodyTextBox))
            {
                MessageBox.Show("Message Header and Body can't be empty");
                isHeaderOk = false;
            }

            if (HeaderTextBox.Length != 10)
            {
                isHeaderOk = false;
            }

            string testHeaderIndex = HeaderTextBox[1..];
            bool testIndex = int.TryParse(testHeaderIndex, out int theNumber);
            if (!testIndex)
                isHeaderOk = false;
            if (theNumber == 0)
                isHeaderOk = false;
            if (isHeaderOk)
            {
                HeaderTextBox = HeaderTextBox.ToUpper();
                switch (HeaderTextBox[0])
                {
                    case 'S':
                        SMS sms = new(HeaderTextBox, BodyTextBox);
                        if (sms.Sender == "Unknown")
                            MessageBox.Show("The Message needs a Sender\n(In the form of an international phone number)");
                        else if (string.IsNullOrWhiteSpace(sms.Text))
                            MessageBox.Show("The Message needs a Text\n(at least 2 characters)");
                        else if (sms.Text.Length > 140)
                            MessageBox.Show("A SMS cannot be over 140 characters long!");
                        else
                        {
                            sms.WriteToJSON();
                            HeaderTextBox = string.Empty;
                            BodyTextBox = string.Empty;
                            OnChanged(nameof(HeaderTextBox));
                            OnChanged(nameof(BodyTextBox));
                            MessageBox.Show("SMS saved !", "Saved", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        break;
                    case 'E':
                        Email email = new(HeaderTextBox, BodyTextBox);
                        if (email.Sender == "Unknown")
                            MessageBox.Show("The Message needs a Sender\n(In the form of an email address)");
                        else if (string.IsNullOrWhiteSpace(email.Object))
                            MessageBox.Show("The Message needs an object");
                        else if (string.IsNullOrWhiteSpace(email.Text))
                            MessageBox.Show("The Message needs a Text\n(at least 2 characters)");
                        else
                        {
                            email.WriteToJSON();
                            HeaderTextBox = string.Empty;
                            BodyTextBox = string.Empty;
                            OnChanged(nameof(HeaderTextBox));
                            OnChanged(nameof(BodyTextBox));
                            MessageBox.Show("Email saved !", "Saved", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        break;
                    case 'T':
                        Tweet tweet = new(HeaderTextBox, BodyTextBox);
                        if (tweet.Sender == "Unknown")
                            MessageBox.Show("The Message needs a Sender\n(In the form of a tweeter @Username)");
                        else if (string.IsNullOrWhiteSpace(tweet.Text))
                            MessageBox.Show("The Message needs a Text\n(at least 2 characters)");
                        else if (tweet.Text.Length > 140)
                            MessageBox.Show("A Tweet cannot be over 140 characters long!");
                        else
                        {
                            tweet.WriteToJSON();
                            HeaderTextBox = string.Empty;
                            BodyTextBox = string.Empty;
                            OnChanged(nameof(HeaderTextBox));
                            OnChanged(nameof(BodyTextBox));
                            MessageBox.Show("Tweet saved !", "Saved", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        break;
                    default:
                        MessageBox.Show("ERROR:\nUnable to recognize that header as a valid message header.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                }
            }
            else
                MessageBox.Show("Invalid Message Header");
        }
    }
}
