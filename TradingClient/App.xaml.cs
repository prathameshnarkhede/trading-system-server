using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TradingClient.ViewModels;

namespace TradingClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            MainWindow = new MainWindow();
            MainWindow.DataContext = new MainWindowViewModel();

        }
    }
}
