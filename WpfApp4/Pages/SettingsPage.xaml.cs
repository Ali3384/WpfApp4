using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace WpfApp4.Pages
{

    /// <summary>
    /// Логика взаимодействия для SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        string server;
        string port;
        string uid;
        string pwd;
        string database;
        string oldconnection;
        string newconnection;
        string serverold = "";
        string portold = "";
        string uidold = "";
        string pwdold = "";
        string databaseold = "";
        public SettingsPage()
        {
            InitializeComponent();
            GetConnectionData();
            FillWithOldData();

        }

        private void butsave_Click(object sender, RoutedEventArgs e)
        {
            SetNewConnectionData();
            MessageBox.Show("Settings are saved.", "Saved", MessageBoxButton.OK, MessageBoxImage.Information);
            NavigationService.Navigate(new WelcomePage());
        }

        private void SetNewConnectionData()
        {
            StringBuilder connectionStringBuilder = new StringBuilder();
            connectionStringBuilder.Append("server=").Append(servertxt.Text).Append(";");
            connectionStringBuilder.Append("port=").Append(porttxt.Text).Append(";");
            connectionStringBuilder.Append("uid=").Append(useridtxt.Text).Append(";");
            connectionStringBuilder.Append("pwd=").Append(passwordtxt.Text).Append(";");
            connectionStringBuilder.Append("database=").Append(databasetxt.Text).Append(";");

            newconnection = connectionStringBuilder.ToString();
            Properties.Settings.Default.connection = newconnection;
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Upgrade();


        }



        private void GetConnectionData()
        {
            string oldconnection = Properties.Settings.Default.connection;

            // Split the connection string by semicolons to get individual components
            string[] parts = oldconnection.Split(';');

            // Initialize variables to hold individual components


            // Loop through the parts to extract key-value pairs
            foreach (string part in parts)
            {
                // Split each part by '=' to get key and value
                string[] keyValue = part.Split('=');
                if (keyValue.Length == 2)
                {
                    string key = keyValue[0].Trim().ToLower();
                    string value = keyValue[1].Trim();

                    // Assign values to appropriate variables based on the key
                    switch (key)
                    {
                        case "server":
                            serverold = value;
                            break;
                        case "port":
                            portold = value;
                            break;
                        case "uid":
                            uidold = value;
                            break;
                        case "pwd":
                            pwdold = value;
                            break;
                        case "database":
                            databaseold = value;
                            break;
                        default:
                            // Handle unknown keys if necessary
                            break;
                    }
                }
            }
        }
        private void FillWithOldData()
        {
            servertxt.Text = serverold;
            porttxt.Text = portold;
            useridtxt.Text = uidold;
            passwordtxt.Text = pwdold;
            databasetxt.Text = databaseold;
        }
    }
}
