using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace ARM_RZA_v._1._0
{
    /// <summary>
    /// Логика взаимодействия для MGTO_Window.xaml
    /// </summary>
    public partial class MGTO_Window : Window
    {
        public MGTO_Window()
        {
            InitializeComponent();

            this.DataContext = new MGTO_View_Model();
        }
    }
}
