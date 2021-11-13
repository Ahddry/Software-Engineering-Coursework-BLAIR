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

        public SingleMessageViewModel(string path, int index, List<MessageType> messageList)
        {
            Email email = new("", "");
            SMS sms = new("", "");
            Tweet tweet = new("", "");
            string content = File.ReadAllText(path);
            if (content.Contains("\"Type\":\"Email\""))
            {
                //https://docs.microsoft.com/en-us/dotnet/framework/wcf/feature-details/how-to-serialize-and-deserialize-json-data
                MemoryStream ms = new(Encoding.UTF8.GetBytes(content));
                DataContractJsonSerializer ser = new(typeof(Email));
                email = ser.ReadObject(ms) as Email;
                ms.Close();
                Type = email.Type;
            }
            else if (content.Contains("\"Type\":\"SMS\""))
            {
                //https://docs.microsoft.com/en-us/dotnet/framework/wcf/feature-details/how-to-serialize-and-deserialize-json-data
                MemoryStream ms = new(Encoding.UTF8.GetBytes(content));
                DataContractJsonSerializer ser = new(typeof(SMS));
                sms = ser.ReadObject(ms) as SMS;
                ms.Close();
                Type = sms.Type;
            }
            else if (content.Contains("\"Type\":\"Tweet\""))
            {
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
                case "Email":
                    Header = $"Message n°{email.Header}";
                    Sender = $"From: {email.Sender}\n {email.Object}";
                    //MessageObject = email.Object;
                    Text = email.Text;
                    Date = email.Date;
                    ButtonName = " Show Qurantined URLs ";
                    QuarantinedLinks = email.QuarantinedLinks;
                    if(email.SIR)
                    {
                        OtherOption1 = "SIR Sort Code:";
                        OtherOption2 = email.SIRSortCode;
                        OtherOption3 = "Nature of Incident:";
                        OtherOption4 = email.NatureOfIncident;
                    }
                    else
                    {
                        if (email.QuarantinedLinks != null)
                        {
                            OtherOption1 = "Number of Quarantined URLs:";
                            OtherOption2 = QuarantinedLinks.Length.ToString();
                        }
                    }
                    break;
                case "SMS":
                    Header = $"Message n°{sms.Header}";
                    Sender = sms.Sender;
                    Text = sms.Text;
                    Date = sms.Date;
                    break;
                case "Tweet":
                    Header = $"Message n°{tweet.Header}";
                    Sender = tweet.Sender;
                    Text = tweet.Text;
                    Date = tweet.Date;
                    if (tweet.Mentions != null)
                    {
                        OtherOption1 = "Mentions:";
                        foreach (string mention in tweet.Mentions)
                            OtherOption2 += $"{mention} ";
                    }
                    if (tweet.Hashtags != null)
                    {
                        OtherOption3 = "Hashtags:";
                        foreach (string hashtag in tweet.Hashtags)
                            OtherOption4 += $"{hashtag} ";
                    }
                    break;
                default:
                    break;
            }
        }

        public void BackButtonClick()
        {
            ContentControlBinding = new ReadMessagesView();
            OnChanged(nameof(ContentControlBinding));
        }

        public void PreviousButtonClick()
        {
            if (Index > 0)
            {
                ContentControlBinding = new SingleMessageView(ReadMessages[Index - 1].GetPath(), Index - 1, ReadMessages);
                OnChanged(nameof(ContentControlBinding));
            }
        }

        public void NextButtonClick()
        {
            if (Index < ReadMessages.Count-1)
            {
                ContentControlBinding = new SingleMessageView(ReadMessages[Index + 1].GetPath(), Index + 1, ReadMessages);
                OnChanged(nameof(ContentControlBinding));
            }
        }

        public void ShowQuarantinedURLsClick()
        {
            if (QuarantinedLinks != null)
            {
                string links = "Only open these links if the sender of the message is trustworthy!\n";
                foreach (string link in QuarantinedLinks)
                    links += $"{link}\n";
                MessageBox.Show(links, "Quarantined links", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
