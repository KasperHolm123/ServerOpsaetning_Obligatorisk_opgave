using ServerOpsaetning.Model;
using ServerOpsaetning.ViewModel;
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
using System.Windows.Shapes;

namespace ServerOpsaetning.View
{
    /// <summary>
    /// Interaction logic for ServerDetailsView.xaml
    /// </summary>
    public partial class ServerDetailsView : Window
    {
        ServerDetailsViewModel model;

        public ServerDetailsView(Server server)
        {
            model = new ServerDetailsViewModel(server);
            DataContext = model;
            InitializeComponent();
        }
    }
}
