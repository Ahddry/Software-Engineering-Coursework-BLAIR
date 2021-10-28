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
using Coursework1.UI.View_Models;

namespace Coursework1.UI.View
{
    /// <summary>
    /// Logique d'interaction pour NMAdvancedView.xaml
    /// </summary>
    public partial class NMAdvancedView : UserControl
    {
        public NMAdvancedView()
        {
            InitializeComponent();
            this.DataContext = new NMAdvancedViewModel();
        }
    }
}
