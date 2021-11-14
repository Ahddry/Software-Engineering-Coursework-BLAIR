using Coursework1.Core;
using Coursework1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Coursework1.UI.View_Models
{
    public class TweetCreationViewModel : BaseViewModel
    {
        public string Username { get; set; } //Username text box text
        public string Text { get; set; } //Text text box text
        public ICommand SaveButtonCommand { get; private set; } //Save button control

        public TweetCreationViewModel()
        {
            Username = string.Empty; //Initialization of the content of the text boxes
            Text = string.Empty;
            SaveButtonCommand = new RelayCommand(SaveButtonClick); //Binding the Save button control to its function
        }

        public void SaveButtonClick()
        {
            if (string.IsNullOrWhiteSpace(Username)) //Testing if the username is filled in
            {
                MessageBox.Show("Your Tweet needs a Username to be sent!");
            }
            else if (string.IsNullOrWhiteSpace(Text))
            {
                MessageBox.Show("Your Tweet needs a Text!");//Testing if the Text is filled in
            }
            else if (Username[0] != '@')
            {
                MessageBox.Show("Your username must start with an @");//Testing if the username begins with an @
            }
            else if (Text.Length > 140)
            {
                MessageBox.Show("A Tweet cannot be over 140 characters long!");//Testing the length of the text
            }
            else
            {
                //Creation of the header
                string header = "T";
                Random random = new();
                for (int i = 0; i < 9; i++)
                {
                    int number = random.Next(10);
                    header += number.ToString();
                }
                //Creation of the body
                string body = $"{Username}\n{Text}";
                Tweet tweet = new(header, body);
                tweet.WriteToJSON(); //Saving as a JSON File
                Username = string.Empty;
                Text = string.Empty;
                OnChanged(nameof(Username));
                OnChanged(nameof(Text));
                MessageBox.Show("Tweet saved !", "Saved", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
