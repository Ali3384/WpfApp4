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
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        public MainWindow()
        {

            InitializeComponent();
            
            MainFrame.Content = new WelcomePage();
           
            
        }

        
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        
        private void closeApp(object sender, MouseButtonEventArgs e)
        {
            string connectionString = (string)Application.Current.FindResource("MyConnectionString");
            string tableName = "choosenlocks";
            string query = $"TRUNCATE TABLE {tableName}";

            MySqlConnection connection = new MySqlConnection(connectionString);
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
            string table2Name = "choosenplate";
            string query2 = $"TRUNCATE TABLE {table2Name}";

            MySqlConnection connection2 = new MySqlConnection(connectionString);
            MySqlCommand command2 = new MySqlCommand(query2, connection2);

            try
            {
                connection2.Open();
                command2.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
            }
            string table3Name = "choosensystem";
            string query3 = $"TRUNCATE TABLE {table3Name}";

            MySqlConnection connection3 = new MySqlConnection(connectionString);
            MySqlCommand command3 = new MySqlCommand(query3, connection3);

            try
            {
                connection3.Open();
                command3.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
            }
            string table4Name = "choosenmainstrikers";
            string query4 = $"TRUNCATE TABLE {table4Name}";

            MySqlConnection connection4 = new MySqlConnection(connectionString);
            MySqlCommand command4 = new MySqlCommand(query4, connection4);

            try
            {
                connection4.Open();
                command4.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
            }
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
