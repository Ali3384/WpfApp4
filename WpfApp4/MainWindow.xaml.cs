using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using WpfApp4.Pages;

namespace WpfApp4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Content = new WelcomePage();
            fuhrLogo.Visibility = Visibility.Visible;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

    
        private void onClosed(object sender, EventArgs e)
        {
            closing();
        }
        private void closing()
        {
            string connectionString = Properties.Settings.Default.connection;

            // Truncate tables
            TruncateTable(connectionString, "choosenlocks");
            TruncateTable(connectionString, "choosenplate");
            TruncateTable(connectionString, "choosensystem");
            TruncateTable(connectionString, "choosenmainstrikers");
            TruncateTable(connectionString, "choosenonepiecestrikers");
            TruncateTable(connectionString, "choosencentralstrikers");
            TruncateTable(connectionString, "finaltable");
            TruncateTable(connectionString, "isonepiece");
            // Close the window
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
                    
                    Close();
                }
                finally
                {
                    if (connection.State == System.Data.ConnectionState.Open)
                        connection.Close();
                }
            }
        }

        private void closeApp(object sender, MouseButtonEventArgs e)
        {
            closing();
            Close();
        }

        private void closebtn_MouseEnter(object sender, MouseEventArgs e)
        {
            SolidColorBrush blackBrush = new SolidColorBrush();
            blackBrush.Color = Colors.DarkRed;
            closebtn.Fill = blackBrush;
        }

        private void closebtn_MouseLeave(object sender, MouseEventArgs e)
        {
            SolidColorBrush redBrush = new SolidColorBrush();
            redBrush.Color = Colors.Red;
            closebtn.Fill = redBrush;
        }
    }
}
