using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace ARM_RZA_v._1._0
{
    public class DevType : INotifyPropertyChanged
    {
        private string dev_type;
        private int? id_vendor;
        
        public int ID { get; set; }

        public string Dev_type
        {
            get { return dev_type; }
            set
            {
                dev_type = value;
                OnPropertyChanged("Dev_Type");
            }
        }

        public int? ID_vendor
        {
            get { return id_vendor; }
            set
            {
                id_vendor = value;
                OnPropertyChanged("ID_vendor");
            }
        }

        //public virtual ICollection<Device> Devices { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
