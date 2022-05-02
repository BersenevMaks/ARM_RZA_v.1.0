using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ARM_RZA_v._1._0
{
    public class TO : INotifyPropertyChanged
    {
        public int ID { get; set; }

        private string vid_TO;

        public string Vid_TO
        {
            get { return vid_TO; }
            set
            {
                vid_TO = value;
                OnPropertyChanged("Vid_TO");
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
