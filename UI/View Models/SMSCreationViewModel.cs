using Coursework1.Core;
using Coursework1.Models;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace Coursework1.UI.View_Models
{
    /// <summary>
    /// View to easily create a SMS.
    /// </summary>
    public class SMSCreationViewModel : BaseViewModel
    {
        public string PhoneNumber { get; set; } //Phone Number text box text
        public string Text { get; set; }//Text text box text
        public ICommand SaveButtonCommand { get; private set; }//Save button control

        /// <summary>
        /// View to easily create a SMS.
        /// </summary>
        public SMSCreationViewModel()
        {
            PhoneNumber = string.Empty;//Initialization of the content of the text boxes
            Text = string.Empty;
            SaveButtonCommand = new RelayCommand(SaveButtonClick);//Binding the Save button control to its function
        }
        /// <summary>
        /// Save the created SMS
        /// </summary>
        public void SaveButtonClick()
        {
            //Regex pattern found on
            //https://stackoverflow.com/a/19133469
            MatchCollection matches = Regex.Matches(PhoneNumber, @"^((\+\d{1,3}(-| )?\(?\d\)?(-| )?\d{1,5})|(\(?\d{2,6}\)?))(-| )?(\d{3,4})(-| )?(\d{4})(( x| ext)\d{1,5}){0,1}\b");
            if (string.IsNullOrWhiteSpace(PhoneNumber)) //Testing if the PhoneNumber is filled in
            {
                MessageBox.Show("Your SMS needs a PhoneNumber to be sent!");
            }
            else if (string.IsNullOrWhiteSpace(Text))
            {
                MessageBox.Show("Your SMS needs a Text!");//Testing if the Text is filled in
            }
            else if (matches.Count == 0)
            {
                MessageBox.Show("Your Phone number must be a valid international phone number");//Testing if the PhoneNumber begins with an @
            }
            else if (matches[0].ToString() != PhoneNumber)
            {
                MessageBox.Show("Your Phone number must be a valid international phone number");//Testing if the PhoneNumber begins with an @
            }
            else if (Text.Length > 140)
            {
                MessageBox.Show("A SMS cannot be over 140 characters long!");//Testing the length of the text
            }
            else
            {
                //Creation of the header
                string header = "S";
                Random random = new();
                for (int i = 0; i < 9; i++)
                {
                    int number = random.Next(10);
                    header += number.ToString();
                }
                //Creation of the body
                string body = $"{PhoneNumber}\n{Text}";
                SMS sms = new(header, body);
                sms.WriteToJSON(); //Saving as a JSON File
                PhoneNumber = string.Empty;
                Text = string.Empty;
                OnChanged(nameof(PhoneNumber));
                OnChanged(nameof(Text));
                MessageBox.Show("SMS saved !", "Saved", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
