using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace ARM_RZA_v._1._0
{
    public class PS : INotifyPropertyChanged
    {
        private string ps_name;

        public int ID { get; set; }

        public string PS_name
        {
            get { return ps_name; }
            set
            {
                ps_name = value;
                OnPropertyChanged("PS_name");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
