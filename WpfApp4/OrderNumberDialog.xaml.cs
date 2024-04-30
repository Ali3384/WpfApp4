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

namespace WpfApp4
{
    /// <summary>
    /// Логика взаимодействия для OrderNumberDialog.xaml
    /// </summary>
    public partial class OrderNumberDialog : Window
    {
        public string OrderNumber { get; private set; }
        public OrderNumberDialog()
        {
            InitializeComponent();
        }
        private void OK_Click(object sender, RoutedEventArgs e)
        {
            OrderNumber = txtOrderNumber.Text;
            DialogResult = true;
        }


    }
}
