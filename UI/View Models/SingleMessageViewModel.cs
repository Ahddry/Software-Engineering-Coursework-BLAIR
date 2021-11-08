using Coursework1.Core;
using Coursework1.Models;
using Coursework1.UI.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Coursework1.UI.View_Models
{
    class SingleMessageViewModel : BaseViewModel
    {
        public MessageType Message { get; private set; }
        public int Index { get; }
        public List<MessageType> ReadMessages { get; set; }
        public new UserControl ContentControlBinding { get; private set; }
        public ICommand PreviousButtonCommand { get; private set; }
        public ICommand NextButtonCommand { get; private set; }
        #region TextBlocks
        public string Type { get; set; }
        public string Header { get; set; }
        public string Body { get; set; }
        public string OtherOption1 { get; set; }
        public string OtherOption2 { get; set; }
        public string OtherOption3 { get; set; }
        #endregion

        public SingleMessageViewModel(MessageType message, int index, List<MessageType> messageList)
        {
            PreviousButtonCommand = new RelayCommand(PreviousButtonClick);
            NextButtonCommand = new RelayCommand(NextButtonClick);
            Message = message;
            Index = index;
            ReadMessages = messageList;
            Type = Message.Type;
            Header = Message.Header;
            string b = Message.Body;
            Body = Regex.Replace(b, @"\t|\n|\r", " "); //https://stackoverflow.com/questions/4140723/how-to-remove-new-line-characters-from-a-string
            switch(Type)
            {
                case "Email":

                    break;
                case "SMS":
                    break;
                case "Tweet":
                    break;
                default:
                    break;
            }
        }

        public void PreviousButtonClick()
        {
            ContentControlBinding = new SingleMessageView(ReadMessages[Index - 1], Index - 1, ReadMessages);
            OnChanged(nameof(ContentControlBinding));
        }

        public void NextButtonClick()
        {
            ContentControlBinding = new SingleMessageView(ReadMessages[Index + 1], Index + 1, ReadMessages);
            OnChanged(nameof(ContentControlBinding));
        }
    }
}
