using System.Globalization;
using System.Threading;
using System.Windows;
using System.Diagnostics;
using WpfApp4.Properties;
namespace WpfApp4
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        string selectedlanguage;
        protected override void OnStartup(StartupEventArgs e)
        {
            
            base.OnStartup(e);
            selectedlanguage = WpfApp4.Properties.Settings.Default.selected_language;
            if(selectedlanguage == null)
            {
                selectedlanguage = "en-EN";
            }
           
                Thread.CurrentThread.CurrentCulture = new CultureInfo(selectedlanguage);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(selectedlanguage);
            
           
        }
    }
}
