using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;

namespace ARM_RZA_v._1._0
{
    class Main_View_Model : INotifyPropertyChanged
    {

        public Main_View_Model()
        {
            //db = new MGTOContext();
            ////db.Devices.Load();
            //db.Mgtoes.Load();
            ////Devices = db.Devices.Local.ToBindingList();
            //Mgtoes = db.Mgtoes.Local;
            //PBarInfo = new ProgressBarInfo();
            //Visible = "Hidden";
            //SetMaxValue(100);
        }

        RelayCommand mgto_Command;
        RelayCommand ggto_Command;
        RelayCommand countRzaCommand;

        // команда открыть окно МГТО
        public RelayCommand MGTO_Command
        {
            get
            {
                return mgto_Command ??
                  (mgto_Command = new RelayCommand((o) =>
                  {
                      MGTO_Window mgtoW = new MGTO_Window();
                      mgtoW.ShowDialog();
                  }));
            }
        }

        // команда открыть окно МГТО
        public RelayCommand CountRZACommand
        {
            get
            {
                return countRzaCommand ??
                  (countRzaCommand = new RelayCommand((o) =>
                  {
                      CounterRZA_Window counterRZA_Window = new CounterRZA_Window();
                      counterRZA_Window.ShowDialog();
                  }));
            }
        }

        // команда открыть окно ГГТО
        public RelayCommand GGTO_Command
        {
            get
            {
                return ggto_Command ??
                  (ggto_Command = new RelayCommand((o) =>
                  {
                      GGTO_Window ggtoW = new GGTO_Window();
                      ggtoW.ShowDialog();
                  }));
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
