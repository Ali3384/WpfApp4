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
using MySql.Data.MySqlClient;


namespace WpfApp4.Pages
{
    /// <summary>
    /// Логика взаимодействия для Page2.xaml
    /// </summary>
    public partial class Page3 : Page
    {
        public string choosenHeight;
        public string choosenSeries;
        private MySqlConnection connection;
        private MySqlCommand cmd;
        public Page3()
        {
            InitializeComponent();
            
        }
        
      

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new WelcomePage());
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (heightcombobox.SelectedItem != null)
            {
                choosenHeight = ((ComboBoxItem)heightcombobox.SelectedItem).Content.ToString();
                seriescombobox.IsEnabled = true;
                seriescombobox.Items.Clear();
                PopulateLockSeriesComboBox();
            }
        }

        private void PopulateLockSeriesComboBox()
        {
            
            string connectionString = "server=localhost;port=3306;uid=root;pwd=root;database=dobory;";
            string query = "SELECT DISTINCT Lock_Series FROM choosenlocks WHERE Lock_Height = @Height";

            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();

                cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Height", choosenHeight); // Add parameter for choosenHeight
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string lockSeries = reader.GetString("Lock_Series");
                    seriescombobox.Items.Add(lockSeries);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
            }
        }

        private void PopulateLockTypeComboBox()
        {
            string connectionString = "server=localhost;port=3306;uid=root;pwd=root;database=dobory;";
            string query = "SELECT DISTINCT Lock_Type FROM choosenlocks WHERE Lock_Series = @Series";

            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();

                cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Series", choosenSeries); // Add parameter for choosenSeries

                // Assuming choosenSeries is a string type, adjust it accordingly if it's an int
                // If choosenSeries is int, use Convert.ToInt32() to convert it to int
                // Example: cmd.Parameters.AddWithValue("@Series", Convert.ToInt32(choosenSeries));

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int lockType = reader.GetInt32("Lock_Type"); // Assuming Lock_Type is of type INT
                    typecombobox.Items.Add(lockType.ToString()); // Convert int to string before adding to the ComboBox
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (connection != null && connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
            }
        }

        private void seriescombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (seriescombobox.SelectedItem != null)
            {
                choosenSeries = seriescombobox.SelectedItem.ToString();
                typecombobox.IsEnabled = true;
                typecombobox.Items.Clear();
                PopulateLockTypeComboBox(); // Call the new method
            }
        }
    }
}
