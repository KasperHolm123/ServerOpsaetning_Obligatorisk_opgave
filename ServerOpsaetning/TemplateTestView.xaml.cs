using ServerOpsaetning.Templates;
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

namespace ServerOpsaetning
{
    /// <summary>
    /// Interaction logic for TemplateTestView.xaml
    /// </summary>
    public partial class TemplateTestView : Window
    {
        TempClass temp;
        public TemplateTestView()
        {
            InitializeComponent();
            temp = new TempClass((p) =>
            {
                mainWrapPanel.Children.Add(p.CreateTemplate());
            });
        }
    }
}
