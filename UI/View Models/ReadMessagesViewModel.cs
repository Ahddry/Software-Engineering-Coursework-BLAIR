using Coursework1.Core;
using Coursework1.Models;
using Coursework1.UI.View;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

namespace Coursework1.UI.View_Models
{
    /// <summary>
    /// Model to the view that allows the user to read all the saved messages at the same time,
    /// but not in a detailed form.
    /// </summary>
    public class ReadMessagesViewModel : BaseViewModel
    {
        public List<MessageType> ReadMessages { get; set; } //All the mesages read by the system
        public List<string> UnreadableMessages { get; set; }//All the messages the system couldn't read
        public ObservableCollection<MessageType> Messages { get; set; }//Display for the messages
        public new UserControl ContentControlBinding { get; private set; }//View to switch to
        public ICommand OpenItemCommand { get; private set; }
        public MessageType SelectedMessage { get; set; }
        /// <summary>
        /// Model to the view that allows the user to read all the saved messages at the same time,
        /// but not in a detailed form.
        /// </summary>
        public ReadMessagesViewModel()
        {
            ReadMessages = new();
            //Read all the files in the Saved Messages folder
            string path = @$"{ System.IO.Directory.GetCurrentDirectory()}\Saved Messages\";
            #region ReadFiles
            OpenItemCommand = new RelayCommand(OpenItem);
            //Source:
            //https://www.codegrepper.com/code-examples/csharp/c%23+read+all+files+in+a+directory
            foreach (string file in Directory.EnumerateFiles(path, "*.json"))
            {
                string content = File.ReadAllText(file);
                if (content.Contains("\"Type\":\"Email\""))//If it s an email
                {
                    //Reserealize the file as an Email
                    //https://docs.microsoft.com/en-us/dotnet/framework/wcf/feature-details/how-to-serialize-and-deserialize-json-data
                    Email email;
                    MemoryStream ms = new(Encoding.UTF8.GetBytes(content));
                    DataContractJsonSerializer ser = new(typeof(Email));
                    email = ser.ReadObject(ms) as Email;
                    ms.Close();
                    ReadMessages.Add(email);
                }
                else if (content.Contains("\"Type\":\"SMS\""))//If it is a SMS
                {
                    //Reserealize the file as a SMS
                    //https://docs.microsoft.com/en-us/dotnet/framework/wcf/feature-details/how-to-serialize-and-deserialize-json-data
                    SMS sms;
                    MemoryStream ms = new(Encoding.UTF8.GetBytes(content));
                    DataContractJsonSerializer ser = new(typeof(SMS));
                    sms = ser.ReadObject(ms) as SMS;
                    ms.Close();
                    ReadMessages.Add(sms);
                }
                else if (content.Contains("\"Type\":\"Tweet\""))//If it is a Tweet
                {
                    //Reserealize the file as a Tweet
                    //https://docs.microsoft.com/en-us/dotnet/framework/wcf/feature-details/how-to-serialize-and-deserialize-json-data
                    Tweet tweet;
                    MemoryStream ms = new(Encoding.UTF8.GetBytes(content));
                    DataContractJsonSerializer ser = new(typeof(Tweet));
                    tweet = ser.ReadObject(ms) as Tweet;
                    ms.Close();
                    ReadMessages.Add(tweet);
                }
                else
                {   //The unknown files are added in the list
                    UnreadableMessages.Add(file);
                }
            }
            #endregion
            SelectedMessage = new MessageType("", "");
            if (ReadMessages != null)
                Messages = new ObservableCollection<MessageType>(ReadMessages); //Display the read messages
        }
        /// <summary>
        /// On a right click from the user, it will open the selected messages in a more detailed view
        /// </summary>
        public void OpenItem()
        {
            if (SelectedMessage.Type != "Unknown")
            {
                ContentControlBinding = new SingleMessageView(SelectedMessage.GetPath(), ReadMessages.IndexOf(SelectedMessage), ReadMessages);
                OnChanged(nameof(ContentControlBinding));
            }
        }

    }
}
