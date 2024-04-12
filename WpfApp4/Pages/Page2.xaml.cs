using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;


namespace WpfApp4.Pages
{
    /// <summary>
    /// Логика взаимодействия для Page2.xaml
    /// </summary>
    public partial class Page2 : Page
    {
        private MySqlConnection connection;
        private MySqlCommand cmd;
        string choosenSystem = "";
        string choosenleaf = "";
        
        public Page2()
        {
            InitializeComponent();
            _1leaf.IsChecked = true;
            PopulateComboBox();
            
        }

        private void PopulateComboBox()
        {
            
            string connectionString = WpfApp4.Connection.ConnectionString; 
            string query = "SELECT System_Name FROM systems"; 

            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();

                cmd = new MySqlCommand(query, connection);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string systemName = reader.GetString("System_Name");
                    systemscombobox.Items.Add(systemName); 
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error to connect database application will be closed please try again or change settings.", "Connection Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
            }
        }

        string connectionString = WpfApp4.Connection.ConnectionString;

        public void GetPlateForStriker(string chosenSystem)
        {
            MySqlConnection connection = null;
            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();

                string plateQuery = "SELECT Strike_Plate FROM systems WHERE System_Name = @chosenSystem";
                using (MySqlCommand plateCmd = new MySqlCommand(plateQuery, connection))
                {
                    plateCmd.Parameters.AddWithValue("@chosenSystem", chosenSystem); // Use chosenSystem instead of connection
                    object result = plateCmd.ExecuteScalar(); // ExecuteScalar to get a single value
                    if (result != null)
                    {
                        // Assuming the Strike_Plate is of string type, you can adjust accordingly
                        string strikePlate = result.ToString();

                        // Insert into choosenplate table
                        string insertQuery = "INSERT INTO choosenplate (Strike_Plate) VALUES (@strikePlate)";
                        using (MySqlCommand insertCmd = new MySqlCommand(insertQuery, connection))
                        {
                            insertCmd.Parameters.AddWithValue("@strikePlate", strikePlate);
                            insertCmd.ExecuteNonQuery();
                        }
                        string insertsystemQuery = "INSERT INTO choosensystem (Door_System) VALUES (@chosenSystem)";
                        using (MySqlCommand insertCmd = new MySqlCommand(insertsystemQuery, connection))
                        {
                            insertCmd.Parameters.AddWithValue("@chosenSystem", chosenSystem);
                            insertCmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        MessageBox.Show("No strike plate found for the chosen system.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while retrieving and inserting strike plate: " + ex.Message);
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        public void GetAndInsertLocks(string chosenSystem)
        {
            MySqlConnection connection = null;
            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();

                // Retrieve Lock_Plate from systems table
                string systemQuery = "SELECT Lock_Plate FROM systems WHERE System_Name = @chosenSystem";
                using (MySqlCommand systemCmd = new MySqlCommand(systemQuery, connection))
                {
                    systemCmd.Parameters.AddWithValue("@chosenSystem", chosenSystem);
                    string lockPlate = systemCmd.ExecuteScalar()?.ToString();

                    if (!string.IsNullOrEmpty(lockPlate))
                    {
                        // Retrieve locks based on Lock_Plate
                        string locksQuery = "SELECT * FROM locks WHERE Lock_Shape = @lockPlate";
                        using (MySqlCommand locksCmd = new MySqlCommand(locksQuery, connection))
                        {
                            locksCmd.Parameters.AddWithValue("@lockPlate", lockPlate);

                            using (MySqlDataReader reader = locksCmd.ExecuteReader(CommandBehavior.CloseConnection))
                            {
                                while (reader.Read())
                                {
                                    InsertLock(reader, connectionString);
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Lock Plate not found for the chosen system.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                connection?.Close();
            }
        }

        private void InsertLock(MySqlDataReader reader, string connectionString)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string insertQuery = "INSERT INTO choosenlocks (Lock_Code, Lock_Series, Lock_Shape, Lock_Height, Lock_Type, Lock_Side, Lock_Function) VALUES (@Lock_Code, @Lock_Series, @Lock_Shape, @Lock_Height, @Lock_Type, @Lock_Side, @Lock_Function)";
                using (MySqlCommand insertCmd = new MySqlCommand(insertQuery, connection))
                {
                    insertCmd.Parameters.AddWithValue("@Lock_Code", reader["Lock_Code"]);
                    insertCmd.Parameters.AddWithValue("@Lock_Series", reader["Lock_Series"]);
                    insertCmd.Parameters.AddWithValue("@Lock_Shape", reader["Lock_Shape"]);
                    insertCmd.Parameters.AddWithValue("@Lock_Height", reader["Lock_Height"]);
                    insertCmd.Parameters.AddWithValue("@Lock_Type", reader["Lock_Type"]);
                    insertCmd.Parameters.AddWithValue("@Lock_Side", reader["Lock_Side"]);
                    insertCmd.Parameters.AddWithValue("@Lock_Function", reader["Lock_Function"]);

                    insertCmd.ExecuteNonQuery();
                }
            }
        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Page3());
            GetPlateForStriker(choosenSystem);
            GetAndInsertLocks(choosenSystem);
        }

        private void _2leaf_Checked(object sender, RoutedEventArgs e)
        {
            choosenleaf = "2leaf";
            _1leaf.IsChecked = false;
            }

        private void _1leaf_Checked(object sender, RoutedEventArgs e)
        {
            choosenleaf = "1leaf";
            _2leaf.IsChecked = false;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
            choosenSystem = systemscombobox.SelectedItem.ToString();
            
        }
    }
}
