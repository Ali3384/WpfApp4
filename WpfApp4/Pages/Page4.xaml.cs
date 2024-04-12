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
        string typeofLock;
        string heightOfLock;
        int check;
        string choosenonepiece;
        string choosenside;
        public Page4()
        {
            
            InitializeComponent();
            
            GetLockinfo();
            GetLockHeightinfo();
            GetPlateForStriker();
            GetSysteminfo();
            getIsAsymmetric();
            InsertIntoChoosenMainStrikers();
            InsertIntoChoosenCentralStrikers();
            InsertIntoChoosenOnePieceStrikers();
            getIsOnePiece();
            FillOnePieceComboBox();
        }



        string connectionString = WpfApp4.Connection.ConnectionString;

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
        public void GetLockinfo()
        {
            MySqlConnection connection = null;
            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();

                string systemQuery = "SELECT Lock_Type FROM choosenlocks";
                using (MySqlCommand plateCmd = new MySqlCommand(systemQuery, connection))
                {
                    object result = plateCmd.ExecuteScalar(); // ExecuteScalar to get a single value
                    if (result != null)
                    {
                        // Assuming the Strike_Plate is of string type, you can adjust accordingly
                        typeofLock = result.ToString();

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
        public void GetLockHeightinfo()
        {
            MySqlConnection connection = null;
            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();

                string systemQuery = "SELECT Lock_Height FROM choosenlocks";
                using (MySqlCommand plateCmd = new MySqlCommand(systemQuery, connection))
                {
                    object result = plateCmd.ExecuteScalar(); // ExecuteScalar to get a single value
                    if (result != null)
                    {
                        // Assuming the Strike_Plate is of string type, you can adjust accordingly
                        heightOfLock = result.ToString();

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
        public void getIsOnePiece()
        {
            MySqlConnection connection = null;
            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();

                string selectQuery = "SELECT COUNT(*) FROM choosenonepiecestrikers";
                MySqlCommand command = new MySqlCommand(selectQuery, connection);
                check = Convert.ToInt32(command.ExecuteScalar());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while checking: " + ex.Message);
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
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
                                     "WHERE Striker_Shape = @strikePlate AND Is_Asymmetric = @isAsymmetric AND Striker_Type LIKE @typeofLock";

                using (MySqlCommand insertCmd = new MySqlCommand(insertQuery, connection))
                {
                    insertCmd.Parameters.AddWithValue("@strikePlate", strikePlate);
                    insertCmd.Parameters.AddWithValue("@isAsymmetric", isAsymmetric);
                    insertCmd.Parameters.AddWithValue("@typeofLock", "%" + typeofLock + "%");
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
        public void InsertIntoChoosenCentralStrikers()
        {
            MySqlConnection connection = null;
            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();

                string insertQuery = "INSERT INTO choosencentralstrikers (Striker_Code, Striker_Shape, Striker_side, Is_Asymmetric, Outside_Inside) " +
                                     "SELECT Striker_Code, Striker_Shape, Striker_side, Is_Asymmetric, Outside_Inside " +
                                     "FROM central_strikers " +
                                     "WHERE Striker_Shape = @strikePlate AND Is_Asymmetric = @isAsymmetric";

                using (MySqlCommand insertCmd = new MySqlCommand(insertQuery, connection))
                {
                    insertCmd.Parameters.AddWithValue("@strikePlate", strikePlate);
                    insertCmd.Parameters.AddWithValue("@isAsymmetric", isAsymmetric);
                    int rowsAffected = insertCmd.ExecuteNonQuery();
                    MessageBox.Show(rowsAffected + " rows inserted into choosencentralstrikers table.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while inserting data into choosencentralstrikers table: " + ex.Message);
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        public void InsertIntoChoosenOnePieceStrikers()
        {
            MySqlConnection connection = null;
            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();

                string insertQuery = "INSERT INTO choosenonepiecestrikers (Striker_Code, One_piece, Striker_Shape, Striker_Type, Striker_side, Striker_Height) " +
                     "SELECT Striker_Code, One_piece, Striker_Shape, Striker_Type, Striker_side, Striker_Height " +
                     "FROM onepiece_strikers " +
                     "WHERE Striker_Shape = @strikePlate AND Striker_Height LIKE @heightofLock AND Striker_Type LIKE @typeofLock";

                using (MySqlCommand insertCmd = new MySqlCommand(insertQuery, connection))
                {
                    insertCmd.Parameters.AddWithValue("@strikePlate", strikePlate);
                    insertCmd.Parameters.AddWithValue("@heightofLock", "%" + heightOfLock + "%");
                    insertCmd.Parameters.AddWithValue("@typeofLock", "%" + typeofLock + "%");
                    int rowsAffected = insertCmd.ExecuteNonQuery();
                    MessageBox.Show(rowsAffected + " rows inserted into choosenonepiecestrikers table.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while inserting data into OnePiece table: " + ex.Message);
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
            string selectQuery = "";
            MySqlConnection connection = null;
            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();
                
                if (choosenonepiece == "Single Strikers")
                {
                    selectQuery = "SELECT DISTINCT Striker_Type FROM choosenmainstrikers";
                }
                else
                {
                    selectQuery = "SELECT DISTINCT Striker_Type FROM choosenonepiecestrikers";
                }
                

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
        private void onepieceget()
        {
            try
            {
                // Connection string
                string connectionString = WpfApp4.Connection.ConnectionString;

                // SQL query to delete data from choosenlocks table
                string deleteQuery = "DELETE FROM choosenonepiecestrikers " +
                     "WHERE Striker_side != @Side AND Striker_side != 'Universal'";

                // Open connection
                using (connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Prepare command for deletion
                    using (cmd = new MySqlCommand(deleteQuery, connection))
                    {
                        // Add parameters for selected values
                        cmd.Parameters.AddWithValue("@Side", choosenside);
                        int rowsAffected = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void centralstrikeget()
        {
            try
            {
                // Connection string
                string connectionString = WpfApp4.Connection.ConnectionString;

                // SQL query to delete data from choosenlocks table
                string deleteQuery = "DELETE FROM choosencentralstrikers " +
                     "WHERE Striker_side != @Side AND Striker_side != 'Universal'";

                // Open connection
                using (connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Prepare command for deletion
                    using (cmd = new MySqlCommand(deleteQuery, connection))
                    {
                        // Add parameters for selected values
                        cmd.Parameters.AddWithValue("@Side", choosenside);
                        int rowsAffected = cmd.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void mainstrikeget()
        {
            try
            {
                // Connection string
                string connectionString = WpfApp4.Connection.ConnectionString;

                // SQL query to delete data from choosenlocks table
                string deleteQuery = "DELETE FROM choosenmainstrikers " +
                     "WHERE Striker_Type != @Type";

                // Open connection
                using (connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Prepare command for deletion
                    using (cmd = new MySqlCommand(deleteQuery, connection))
                    {
                        // Add parameters for selected values
                        cmd.Parameters.AddWithValue("@Type", choosentype);
                        int rowsAffected = cmd.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        public void FillSideComboBox(string choosentype)
        {
            MySqlConnection connection = null;
            try
            {
                string selectQuery = "";
                connection = new MySqlConnection(connectionString);
                connection.Open();
                if (choosenonepiece == "Single Strikers")
                {
                    selectQuery = "SELECT DISTINCT Striker_side FROM choosencentralstrikers";
                }
                else
                {
                    selectQuery = "SELECT DISTINCT Striker_side FROM choosenonepiecestrikers WHERE Striker_Type = @choosentype";
                }
                

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
            if (typecombobox.SelectedItem != null)
            {
                choosentype = typecombobox.SelectedItem.ToString();
                sidecombobox.Items.Clear();
                FillSideComboBox(choosentype);
            }
        }

        public void FillOnePieceComboBox()
        {
            if (check > 0)
            {
                onepiececombobox.Items.Add("One Piece Striker");
                onepiececombobox.Items.Add("Single Strikers");
            }
            else
            {
                onepiececombobox.Items.Add("Single Strikers");
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(strikePlate + " " + choosenSystem + " " + choosentype + " " + isAsymmetric + " " + heightOfLock);
            if(choosenonepiece == "Single Strikers")
            {
                mainstrikeget();
                centralstrikeget();
            }
            else
            {
                onepieceget();
            }
            NavigationService.Navigate(new FinalPage());
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void onepiececombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
                choosenonepiece = onepiececombobox.SelectedItem.ToString();
                typecombobox.Items.Clear();
                FillTypeComboBox();
        }

        private void sidecombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            choosenside = sidecombobox.SelectedItem.ToString();
        }
    }
}
