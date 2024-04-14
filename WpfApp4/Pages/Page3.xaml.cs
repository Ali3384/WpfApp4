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
        public string choosenType;
        public string choosenFunction;
        private MySqlConnection connection;
        private MySqlCommand cmd;
        public Page3()
        {
            InitializeComponent();
            
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (heightcombobox.SelectedItem != null)
            {
                choosenHeight = ((ComboBoxItem)heightcombobox.SelectedItem).Content.ToString();
                seriescombobox.IsEnabled = true;
                seriescombobox.Items.Clear();
                typecombobox.Items.Clear();
                functioncombobox.Items.Clear();
                PopulateLockSeriesComboBox();
            }
        }

        private void PopulateLockSeriesComboBox()
        {

            string connectionString = Properties.Settings.Default.connection;
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
            string connectionString = Properties.Settings.Default.connection;
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
                functioncombobox.Items.Clear();
                typecombobox.Items.Clear();
                PopulateLockTypeComboBox(); // Call the new method
            }
        }
        private void PopulateFunctionComboBox()
        {
            // Connection string
            string connectionString = Properties.Settings.Default.connection;

            // SQL query to fetch distinct functions based on selected height, series, and type
            string query = "SELECT DISTINCT Lock_Function FROM choosenlocks " +
                           "WHERE Lock_Height = @Height AND Lock_Series = @Series AND Lock_Type = @Type";

            try
            {
                // Open connection
                using (connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Prepare command
                    using (cmd = new MySqlCommand(query, connection))
                    {
                        // Add parameters for selected height, series, and type
                        cmd.Parameters.AddWithValue("@Height", choosenHeight);
                        cmd.Parameters.AddWithValue("@Series", choosenSeries);
                        cmd.Parameters.AddWithValue("@Type", choosenType);

                        // Execute reader
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Clear existing items
                            functioncombobox.Items.Clear();

                            // Read each distinct function and add to functioncombobox
                            while (reader.Read())
                            {
                                string lockFunction = reader.GetString("Lock_Function");
                                functioncombobox.Items.Add(lockFunction);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void typecombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (typecombobox.SelectedItem != null)
            {
                choosenType = typecombobox.SelectedItem.ToString();
                functioncombobox.IsEnabled = true;
                functioncombobox.Items.Clear();
                PopulateFunctionComboBox(); // Call the new method
            }
        }

        private void functioncombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            if (functioncombobox.SelectedItem != null)
            {
                NextStepBtn.IsEnabled = true;
                choosenFunction = functioncombobox.SelectedItem.ToString();

            }
            else
            {
                NextStepBtn.IsEnabled= false;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {


            try
            {
                // Connection string
                string connectionString = Properties.Settings.Default.connection;

                // SQL query to delete data from choosenlocks table
                string deleteQuery = "DELETE FROM choosenlocks " +
                                     "WHERE Lock_Height != @Height OR Lock_Series != @Series " +
                                     "OR Lock_Type != @Type OR Lock_Function != @Function";

                // Open connection
                using (connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Prepare command for deletion
                    using (cmd = new MySqlCommand(deleteQuery, connection))
                    {
                        // Add parameters for selected values
                        cmd.Parameters.AddWithValue("@Height", choosenHeight);
                        cmd.Parameters.AddWithValue("@Series", choosenSeries);
                        cmd.Parameters.AddWithValue("@Type", choosenType);
                        cmd.Parameters.AddWithValue("@Function", choosenFunction);

                        // Execute deletion
                        int rowsAffected = cmd.ExecuteNonQuery();
                    }
                }
                NavigationService.Navigate(new Page4());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}
