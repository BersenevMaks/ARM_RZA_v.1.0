using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ARM_RZA_v._1._0
{
    public class ProgressBarInfo : INotifyPropertyChanged
    {
        

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
