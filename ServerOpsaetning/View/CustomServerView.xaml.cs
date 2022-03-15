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
using ServerOpsaetning.Model;
using ServerOpsaetning.ViewModel;

namespace ServerOpsaetning.View
{
    /// <summary>
    /// Interaction logic for CustomServerView.xaml
    /// </summary>
    public partial class CustomServerView : Window
    {
        public EditViewModel model;
        public CustomServerView(ServerCreated server)
        {
            model = new EditViewModel(server);
            model.CloseRequest += (s, e) => this.Close();
            DataContext = model;
            InitializeComponent();
        }
    }

}
