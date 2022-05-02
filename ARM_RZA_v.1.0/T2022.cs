using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace ARM_RZA_v._1._0
{
    public class T2022 : INotifyPropertyChanged
    {
        public int ID { get; set; }

        [ForeignKey("Device")]
        public int Device_ID { get; set; }

        [ForeignKey("TO")]
        public int Vid_TO_Id { get; set; }

        public virtual Device Device { get; set; }

        public virtual TO TO { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}