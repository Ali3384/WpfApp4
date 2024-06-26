﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
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
        public string plateforLock;
        private MySqlConnection connection;
        private MySqlCommand cmd;
        string choosenSystem = "";
        public string choosenleaf { get; set; }
        string strikePlate;
        private List<string> systems;

        public Page2()
        {
            InitializeComponent();
            _1leaf.IsChecked = true;
            PopulateComboBox();

        }

        private void PopulateComboBox()
        {

            string connectionString = Properties.Settings.Default.connection + "database=alu_standard;";
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

        string connectionString = Properties.Settings.Default.connection + "database=alu_standard;";

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
                        strikePlate = result.ToString();

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
                    plateforLock = lockPlate;
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

        public void InsertLock(MySqlDataReader reader, string connectionString)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string insertQuery = "INSERT INTO choosenlocks (Lock_Code, Lock_Series, Lock_Shape, Lock_Height, Lock_Type, Lock_Side, Lock_Function, Higher) VALUES (@Lock_Code, @Lock_Series, @Lock_Shape, @Lock_Height, @Lock_Type, @Lock_Side, @Lock_Function, @Higher)";
                using (MySqlCommand insertCmd = new MySqlCommand(insertQuery, connection))
                {
                    insertCmd.Parameters.AddWithValue("@Lock_Code", reader["Lock_Code"]);
                    insertCmd.Parameters.AddWithValue("@Lock_Series", reader["Lock_Series"]);
                    insertCmd.Parameters.AddWithValue("@Lock_Shape", reader["Lock_Shape"]);
                    insertCmd.Parameters.AddWithValue("@Lock_Height", reader["Lock_Height"]);
                    insertCmd.Parameters.AddWithValue("@Lock_Type", reader["Lock_Type"]);
                    insertCmd.Parameters.AddWithValue("@Lock_Side", reader["Lock_Side"]);
                    insertCmd.Parameters.AddWithValue("@Lock_Function", reader["Lock_Function"]);
                    insertCmd.Parameters.AddWithValue("@Higher", reader["Higher"]);
                    insertCmd.ExecuteNonQuery();
                }
            }
        }
        public void getandinsert()
        {
            GetAndInsertLocks(Properties.Settings.Default.choosensystem);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string insertQuery;
            GetPlateForStriker(choosenSystem);
            GetAndInsertLocks(choosenSystem);
            Properties.Settings.Default.choosensystem = choosenSystem;
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Upgrade();
            if (_1leaf.IsChecked == true)
            {
                Properties.Settings.Default.leaf = "1leaf";
            }
            else
            {
                Properties.Settings.Default.leaf = "2leaf";
                if (strikePlate == "F24")
                {
                    insertQuery = "INSERT INTO finaltable (Item_Code, Item_Description, Quantity) VALUES ('VBK345K24R35N', @espagnolette_bolt, '1')";
                }
                else
                {
                    insertQuery = "INSERT INTO finaltable (Item_Code, Item_Description, Quantity) VALUES ('VBK345K24I35N', @espagnolette_bolt, '1')";
                }
                string connectionString = Properties.Settings.Default.connection + "database=alu_standard;";
                try
                {
                    connection = new MySqlConnection(connectionString);
                    connection.Open();


                    using (MySqlCommand insertCmd = new MySqlCommand(insertQuery, connection))
                    {
                        byte[] espagnoletteBoltBytes = Encoding.UTF8.GetBytes(Properties.Resources.espagnolette_bolt);

                        // Pass the byte array as a parameter value
                        insertCmd.Parameters.AddWithValue("@espagnolette_bolt", espagnoletteBoltBytes);
                        insertCmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error while inserting data into choosenmainstrikers table: " + ex.Message);
                }
                finally
                {
                    if (connection != null && connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }

            NavigationService.Navigate(new Page3());
        }

        private void _2leaf_Checked(object sender, RoutedEventArgs e)
        {

            _1leaf.IsChecked = false;
        }

        private void _1leaf_Checked(object sender, RoutedEventArgs e)
        {

            _2leaf.IsChecked = false;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Properties.Settings.Default.choosensystem = choosenSystem;
            choosenSystem = systemscombobox.SelectedItem.ToString();
            if (choosenSystem == "REYNAERS (SL38 HI)" | choosenSystem == "COR 70 Industrial - system cieply 70mm.")
            {
                _2leaf.IsEnabled = false;
                _2leaf.IsChecked = false;
                _1leaf.IsChecked = true;
            }
            else
            {
                _2leaf.IsEnabled = true;

            }
            nextbtn.IsEnabled = true;
        }

        private void _1leaf_Unchecked(object sender, RoutedEventArgs e)
        {

        }
       

    }
}
