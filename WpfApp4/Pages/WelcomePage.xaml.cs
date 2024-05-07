using MySql.Data.MySqlClient;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace WpfApp4.Pages
{
    /// <summary>
    /// Логика взаимодействия для WelcomePage.xaml
    /// </summary>
    public partial class WelcomePage : Page
    {
        bool isconnection = false;
        public WelcomePage()
        {
            InitializeComponent();
        }

        private void checkConnection()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(Properties.Settings.Default.connection + "database=alu_standard;"))
                {
                    connection.Open();
                    isconnection = true;
                }
            }
            catch
            {

                isconnection = false;
            }
        }
        private void but1_Click(object sender, RoutedEventArgs e)
        {
            checkConnection();
            if (isconnection)
            {
                string connectionString = Properties.Settings.Default.connection + "database=alu_standard;";

                // Truncate tables
                TruncateTable(connectionString, "choosenlocks");
                TruncateTable(connectionString, "choosenplate");
                TruncateTable(connectionString, "choosensystem");
                TruncateTable(connectionString, "choosenmainstrikers");
                TruncateTable(connectionString, "choosenonepiecestrikers");
                TruncateTable(connectionString, "choosencentralstrikers");
                TruncateTable(connectionString, "finaltable");
                TruncateTable(connectionString, "isonepiece");
                NavigationService.Navigate(new Page1());
            }
            else
            {
                MessageBox.Show("Error while connecting to Database, please check settings", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                NavigationService.Navigate(new SettingsPage());
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
        private void butsettings_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SettingsPage());
        }
    }
}
