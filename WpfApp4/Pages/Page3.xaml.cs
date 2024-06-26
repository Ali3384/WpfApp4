﻿using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;


namespace WpfApp4.Pages
{
    /// <summary>
    /// Логика взаимодействия для Page2.xaml
    /// </summary>
    public partial class Page3 : Page
    {
        public int choosenoption;
        public string chooseneopener;
        public string choosenHeight;
        public string choosenSeries;
        public string choosenType;
        public string choosenFunction;
        public string isExtension;
        public string lockShape;
        public string extension;
        public string choosenleaf { get; set; }
        public string eopener { get; set; }
        private MySqlConnection connection;
        private MySqlCommand cmd;
        private bool choosenaddlock;
        public string roundrod;
        public string roundrodheight;
        public Page3()
        {
            InitializeComponent();
            Page2 page2 = new Page2();
            GetFirstLockShape();


        }
        string connectionString = Properties.Settings.Default.connection + "database=alu_standard;";
        private void GetFirstLockShape()
        {

            string connectionString = Properties.Settings.Default.connection + "database=alu_standard;";
            string query = "SELECT DISTINCT Lock_Shape FROM choosenlocks";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                lockShape = reader.GetString(0);
                            }
                            else
                            {
                                // Handle case where no rows are returned
                                MessageBox.Show("No lock shapes found.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log exception details somewhere
                MessageBox.Show("An error occurred while fetching lock shapes. Please try again later.");
            }


        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (heightcombobox.SelectedItem != null)
            {

                string combobox = ((ComboBoxItem)heightcombobox.SelectedItem).Content.ToString();
                if (combobox == "1670-1870")
                {
                    roundrod = "VQRG10V20";
                    roundrodheight = Properties.Resources.round_rod_2000;
                    choosenHeight = "Low";
                    isExtension = "No";
                }
                if (combobox == "1870-2170")
                {
                    roundrod = "VQRG10V25";
                    roundrodheight = Properties.Resources.round_rod_2500;
                    choosenHeight = "Standard";
                    isExtension = "No";
                }
                if (combobox == "2170-2400")
                {
                    roundrodheight = Properties.Resources.round_rod_2500;
                    roundrod = "VQRG10V25";
                    choosenHeight = "High";
                    isExtension = "No";
                }
                if (combobox == "2390-2990")
                {
                    roundrodheight = Properties.Resources.round_rod_3000;
                    roundrod = "VQRG10V30";
                    choosenHeight = "Standard";
                    isExtension = "Yes";
                }
                if (combobox == "2590-3185")
                {
                    roundrodheight = Properties.Resources.round_rod_3000;
                    roundrod = "VQRG10V30";
                    choosenHeight = "High";
                    isExtension = "Yes";
                }
                seriescombobox.IsEnabled = true;
                seriescombobox.Items.Clear();
                typecombobox.Items.Clear();
                functioncombobox.Items.Clear();

                PopulateLockSeriesComboBox();

            }
        }

