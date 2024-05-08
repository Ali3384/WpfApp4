using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.Win32;
using System.Windows.Documents;
using System.Xml.Linq;
using System.IO;
using iText.IO.Image;
using System.Windows.Media;
using System.Drawing;
using iText.IO.Font;
using iText.Kernel.Font;
using Color = System.Windows.Media.Color;

namespace WpfApp4.Pages
{
    /// <summary>
    /// Логика взаимодействия для basketpage.xaml
    /// </summary>
    public partial class basketpage : Page
    {
        string connectionStringbasket = Properties.Settings.Default.connection + "database=basket;";
        DataTable dataTable = new DataTable();
        string orderNumber = "";
        public basketpage()
        {
            InitializeComponent();
            fillData();
        }
        private void fillData()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionStringbasket))
                {
                    connection.Open();
                    string query = "SELECT Item, Quantity, Description FROM finaltable";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(command);

                    adapter.Fill(dataTable);
                    dataTable.Columns["Item_Code"].ColumnName = Properties.Resources.column_code;
                    dataTable.Columns["Quantity"].ColumnName = Properties.Resources.column_qty;
                    dataTable.Columns["Item_Description"].ColumnName = Properties.Resources.column_description;

                    finaltable.ItemsSource = dataTable.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while filling data: " + ex.Message);
            }
        }
    }
}
