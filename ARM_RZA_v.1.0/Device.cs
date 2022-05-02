using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace ARM_RZA_v._1._0
{
    public class Device : INotifyPropertyChanged
    {
        private string res;
        private string ps_type;
        private string dev_name;
        private string terminal_type;
        private string uprav;
        private string vedom;
        private double napr;
        private int year_create;
        private int year_start;
        private int cicle;
        private int last_year_vosst;
        private string ispol_type;

        public int ID { get; set; }

        public string Res
        {
            get { return res; }
            set
            {
                res = value;
                OnPropertyChanged("Res");
            }
        }

        public string PS_type
        {
            get { return ps_type; }
            set
            {
                ps_type = value;
                OnPropertyChanged("PS_type");
            }
        }

        public string Dev_name
        {
            get { return dev_name; }
            set
            {
                dev_name = value;
                OnPropertyChanged("Dev_name");
            }
        }

        [ForeignKey("DevType")] public int Dev_typeId { get; set; }

        [ForeignKey("Prisoed")] public int PrisoedId { get; set; }


        //public string Dev_type
        //{
        //    get { return dev_type; }
        //    set
        //    {
        //        dev_type = value;
        //        OnPropertyChanged("Dev_type");
        //    }
        //}

        public string Terminal_type
        {
            get { return terminal_type; }
            set
            {
                terminal_type = value;
                OnPropertyChanged("Terminal_type");
            }
        }

        public string Uprav
        {
            get { return uprav; }
            set
            {
                uprav = value;
                OnPropertyChanged("Uprav");
            }
        }

        public string Vedom
        {
            get { return vedom; }
            set
            {
                vedom = value;
                OnPropertyChanged("Vedom");
            }
        }

        public double Napr
        {
            get { return napr; }
            set
            {
                napr = value;
                OnPropertyChanged("Napr");
            }
        }

        public int Year_create
        {
            get { return year_create; }
            set
            {
                year_create = value;
                OnPropertyChanged("Year_create");
            }
        }

        public int Year_start
        {
            get { return year_start; }
            set
            {
                year_start = value;
                OnPropertyChanged("Year_start");
            }
        }

        public int Cicle
        {
            get { return cicle; }
            set
            {
                cicle = value;
                OnPropertyChanged("Cicle");
            }
        }

        public int Last_year_vosst
        {
            get { return last_year_vosst; }
            set
            {
                last_year_vosst = value;
                OnPropertyChanged("Last_year_vosst");
            }
        }

        public string Ispol_type
        {
            get { return ispol_type; }
            set
            {
                ispol_type = value;
                OnPropertyChanged("Ispol_type");
            }
        }
        
        public virtual DevType DevType { get; set; }

        public virtual Prisoed Prisoed { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
