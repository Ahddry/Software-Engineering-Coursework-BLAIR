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
    /// <summary>
    /// Model to the view that allows the user to write messages in the most basic form, with a header and a body.
    /// </summary>
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
        /// <summary>
        /// Model to the view that allows the user to write messages in the most basic form, with a header and a body.
        /// </summary>
        public NMAdvancedViewModel()
        {
            HeaderTextBlock = "Header";
            BodyTextBlock = "Body";

            HeaderTextBox = string.Empty;
            BodyTextBox = string.Empty;

            SaveButtonText = "Save Message";

            SaveButtonCommand = new RelayCommand(SaveButtonClick);
        }
        /// <summary>
        /// Verify wether the message can be saved or not and save it.
        /// </summary>
        private void SaveButtonClick()
        {
            bool isHeaderOk = true; //Find if the content of the file follows the right formal
            if (string.IsNullOrWhiteSpace(HeaderTextBox) || string.IsNullOrWhiteSpace(BodyTextBox))
            {
                MessageBox.Show("Message Header and Body can't be empty");
                isHeaderOk = false;
            }
            //Find if the header follows all the valid conditions
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
                    case 'S'://Parse the body as a SMS
                        SMS sms = new(HeaderTextBox, BodyTextBox);
                        if (sms.Sender == "Unknown")
                            MessageBox.Show("The Message needs a Sender\n(In the form of an international phone number)");
                        else if (string.IsNullOrWhiteSpace(sms.Text))//Find if the text follows the conditions
                            MessageBox.Show("The Message needs a Text\n(at least 2 characters)");
                        else if (sms.Text.Length > 140)
                            MessageBox.Show("A SMS cannot be over 140 characters long!");
                        else
                        {   //Save the SMS if eveything is valid
                            sms.WriteToJSON();
                            HeaderTextBox = string.Empty;
                            BodyTextBox = string.Empty;
                            OnChanged(nameof(HeaderTextBox));
                            OnChanged(nameof(BodyTextBox));
                            MessageBox.Show("SMS saved !", "Saved", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        break;
                    case 'E'://Parse the body as an Email
                        Email email = new(HeaderTextBox, BodyTextBox);
                        if (email.Sender == "Unknown")//Find if the text follows all the conditions
                            MessageBox.Show("The Message needs a Sender\n(In the form of an email address)");
                        else if (string.IsNullOrWhiteSpace(email.Object))
                            MessageBox.Show("The Message needs an object");
                        else if (string.IsNullOrWhiteSpace(email.Text))
                            MessageBox.Show("The Message needs a Text\n(at least 2 characters)");
                        else if (email.Text.Length > 1028)
                            MessageBox.Show("An Email text cannot be over 1028 characters long!");
                        else if (email.Object.Length > 60)
                            MessageBox.Show("An Email object cannot be over 20 characters long!");
                        else
                        {   //Save the Email if everything is valid
                            email.WriteToJSON();
                            HeaderTextBox = string.Empty;
                            BodyTextBox = string.Empty;
                            OnChanged(nameof(HeaderTextBox));
                            OnChanged(nameof(BodyTextBox));
                            MessageBox.Show("Email saved !", "Saved", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        break;
                    case 'T'://Parse the body as a Tweet
                        Tweet tweet = new(HeaderTextBox, BodyTextBox);
                        if (tweet.Sender == "Unknown")
                            MessageBox.Show("The Message needs a Sender\n(In the form of a tweeter @Username)");
                        else if (string.IsNullOrWhiteSpace(tweet.Text))//Find if the text follows all the conditions
                            MessageBox.Show("The Message needs a Text\n(at least 2 characters)");
                        else if (tweet.Text.Length > 140)
                            MessageBox.Show("A Tweet cannot be over 140 characters long!");
                        else
                        {   //Save the Tweet if everything is valid
                            tweet.WriteToJSON();
                            HeaderTextBox = string.Empty;
                            BodyTextBox = string.Empty;
                            OnChanged(nameof(HeaderTextBox));
                            OnChanged(nameof(BodyTextBox));
                            MessageBox.Show("Tweet saved !", "Saved", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        break;
                    default: //Display a message if the message doesn't follow the conditions.
                        MessageBox.Show("ERROR:\nUnable to recognize that header as a valid message header.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                }
            }
            else
                MessageBox.Show("Invalid Message Header");
        }
    }
}
