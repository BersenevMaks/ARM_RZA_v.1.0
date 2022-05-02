using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace ARM_RZA_v._1._0
{
    public class Prisoed : INotifyPropertyChanged
    {
        private string prisoed;
        
        public int ID { get; set; }

        public string Pris
        {
            get { return prisoed; }
            set
            {
                prisoed = value;
                OnPropertyChanged("Pris");
            }
        }

        [ForeignKey("PS")]
        public int PSId { get; set; }

        public PS PS { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
