using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;


namespace WpfApp4.Pages
{
    /// <summary>
    /// Логика взаимодействия для Page4.xaml
    /// </summary>
    public partial class Page4 : Page
    {
        private MySqlConnection connection;
        private MySqlCommand cmd;
        string strikePlate;
        string choosenSystem;
        string isAsymmetric;
        string choosentype;
        public Page4()
        {
            InitializeComponent();
            GetPlateForStriker();
            GetSysteminfo();
            getIsAsymmetric();
            InsertIntoChoosenMainStrikers();
            FillTypeComboBox();
        }



        string connectionString = (string)Application.Current.FindResource("MyConnectionString");

        public void GetPlateForStriker()
        {
            MySqlConnection connection = null;
            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();

                string plateQuery = "SELECT Strike_Plate FROM choosenplate";
                using (MySqlCommand plateCmd = new MySqlCommand(plateQuery, connection))
                {
                    object result = plateCmd.ExecuteScalar(); // ExecuteScalar to get a single value
                    if (result != null)
                    {
                        // Assuming the Strike_Plate is of string type, you can adjust accordingly
                        strikePlate = result.ToString();

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
        public void GetSysteminfo()
        {
            MySqlConnection connection = null;
            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();

                string systemQuery = "SELECT Door_System FROM choosensystem";
                using (MySqlCommand plateCmd = new MySqlCommand(systemQuery, connection))
                {
                    object result = plateCmd.ExecuteScalar(); // ExecuteScalar to get a single value
                    if (result != null)
                    {
                        // Assuming the Strike_Plate is of string type, you can adjust accordingly
                        choosenSystem = result.ToString();

                    }
                    else
                    {
                        MessageBox.Show("No system found.");
                    }
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while retrieving and inserting system info: " + ex.Message);
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            
        }
        public void getIsAsymmetric()
        {
            MySqlConnection connection = null;
            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();
                string asymmetricQuery = "SELECT Is_Asymmetric FROM systems WHERE System_Name = @choosenSystem";
                using (MySqlCommand asymmCmd = new MySqlCommand(asymmetricQuery, connection))
                {
                    asymmCmd.Parameters.AddWithValue("@choosenSystem", choosenSystem); // Set the parameter value
                    object result = asymmCmd.ExecuteScalar(); // ExecuteScalar to get a single value
                    if (result != null)
                    {
                        // Assuming the Is_Asymmetric is of string type, you can adjust accordingly
                        isAsymmetric = result.ToString();
                    }
                    else
                    {
                        MessageBox.Show("No system found.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while retrieving and inserting system info: " + ex.Message);
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            MessageBox.Show(isAsymmetric);
            if (isAsymmetric == "No")
            {
                insideoutsidelabel.Visibility = Visibility.Hidden;
                inside_outsidecombobox.Visibility = Visibility.Hidden;
            }
            else
            {

            }
        }
        public void InsertIntoChoosenMainStrikers()
        {
            MySqlConnection connection = null;
            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();

                string insertQuery = "INSERT INTO choosenmainstrikers (Striker_Code, Striker_Shape, Striker_Type, Striker_side, Is_Asymmetric) " +
                                     "SELECT Striker_Code, Striker_Shape, Striker_Type, Striker_side, Is_Asymmetric " +
                                     "FROM main_strikers " +
                                     "WHERE Striker_Shape = @strikePlate AND Is_Asymmetric = @isAsymmetric";

                using (MySqlCommand insertCmd = new MySqlCommand(insertQuery, connection))
                {
                    insertCmd.Parameters.AddWithValue("@strikePlate", strikePlate);
                    insertCmd.Parameters.AddWithValue("@isAsymmetric", isAsymmetric);
                    int rowsAffected = insertCmd.ExecuteNonQuery();
                    MessageBox.Show(rowsAffected + " rows inserted into choosenmainstrikers table.");
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
        public void FillTypeComboBox()
        {
            MySqlConnection connection = null;
            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();

                string selectQuery = "SELECT DISTINCT Striker_Type FROM choosenmainstrikers";

                using (MySqlCommand selectCmd = new MySqlCommand(selectQuery, connection))
                {
                    using (MySqlDataReader reader = selectCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string strikerType = reader["Striker_Type"].ToString();
                            typecombobox.Items.Add(strikerType);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while filling type combobox: " + ex.Message);
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }
        public void FillSideComboBox(string choosentype)
        {
            MySqlConnection connection = null;
            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();

                string selectQuery = "SELECT DISTINCT Striker_side FROM choosenmainstrikers WHERE Striker_Type = @choosentype";

                using (MySqlCommand selectCmd = new MySqlCommand(selectQuery, connection))
                {
                    selectCmd.Parameters.AddWithValue("@choosentype", choosentype);

                    using (MySqlDataReader reader = selectCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string strikerSide = reader["Striker_side"].ToString();
                            sidecombobox.Items.Add(strikerSide);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while filling side combobox: " + ex.Message);
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }
        private void typecombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            choosentype = typecombobox.SelectedItem.ToString();
            sidecombobox.Items.Clear();
            FillSideComboBox(choosentype);
        }
    }
}
