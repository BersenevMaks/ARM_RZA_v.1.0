using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ARM_RZA_v._1._0
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        //App()
        //{
        //    InitializeComponent();
        //}

        [STAThread]
        static void Main()
        {
            App app = new App();
            MainWindow window = new MainWindow();
            app.Run(window);
        }
    }
}
