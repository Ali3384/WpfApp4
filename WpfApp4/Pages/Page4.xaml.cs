using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Text;
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

        public string choosenSystem { get; set; }
        string isAsymmetric;
        string choosentype;
        string typeofLock;
        string heightOfLock;
        int check;
        public int check2 { get; set; }
        public int checkfrompage4 { get; set; }
        public string choosenonepiece { get; set; }
        public string str { get; set; }
        public string choosenside { get; set; }
        public string eopenerfrompage3;
        bool eOpenerExists = false;
        bool addLockExists = false;
        string addlockstriker;
        public Page4()
        {


            InitializeComponent();
            Page3 page3 = new Page3();
            eopenerfrompage3 = page3.eopener;

            GetLockinfo();
            GetLockHeightinfo();
            GetPlateForStriker();
            GetSysteminfo();
            getIsAsymmetric();
            InsertIntoChoosenMainStrikers();
            InsertIntoChoosenCentralStrikers();
            InsertIntoChoosenOnePieceStrikers();
            getIsOnePiece();
            getIsOnePiece2();
            FillOnePieceComboBox();
            CheckIfEOpenerExists();
            checkAddLock();
        }



        string connectionString = Properties.Settings.Default.connection;

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
        public void CheckIfEOpenerExists()
        {


            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM finaltable WHERE Item_Description = @ItemDescription";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    byte[] eopener = Encoding.UTF8.GetBytes(Properties.Resources.adde_opener);
                    command.Parameters.AddWithValue("@ItemDescription", eopener);

                    try
                    {
                        connection.Open();
                        int count = Convert.ToInt32(command.ExecuteScalar());
                        eOpenerExists = count > 0;
                    }
                    catch (MySqlException ex)
                    {
                        // Handle exceptions here
                        Console.WriteLine("An error occurred: " + ex.Message);
                    }
                }
            }

        }
        private void addStrikerForAdditionalLock()
        {

            if (strikePlate == "U24x8,5")
            {
                addlockstriker = "VRNC58590";
            }
            else if (strikePlate == "U24x6,5")
            {
                addlockstriker = "VRNC58912";
            }
            else if (strikePlate == "U24x5")
            {
                addlockstriker = "VRNC54138";
            }
            else if (isAsymmetric == "Yes" && strikePlate == "U24x5")
            {
                addlockstriker = "VRNC54138";
            }
            else if (strikePlate == "U22x5")
            {
                addlockstriker = "VRNC57285";
            }
            else if (strikePlate == "F24")
            {
                addlockstriker = "VRNC48240";
            }
            string connectionString = Properties.Settings.Default.connection;
            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();

                string insertQuery = "INSERT INTO finaltable (Item_Code, Item_Description, Quantity) VALUES (@addlockstriker, @striker_addlock, '1')";
                using (MySqlCommand insertCmd = new MySqlCommand(insertQuery, connection))
                {
                    byte[] striker_addlock = Encoding.UTF8.GetBytes(Properties.Resources.striker_for_addlock);
                    insertCmd.Parameters.AddWithValue("@addlockstriker", addlockstriker);
                    insertCmd.Parameters.AddWithValue("@striker_addlock", striker_addlock);
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
        private void checkAddLock()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM finaltable WHERE Item_Description = @ItemDescription";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    byte[] addlock = Encoding.UTF8.GetBytes(Properties.Resources.add_lock_insert);
                    command.Parameters.AddWithValue("@ItemDescription", addlock);

                    try
                    {
                        connection.Open();
                        int count = Convert.ToInt32(command.ExecuteScalar());
                        addLockExists = count > 0;
                    }
                    catch (MySqlException ex)
                    {
                        // Handle exceptions here
                        Console.WriteLine("An error occurred: " + ex.Message);
                    }
                }
            }
        }

        private void addStrikerForUpperBolt()
        {
            string insertQuery;
            if (strikePlate == "F24")
            {
                insertQuery = "INSERT INTO finaltable (Item_Code, Item_Description, Quantity) VALUES ('VRNR58613', @striker_upper, '1')";
            }
            else if (strikePlate == "U24x8,5")
            {
                insertQuery = "INSERT INTO finaltable (Item_Code, Item_Description, Quantity) VALUES ('VRNR58608', @striker_upper, '1')";
            }
            else
            {
                insertQuery = "INSERT INTO finaltable (Item_Code, Item_Description, Quantity) VALUES ('VRNR58610', @striker_upper, '1')";
            }
            string connectionString = Properties.Settings.Default.connection;
            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();


                using (MySqlCommand insertCmd = new MySqlCommand(insertQuery, connection))
                {
                    byte[] upperboltstriker = Encoding.UTF8.GetBytes(Properties.Resources.upperboltstriker);
                    insertCmd.Parameters.AddWithValue("@striker_upper", upperboltstriker);
                    insertCmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while inserting striker for round rod into final table: " + ex.Message);
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
                        MessageBox.Show("No system found in choosensystem.");
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
                        MessageBox.Show("No Type found.");
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
                        MessageBox.Show("No Height found.");
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
        public void getIsOnePiece2()
        {
            MySqlConnection connection = null;
            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();

                string selectQuery = "SELECT COUNT(*) FROM finaltable";
                MySqlCommand command = new MySqlCommand(selectQuery, connection);
                check2 = Convert.ToInt32(command.ExecuteScalar());
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
                string connectionString = Properties.Settings.Default.connection;

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
                string connectionString = Properties.Settings.Default.connection;

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
                string connectionString = Properties.Settings.Default.connection;

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
        private void setOnePiece()
        {


            MySqlConnection connection = null;
            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();

                string insertQuery = "INSERT INTO isonepiece (Result) VALUES (@result)";
                using (MySqlCommand insertCmd = new MySqlCommand(insertQuery, connection))
                {
                    insertCmd.Parameters.AddWithValue("@result", choosenonepiece);

                    insertCmd.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while inserting data into isonepiece table: " + ex.Message);
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
            if (onepiececombobox.SelectedItem != null && sidecombobox.SelectedItem != null && typecombobox.SelectedItem != null)
            {
                if (addLockExists == true && choosenonepiece == "Single Strikers")
                {
                    addStrikerForAdditionalLock();
                }
                if (Properties.Settings.Default.leaf == "2leaf")
                {
                    addStrikerForUpperBolt();
                }
                if (eOpenerExists == true)
                {
                    string insertQuery = "";
                    if (strikePlate != "F24")
                    {
                        insertQuery = "INSERT INTO finaltable (Item_Code, Item_Description, Quantity) VALUES ('VRYR14761', @addlatchslide, '1')";
                    }
                    else if (choosenSystem == "WICONA WICSTYLE 75 evo")
                    {
                        insertQuery = "INSERT INTO finaltable (Item_Code, Item_Description, Quantity) VALUES ('VRYR59300', @addlatchslide, '1')";
                    }
                    else
                    {
                        insertQuery = "INSERT INTO finaltable (Item_Code, Item_Description, Quantity) VALUES ('VRYR69641', @addlatchslide, '1')";
                    }
                    string connectionString = Properties.Settings.Default.connection;
                    try
                    {
                        connection = new MySqlConnection(connectionString);
                        connection.Open();


                        using (MySqlCommand insertCmd = new MySqlCommand(insertQuery, connection))
                        {
                            byte[] addlatchslide = Encoding.UTF8.GetBytes(Properties.Resources.e_opener_latchslide);
                            insertCmd.Parameters.AddWithValue("@addlatchslide", addlatchslide);
                            insertCmd.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error while inserting e-opener latch slide data: " + ex.Message);
                    }
                    finally
                    {
                        if (connection != null && connection.State == ConnectionState.Open)
                        {
                            connection.Close();
                        }
                    }
                }
                setOnePiece();
                mainstrikeget();
                onepieceget();
                centralstrikeget();
                str = choosenonepiece;
                checkfrompage4 = check2;
                NavigationService.Navigate(new FinalPage());
            }
            else
            {
                MessageBox.Show("You didn't fill all needed data.");
            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        public void onepiececombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            choosenonepiece = onepiececombobox.SelectedItem.ToString();

            typecombobox.Items.Clear();
            FillTypeComboBox();
        }

        private void sidecombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sidecombobox.SelectedItem != null)
            {
                choosenside = sidecombobox.SelectedItem.ToString();
            }
        }

        private void inside_outsidecombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
