using Coursework1.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Coursework1.UI.View_Models
{
    public class ReadMessagesViewModel
    {
        public List<MessageType> ReadMessages { get; set; } 
        public List<string> UnreadableMessages { get; set; }
        public ObservableCollection<MessageType> Messages { get; set; }
        public string testlol { get; set; }
        public string leContenu { get; set; }
        public ReadMessagesViewModel()
        {
            ReadMessages = new();
            string path = @$"{ System.IO.Directory.GetCurrentDirectory()}\..\..\..\Saved Messages\";
            #region ReadFiles
            foreach (string file in Directory.EnumerateFiles(path, "*.json"))
            {
                string content = File.ReadAllText(file);
                if (content.Contains("\"Type\":\"Email\""))
                {
                    //https://docs.microsoft.com/en-us/dotnet/framework/wcf/feature-details/how-to-serialize-and-deserialize-json-data
                    Email email;
                    MemoryStream ms = new(Encoding.UTF8.GetBytes(content));
                    DataContractJsonSerializer ser = new(typeof(Email));
                    email = ser.ReadObject(ms) as Email;
                    ms.Close();
                    ReadMessages.Add(email);
                }
                else if (content.Contains("\"Type\":\"SMS\""))
                {
                    //https://docs.microsoft.com/en-us/dotnet/framework/wcf/feature-details/how-to-serialize-and-deserialize-json-data
                    SMS sms;
                    MemoryStream ms = new(Encoding.UTF8.GetBytes(content));
                    DataContractJsonSerializer ser = new(typeof(SMS));
                    sms = ser.ReadObject(ms) as SMS;
                    ms.Close();
                    ReadMessages.Add(sms);
                }
                else if (content.Contains("\"Type\":\"Tweet\""))
                {
                    //https://docs.microsoft.com/en-us/dotnet/framework/wcf/feature-details/how-to-serialize-and-deserialize-json-data
                    Tweet tweet;
                    MemoryStream ms = new(Encoding.UTF8.GetBytes(content));
                    DataContractJsonSerializer ser = new(typeof(Tweet));
                    tweet = ser.ReadObject(ms) as Tweet;
                    ms.Close();
                    ReadMessages.Add(tweet);
                }
                else
                {
                    //UnreadableMessages.Add(new Tuple<string, string>(file, content));
                }
            }
            #endregion

            if (ReadMessages != null)
                Messages = new ObservableCollection<MessageType>(ReadMessages);
            else
            {
                testlol = "C vide\n";
                leContenu += $"voilà.\n{path}";
                //foreach(var truc in ReadMessages)
                //{
                //    leContenu += $"{truc.Type}\n";
                //}
            }
        }

    }
}
