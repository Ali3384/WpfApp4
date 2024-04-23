
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;


namespace WpfApp4.Pages
{
    /// <summary>
    /// Логика взаимодействия для Page1.xaml
    /// </summary>
    public partial class Page1 : Page
    {
        public string choosenmaterial;
        public string choosensolution;

        public Page1()
        {
            InitializeComponent();


        }







        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (material.SelectedItem != null && solution.SelectedItem != null)
            {
                if (choosenmaterial == "Aluminium" && choosensolution == "Standard")
                {
                    NavigationService.Navigate(new Page2());
                }
                else
                {
                    MessageBox.Show("It's not solution for chosen material or solution yet, please try another options");
                }
            }
            else
            {
                MessageBox.Show("Please choose all options");
            }


        }


        public void MakeVisibleButton()
        {
            if (material.SelectedItem != null && solution.SelectedItem != null)
            {
                nextbtn.IsEnabled = true;
            }
            else
            {
                nextbtn.IsEnabled = false;
            }
        }


        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            choosenmaterial = (material.SelectedItem as ComboBoxItem)?.Content.ToString();
            MakeVisibleButton();
        }

        private void solution_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            choosensolution = (solution.SelectedItem as ComboBoxItem)?.Content.ToString();
            MakeVisibleButton();
        }
    }
}
