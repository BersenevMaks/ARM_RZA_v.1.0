using System;
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

namespace ARM_RZA_v._1._0
{
    /// <summary>
    /// Логика взаимодействия для CounterRZA_Window.xaml
    /// </summary>
    
    public partial class CounterRZA_Window : Window
    {

        public CounterRZA_Window()
        {
            InitializeComponent();

            this.DataContext = new CounterRZA_View_Model();
            
        }
    }
}
