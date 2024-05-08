﻿using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using WpfApp4.Pages;

namespace WpfApp4
{
    /// <summary>
    /// Логика взаимодействия для Basket.xaml
    /// </summary>
    public partial class Basket : Window
    {
        public Basket()
        {
            
            InitializeComponent();
            MainFrame.Content = new basketpage();
            fuhrLogo.Visibility = Visibility.Visible;
        }
        private void closeApp(object sender, MouseButtonEventArgs e)
        {
            Close();
        }
    }
}
