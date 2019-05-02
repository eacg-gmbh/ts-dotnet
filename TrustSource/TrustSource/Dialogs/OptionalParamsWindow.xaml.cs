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

namespace TrustSource.Dialogs
{
    /// <summary>
    /// Interaction logic for OptionalParamsWindow.xaml
    /// </summary>
    public partial class OptionalParamsWindow
    {
        public string Branch { get; set; }

        public string TagValue { get; set; }

        public OptionalParamsWindow()
        {
            this.HasMaximizeButton = false;
            this.HasMinimizeButton = false;

            InitializeComponent();          
        }

        private void btnProceed_Click(object sender, RoutedEventArgs e)
        {
            Branch = txtBranchName.Text;
            TagValue = txtTagValue.Text;

            DialogResult = true;
            Close();
        }

        private void btnSkip_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
