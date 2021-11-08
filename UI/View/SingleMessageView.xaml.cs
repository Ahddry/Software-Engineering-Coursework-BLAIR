using Coursework1.Models;
using Coursework1.UI.View_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Coursework1.UI.View
{
    /// <summary>
    /// Logique d'interaction pour SingleMessageView.xaml
    /// </summary>
    public partial class SingleMessageView : UserControl
    {
        public SingleMessageView(MessageType message, int index, List<MessageType> messageList)
        {
            InitializeComponent();
            this.DataContext = new SingleMessageViewModel(message, index, messageList);
        }
    }
}
