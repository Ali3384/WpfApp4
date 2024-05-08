using System.Globalization;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Globalization;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;

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
        string oldconnection;
        string newconnection;
        string serverold = "";
        string portold = "";
        string uidold = "";
        string pwdold = "";
        string selectedlanguage;
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
            if(language.SelectedItem != null)
            {

                Properties.Settings.Default.selected_language = selectedlanguage;
                Properties.Settings.Default.Save();
            }
            
            //NavigationService.Navigate(new WelcomePage());
            string executable = System.Reflection.Assembly.GetExecutingAssembly().Location;
            Process.Start(executable);
            Application.Current.Shutdown();

        }

        private void SetNewConnectionData()
        {
            StringBuilder connectionStringBuilder = new StringBuilder();
            connectionStringBuilder.Append("server=").Append(servertxt.Text).Append(";");
            connectionStringBuilder.Append("port=").Append(porttxt.Text).Append(";");
            connectionStringBuilder.Append("uid=").Append(useridtxt.Text).Append(";");
            connectionStringBuilder.Append("pwd=").Append(passwordtxt.Text).Append(";");
            connectionStringBuilder.Append("charset = utf8mb4;");

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
        }
        

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(language.SelectedIndex == 0)
            {
                selectedlanguage = "en-US";
                
            }
            if (language.SelectedIndex == 1)
            {
                selectedlanguage = "ru-RU";

            }
            if (language.SelectedIndex == 2)
            {
                selectedlanguage = "pl-PL";

            }
        }

        private void backbtn_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new WelcomePage());
        }
    }
}