        private void GetExtensionValue()
        {
            string connectionString = Properties.Settings.Default.connection + "database=alu_standard;";
            string query = "SELECT Item_Code FROM extensions WHERE Extension_Plate = @lockShape AND Extension_Type = @Type";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        // Set parameter values
                        cmd.Parameters.AddWithValue("@lockShape", lockShape);
                        cmd.Parameters.AddWithValue("@Type", choosenType);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                extension = reader.GetString(0);
                            }
                            else
                            {
                                // Handle case where no rows are returned
                                MessageBox.Show("No extension value found for the given criteria.");

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log exception details somewhere
                MessageBox.Show("An error occurred while fetching extension value. Please try again later.");

            }

        }
        private void AddExtension()
        {
            string connectionString = Properties.Settings.Default.connection + "database=alu_standard;";
            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();
                //**********************************************************
                string insertQuery = "INSERT INTO finaltable (Item_Code, Item_Description, Quantity) VALUES (@extension, @extensionadd, '1')";
                using (MySqlCommand insertCmd = new MySqlCommand(insertQuery, connection))
                {
                    byte[] addextension = Encoding.UTF8.GetBytes(Properties.Resources.extension);
                    insertCmd.Parameters.AddWithValue("@extension", extension);
                    insertCmd.Parameters.AddWithValue("@extensionadd", addextension);
                    insertCmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while inserting extension: " + ex.Message);
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }
        private void PopulateLockSeriesComboBox()
        {

            string connectionString = Properties.Settings.Default.connection + "database=alu_standard;";
            string query = "";
            if (isExtension == "Yes")
            {
                query = "SELECT DISTINCT Lock_Series FROM choosenlocks WHERE Lock_Height = @Height AND Higher = 'Yes'";
            }
            else if (isExtension == "No")
            {
                query = "SELECT DISTINCT Lock_Series FROM choosenlocks WHERE Lock_Height = @Height";
            }
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
            string connectionString = Properties.Settings.Default.connection + "database=alu_standard;";
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
            if (choosenSeries != "881GL" && seriescombobox.SelectedItem != null && choosenHeight != "Low")
            {
                AddLock.IsEnabled = true;
            }
            else if (choosenHeight == "Low")
            {
                AddLock.IsEnabled = false;
                AddLock.IsChecked = false;
            }
            else if (choosenSeries == "881GL" && seriescombobox.SelectedItem != null)
            {
                AddLock.IsEnabled = false;
                AddLock.IsChecked = false;
            }
        }
        private void PopulateFunctionComboBox() 
        {
            // Connection string
            string connectionString = Properties.Settings.Default.connection + "database=alu_standard;";

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
                NextStepBtn.IsEnabled = false;
            }
            if(choosenFunction == "Day_Night")
            {
                Eopener.IsEnabled = true;
            }
            else
            {
                Eopener.IsChecked = false;
                Eopener.IsEnabled = false;
            }
        }
        private void addAdditionalLock()
        {
            string connectionString = Properties.Settings.Default.connection + "database=alu_standard;";
            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();

                string insertQuery = "INSERT INTO finaltable (Item_Code, Item_Description, Quantity) VALUES ('VMZ852ZR35', @addlockinsert, '1')";
                using (MySqlCommand insertCmd = new MySqlCommand(insertQuery, connection))
                {
                    byte[] addlockinsert = Encoding.UTF8.GetBytes(Properties.Resources.add_lock_insert);
                    insertCmd.Parameters.AddWithValue("@addlockinsert", addlockinsert);
                    insertCmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while inserting addlock: " + ex.Message);
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        private void addEopener()
        {
            eopener = "Yes";
            string connectionString = Properties.Settings.Default.connection + "database=alu_standard;";
            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();

                string insertQuery = "INSERT INTO finaltable (Item_Code, Item_Description, Quantity) VALUES (@chooseneopener, @adde_opener, '1')";
                using (MySqlCommand insertCmd = new MySqlCommand(insertQuery, connection))
                {
                    byte[] adde_opener = Encoding.UTF8.GetBytes(Properties.Resources.adde_opener);
                    insertCmd.Parameters.AddWithValue("@chooseneopener", chooseneopener);
                    insertCmd.Parameters.AddWithValue("@adde_opener", adde_opener);
                    insertCmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while inserting eopener: " + ex.Message);
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }
        private void addRoundrod()
        {
            string connectionString = Properties.Settings.Default.connection + "database=alu_standard;";
            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();

                string insertQuery = "INSERT INTO finaltable (Item_Code, Item_Description, Quantity) VALUES (@roundrod, @roundrodheight, '1')";
                using (MySqlCommand insertCmd = new MySqlCommand(insertQuery, connection))
                {
                    byte[] addroundrodheight = Encoding.UTF8.GetBytes(roundrodheight);
                    insertCmd.Parameters.AddWithValue("@roundrod", roundrod);
                    insertCmd.Parameters.AddWithValue("roundrodheight", addroundrodheight);
                    insertCmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while inserting round rod into final table: " + ex.Message);
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();

                string insertQuery = "INSERT INTO finaltable (Item_Code, Item_Description, Quantity) VALUES ('VRS57369', @rodguide, '2')";
                using (MySqlCommand insertCmd = new MySqlCommand(insertQuery, connection))
                {
                    byte[] addrodguide = Encoding.UTF8.GetBytes(Properties.Resources.rod_guide);
                    insertCmd.Parameters.AddWithValue("@rodguide", addrodguide);
                    insertCmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while inserting roud guide into final table: " + ex.Message);
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();

                string insertQuery = "INSERT INTO finaltable (Item_Code, Item_Description, Quantity) VALUES ('VRRV56568X', @threshold_strike_plate, '1')";
                using (MySqlCommand insertCmd = new MySqlCommand(insertQuery, connection))
                {
                    byte[] addthreshold = Encoding.UTF8.GetBytes(Properties.Resources.threshold_strike_plate);
                    insertCmd.Parameters.AddWithValue("@threshold_strike_plate", addthreshold);
                    insertCmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while inserting threshold strike plate into final table: " + ex.Message);
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (heightcombobox.SelectedItem != null && seriescombobox.SelectedItem != null && typecombobox.SelectedItem != null && functioncombobox.SelectedItem != null)
            {
                if (AddLock.IsChecked == true)
                {
                    addAdditionalLock();
                }
                if (Eopener.IsChecked == true)
                {
                    addEopener();

                }
                if (isExtension == "Yes")
                {
                    GetExtensionValue();
                    AddExtension();
                }
                if (Properties.Settings.Default.leaf == "2leaf")
                {
                    addRoundrod();

                }
                string connectionString = Properties.Settings.Default.connection + "database=alu_standard;";


                string deleteQuery = "DELETE FROM choosenlocks " +
                                 "WHERE Lock_Height != @Height OR Lock_Series != @Series " +
                                 "OR Lock_Type != @Type OR Lock_Function != @Function";

                try
                {
                    // Connection string


                    // SQL query to delete data from choosenlocks table


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
            else
            {
                MessageBox.Show("You didn't fill all needed data.");
            }
        }



        private void NumberValidation(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void AddLock_Checked(object sender, RoutedEventArgs e)
        {
            choosenaddlock = true;
        }
        private void AddLock_Unchecked(object sender, RoutedEventArgs e)
        {
            choosenaddlock = false;
        }
        private void Eopener_Checked(object sender, RoutedEventArgs e)
        {
            
            eopeneroptions.Visibility = Visibility.Visible;
            
        }
        private void Eopener_Unchecked(object sender, RoutedEventArgs e)
        {
            eopeneroptions.Visibility = Visibility.Hidden;
        }
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            choosenoption = 1;
            chooseneopener = "VRYA08VE70S";
        }


        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
            choosenoption = 2;
            chooseneopener = "VRY000VE70S";
        }

        private void RadioButton_Checked_2(object sender, RoutedEventArgs e)
        {
            choosenoption = 3;
            chooseneopener = "VRYC17ZE70S";
        }

        private void RadioButton_Checked_3(object sender, RoutedEventArgs e)
        {
            choosenoption = 4;
            chooseneopener = "VRYC16ZE70S";
        }
        
    }
}

