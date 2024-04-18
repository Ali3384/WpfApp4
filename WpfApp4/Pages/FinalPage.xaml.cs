using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace WpfApp4.Pages
{
    /// <summary>
    /// Логика взаимодействия для WelcomePage.xaml
    /// </summary>
    public partial class FinalPage : Page
    {

        bool isconnection = false;
        string side;
        public string onepiece { get; set; }
        string connectionString = Properties.Settings.Default.connection;
        string choosenLock;
        string choosenOnepieceStriker;
        string choosenMainStriker;
        string choosenCentralStriker;
        public int qtystrikers;
        public bool check;
        string extension;
        public FinalPage()
        {
            InitializeComponent();
            checkisneedstriker();

            Page4 page4 = new Page4();

            if (check == true)
            {
                qtystrikers = 3;
                extension = "Striker for up/down locks and extension lock";
            }
            else
            {
                qtystrikers = 2;
                extension = "Striker for up/down locks";
            }
            systemlabel.Content = page4.choosenSystem;
            onepiece = page4.str;
            getLock();
            getCentralStriker();
            getMainStriker();
            getOnePieceStriker();
            getonepiece();
            makeFinalTable();
            fillData();


        }

        private void butsettings_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new WelcomePage());
            closing();
        }

        public void closing()
        {


            // Truncate tables
            TruncateTable(connectionString, "choosenlocks");
            TruncateTable(connectionString, "choosenplate");
            TruncateTable(connectionString, "choosensystem");
            TruncateTable(connectionString, "choosenmainstrikers");
            TruncateTable(connectionString, "choosenonepiecestrikers");
            TruncateTable(connectionString, "choosencentralstrikers");
            TruncateTable(connectionString, "finaltable");
            TruncateTable(connectionString, "isonepiece");
        }
        private void fillData()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT Item_Code, Quantity, Item_Description FROM finaltable";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    finaltable.ItemsSource = dataTable.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while filling data: " + ex.Message);
            }
        }

        private void TruncateTable(string connectionString, string tableName)
        {
            string query = $"TRUNCATE TABLE {tableName}";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    if (connection.State == System.Data.ConnectionState.Open)
                        connection.Close();
                }
            }
        }
        private void checkisneedstriker()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM finaltable WHERE Item_Description = @ItemDescription";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ItemDescription", "Extension");

                    try
                    {
                        connection.Open();
                        int count = Convert.ToInt32(command.ExecuteScalar());
                        check = count > 0;
                    }
                    catch (MySqlException ex)
                    {
                        // Handle exceptions here
                        Console.WriteLine("An error occurred: " + ex.Message);
                    }
                }
            }

        }
        private void makeFinalTable()
        {

            MySqlConnection connection = null;
            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();

                string insertQuery = "INSERT INTO finaltable (Item_Code, Item_Description, Quantity) VALUES (@lock, 'Lock', '1')";
                using (MySqlCommand insertCmd = new MySqlCommand(insertQuery, connection))
                {
                    insertCmd.Parameters.AddWithValue("@lock", choosenLock);

                    insertCmd.ExecuteNonQuery();
                }

                if (onepiece == "Single Strikers")
                {
                    string insert3Query = "INSERT INTO finaltable (Item_Code, Item_Description, Quantity) VALUES (@mainstrikers, @extension, @qtystrikers)";
                    using (MySqlCommand insertCmd = new MySqlCommand(insert3Query, connection))
                    {
                        insertCmd.Parameters.AddWithValue("@mainstrikers", choosenMainStriker);
                        insertCmd.Parameters.AddWithValue("@extension", extension);
                        insertCmd.Parameters.AddWithValue("@qtystrikers", qtystrikers);
                        insertCmd.ExecuteNonQuery();
                    }
                    string insert2Query = "INSERT INTO finaltable (Item_Code, Item_Description, Quantity) VALUES (@centralstriker, 'Central striker', '1')";
                    using (MySqlCommand insertCmd = new MySqlCommand(insert2Query, connection))
                    {
                        insertCmd.Parameters.AddWithValue("@centralstriker", choosenCentralStriker);

                        insertCmd.ExecuteNonQuery();
                    }
                }
                else
                {

                    if (check == true)
                    {
                        string insert4Query = "INSERT INTO finaltable (Item_Code, Item_Description, Quantity) VALUES (@onepiecestriker, 'One Piece Striker', '1')";
                        using (MySqlCommand insertCmd = new MySqlCommand(insert4Query, connection))
                        {
                            insertCmd.Parameters.AddWithValue("@onepiecestriker", choosenOnepieceStriker);
                            insertCmd.ExecuteNonQuery();
                        }
                        string insert3Query = "INSERT INTO finaltable (Item_Code, Item_Description, Quantity) VALUES (@mainstrikers, 'Striker for extension lock', '1')";
                        using (MySqlCommand insertCmd = new MySqlCommand(insert3Query, connection))
                        {
                            insertCmd.Parameters.AddWithValue("@mainstrikers", choosenMainStriker);
                            insertCmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        string insert4Query = "INSERT INTO finaltable (Item_Code, Item_Description, Quantity) VALUES (@onepiecestriker, 'One Piece Striker', '1')";
                        using (MySqlCommand insertCmd = new MySqlCommand(insert4Query, connection))
                        {
                            insertCmd.Parameters.AddWithValue("@onepiecestriker", choosenOnepieceStriker);
                            insertCmd.ExecuteNonQuery();
                        }
                    }
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

        private void getLock()
        {
            MySqlConnection connection = null;
            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();

                string systemQuery = "SELECT Lock_Code FROM choosenlocks";
                using (MySqlCommand plateCmd = new MySqlCommand(systemQuery, connection))
                {
                    object result = plateCmd.ExecuteScalar(); // ExecuteScalar to get a single value
                    if (result != null)
                    {
                        // Assuming the Strike_Plate is of string type, you can adjust accordingly
                        choosenLock = result.ToString();

                    }
                    else
                    {
                        MessageBox.Show("No lock found.");
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
        private void getonepiece()
        {
            MySqlConnection connection = null;
            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();

                string systemQuery = "SELECT Result FROM isonepiece";
                using (MySqlCommand plateCmd = new MySqlCommand(systemQuery, connection))
                {
                    object result = plateCmd.ExecuteScalar(); // ExecuteScalar to get a single value
                    if (result != null)
                    {
                        // Assuming the Strike_Plate is of string type, you can adjust accordingly
                        onepiece = result.ToString();

                    }
                    else
                    {
                        MessageBox.Show("No result found.");
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
        private void getMainStriker()
        {
            MySqlConnection connection = null;
            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();

                string systemQuery = "SELECT Striker_Code FROM choosenmainstrikers";
                using (MySqlCommand plateCmd = new MySqlCommand(systemQuery, connection))
                {
                    object result = plateCmd.ExecuteScalar(); // ExecuteScalar to get a single value
                    if (result != null)
                    {
                        // Assuming the Strike_Plate is of string type, you can adjust accordingly
                        choosenMainStriker = result.ToString();

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
        private void getCentralStriker()
        {
            MySqlConnection connection = null;
            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();

                string systemQuery = "SELECT Striker_Code FROM choosencentralstrikers";
                using (MySqlCommand plateCmd = new MySqlCommand(systemQuery, connection))
                {
                    object result = plateCmd.ExecuteScalar(); // ExecuteScalar to get a single value
                    if (result != null)
                    {
                        // Assuming the Strike_Plate is of string type, you can adjust accordingly
                        choosenCentralStriker = result.ToString();

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
        private void getOnePieceStriker()
        {
            MySqlConnection connection = null;
            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();

                string systemQuery = "SELECT Striker_Code FROM choosenonepiecestrikers";
                using (MySqlCommand plateCmd = new MySqlCommand(systemQuery, connection))
                {
                    object result = plateCmd.ExecuteScalar(); // ExecuteScalar to get a single value
                    if (result != null)
                    {
                        // Assuming the Strike_Plate is of string type, you can adjust accordingly
                        choosenOnepieceStriker = result.ToString();

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

    }
}
