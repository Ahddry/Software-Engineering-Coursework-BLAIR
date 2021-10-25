﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Controls;

namespace Coursework1.UI.View_Models
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnChanged(string name)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
        public ICommand HomeCommand { get; private set; }
        public ICommand NewMessageCommand { get; private set; }
        public ICommand ImportMessagesCommand { get; private set; }
        public ICommand ReadMessagesCommand { get; private set; }

        public UserControl ContentControlBinding { get; private set; }
    }
}