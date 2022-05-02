using System.Windows;

namespace ARM_RZA_v._1._0
{
    /// <summary>
    /// Логика взаимодействия для DeviceWindow.xaml
    /// </summary>
    public partial class DeviceWindow : Window
    {
        public Device Device { get; private set; }

        public DeviceWindow(Device d)
        {
            InitializeComponent();
            Device = d;
            this.DataContext = Device;
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
