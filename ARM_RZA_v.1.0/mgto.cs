using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace ARM_RZA_v._1._0
{
    public class Mgto : INotifyPropertyChanged
    {

        [Key]
        public int Id { get; set; }
        
        public string Res { get; set; }

        public string PS_type { get; set; }

        public string PS_name { get; set; }

        public string Prisoed { get; set; }

        public string Dev_name { get; set; }

        public string Dev_type { get; set; }

        public string Terminal_type { get; set; }

        public string Uprav { get; set; }

        public string Vedom { get; set; }

        public double Napr { get; set; }

        public int Year_create { get; set; }

        public int Year_start { get; set; }

        public int Cicle { get; set; }

        public int Last_year_vosst { get; set; }

        public string Y2020 { get; set; }

        public string Y2021 { get; set; }

        public string Y2022 { get; set; }

        public string Y2023 { get; set; }

        public string Y2024 { get; set; }

        public string Y2025 { get; set; }

        public string Ispol_type { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
