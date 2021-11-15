using Coursework1.Core;
using Coursework1.Models;
using Coursework1.UI.View;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Coursework1.UI.View_Models
{
    /// <summary>
    /// Model to the view that allows the user to read a single saved messages at the a time,
    /// but in a very detailed detailed form.
    /// </summary>
    class SingleMessageViewModel : BaseViewModel
    {
        public int Index { get; }
        public List<MessageType> ReadMessages { get; set; }
        public new UserControl ContentControlBinding { get; private set; }
        public ICommand BackButtonCommand { get; private set; }
        public ICommand PreviousButtonCommand { get; private set; }
        public ICommand NextButtonCommand { get; private set; }
        public ICommand ShowQuarantinedURLsCommand { get; private set; }
        private string[] QuarantinedLinks { get; }
        #region TextBlocks
        public string Type { get; set; }
        public string Date { get; set; }
        public string Header { get; set; }
        public string Sender { get; set; }
        public string Text { get; set; }
        public string OtherOption1 { get; set; }
        public string OtherOption2 { get; set; }
        public string OtherOption3 { get; set; }
        public string OtherOption4 { get; set; }
        public string ButtonName { get; set; }
        #endregion
        /// <summary>
        /// Model to the view that allows the user to read a single saved messages at the a time,
        /// but in a very detailed detailed form.
        /// </summary>
        /// <param name="path">Path to the message to open.</param>
        /// <param name="index">Index of the message in the list.</param>
        /// <param name="messageList">List of all the read messages.</param>
        public SingleMessageViewModel(string path, int index, List<MessageType> messageList)
        {
            Email email = new("", "");
            SMS sms = new("", "");
            Tweet tweet = new("", "");
            string content = File.ReadAllText(path);
            if (content.Contains("\"Type\":\"Email\""))//If it s an email
            {
                //Reserealize the file as an Email
                //https://docs.microsoft.com/en-us/dotnet/framework/wcf/feature-details/how-to-serialize-and-deserialize-json-data
                MemoryStream ms = new(Encoding.UTF8.GetBytes(content));
                DataContractJsonSerializer ser = new(typeof(Email));
                email = ser.ReadObject(ms) as Email;
                ms.Close();
                Type = email.Type;
            }
            else if (content.Contains("\"Type\":\"SMS\""))//If it is a SMS
            {
                //Reserealize the file as a SMS
                //https://docs.microsoft.com/en-us/dotnet/framework/wcf/feature-details/how-to-serialize-and-deserialize-json-data
                MemoryStream ms = new(Encoding.UTF8.GetBytes(content));
                DataContractJsonSerializer ser = new(typeof(SMS));
                sms = ser.ReadObject(ms) as SMS;
                ms.Close();
                Type = sms.Type;
            }
            else if (content.Contains("\"Type\":\"Tweet\""))//If it is a Tweet
            {
                //Reserealize the file as a Tweet
                //https://docs.microsoft.com/en-us/dotnet/framework/wcf/feature-details/how-to-serialize-and-deserialize-json-data
                MemoryStream ms = new(Encoding.UTF8.GetBytes(content));
                DataContractJsonSerializer ser = new(typeof(Tweet));
                tweet = ser.ReadObject(ms) as Tweet;
                ms.Close();
                Type = tweet.Type;
            }
            else
            {
                Text = "Unreadable file";
                Header = string.Empty;
                Sender = "From: Unknown";
                Type = "Unknown";
                Date = string.Empty;
            }
            BackButtonCommand = new RelayCommand(BackButtonClick);
            PreviousButtonCommand = new RelayCommand(PreviousButtonClick);
            NextButtonCommand = new RelayCommand(NextButtonClick);
            ShowQuarantinedURLsCommand = new RelayCommand(ShowQuarantinedURLsClick);
            Index = index;
            ReadMessages = messageList;
            ButtonName = string.Empty;
            switch (Type)
            {
                case "Email": //If the message is an email show all its infos
                    Header = $"Message n°{email.Header}";
                    Sender = $"From: {email.Sender}\n {email.Object}";
                    Text = email.Text;
                    Date = email.Date;
                    ButtonName = " Show Qurantined URLs ";
                    QuarantinedLinks = email.QuarantinedLinks;
                    if(email.SIR) //if the email is a SIR
                    {   //add the Sort code and the nature of incident as the import infos to be displayed
                        OtherOption1 = "SIR Sort Code:"; 
                        OtherOption2 = email.SIRSortCode;
                        OtherOption3 = "Nature of Incident:";
                        OtherOption4 = email.NatureOfIncident;
                    }
                    else //if it is a standard email
                    {
                        if (email.QuarantinedLinks != null)
                        {   //Add the number of Quarantined links to the infos
                            OtherOption1 = "Number of Quarantined URLs:";
                            OtherOption2 = QuarantinedLinks.Length.ToString();
                        }
                    }
                    break;
                case "SMS": //If the message is a SMS
                    Header = $"Message n°{sms.Header}";
                    Sender = sms.Sender;
                    Text = sms.Text;
                    Date = sms.Date;
                    break;
                case "Tweet": //If the message is a Tweet
                    Header = $"Message n°{tweet.Header}";
                    Sender = tweet.Sender;
                    Text = tweet.Text;
                    Date = tweet.Date;
                    if (tweet.Mentions != null)
                    {   //Display the Mentioned users
                        OtherOption1 = "Mentions:";
                        foreach (string mention in tweet.Mentions)
                            OtherOption2 += $"{mention} ";
                    }
                    if (tweet.Hashtags != null)
                    {   //Display the Hashtags
                        OtherOption3 = "Hashtags:";
                        foreach (string hashtag in tweet.Hashtags)
                            OtherOption4 += $"{hashtag} ";
                    }
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// Button to return to the Read Messages View
        /// </summary>
        public void BackButtonClick()
        {
            ContentControlBinding = new ReadMessagesView();
            OnChanged(nameof(ContentControlBinding));
        }
        /// <summary>
        /// Load the previous message from the list.
        /// </summary>
        public void PreviousButtonClick()
        {
            if (Index > 0) //If this message is not the already the first of the list
            {
                ContentControlBinding = new SingleMessageView(ReadMessages[Index - 1].GetPath(), Index - 1, ReadMessages);
                OnChanged(nameof(ContentControlBinding));
            }
        }
        /// <summary>
        /// Load the next message of the list.
        /// </summary>
        public void NextButtonClick()
        {
            if (Index < ReadMessages.Count-1)//If this message is not the already the last of the list
            {
                ContentControlBinding = new SingleMessageView(ReadMessages[Index + 1].GetPath(), Index + 1, ReadMessages);
                OnChanged(nameof(ContentControlBinding));
            }
        }
        /// <summary>
        /// If the message has quarantined links, opens a message box with a warning message to inform the user about the 
        /// possible dangerosity of the links and display them.
        /// </summary>
        public void ShowQuarantinedURLsClick()
        {
            if (QuarantinedLinks != null)
            {
                if (QuarantinedLinks.Length != 0)
                {
                    string links = "Only open these links if the sender of the message is trustworthy!\n";
                    foreach (string link in QuarantinedLinks)
                        links += $"{link}\n";
                    MessageBox.Show(links, "Quarantined links", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
    }
}
