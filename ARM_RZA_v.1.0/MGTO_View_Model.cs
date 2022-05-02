using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace ARM_RZA_v._1._0
{
    class MGTO_View_Model : INotifyPropertyChanged
    {
        MGTOContext db;
        RelayCommand addCommand;
        RelayCommand editCommand;
        RelayCommand deleteCommand;
        RelayCommand loadCommand;
        //IEnumerable<Device> devices;
        IEnumerable<Mgto> mgtoes;
        public ProgressBarInfo PBarInfo;

        //public IEnumerable<Device> Devices
        //{
        //    get { return devices; }
        //    set
        //    {
        //        devices = value;
        //        OnPropertyChanged("Devices");
        //    }
        //}
        public IEnumerable<Mgto> Mgtoes
        {
            get { return mgtoes; }
            set
            {
                mgtoes = value;
                OnPropertyChanged("Mgtoes");
            }
        }

        public MGTO_View_Model()
        {
            db = new MGTOContext();
            //db.Devices.Load();
            db.Mgtoes.Load();
            //Devices = db.Devices.Local.ToBindingList();
            Mgtoes = db.Mgtoes.Local;
            PBarInfo = new ProgressBarInfo();
            Visible = "Hidden";
            SetMaxValue(100);
        }

        int p = 0;
        // команда добавления
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                  (addCommand = new RelayCommand((o) =>
                  {
                      //DeviceWindow deviceWindow = new DeviceWindow(new Device());
                      //if (deviceWindow.ShowDialog() == true)
                      //{
                      //    Device device = deviceWindow.Device;
                      //    db.Devices.Add(device);
                      //    db.SaveChanges();
                      //}
                  }));
            }
        }
        // команда редактирования
        public RelayCommand EditCommand
        {
            get
            {
                return editCommand ??
                  (editCommand = new RelayCommand((selectedItem) =>
                  {
                      //// если ни одного объекта не выделено, выходим
                      //if (selectedItem == null) return;
                      //// получаем выделенный объект
                      //Device device = selectedItem as Device;

                      //DeviceWindow deviceWindow = new DeviceWindow(new Device
                      //{
                      //    ID = device.ID,
                      //    Dev_name = device.Dev_name
                      //    //Dev_type = device.Dev_type
                      //});

                      //if (deviceWindow.ShowDialog() == true)
                      //{
                      //    // получаем измененный объект
                      //    device = db.Devices.Find(deviceWindow.Device.ID);
                      //    if (device != null)
                      //    {
                      //        device.Dev_name = deviceWindow.Device.Dev_name;
                      //        //device.Dev_type = deviceWindow.Device.Dev_type;
                      //        db.Entry(device).State = EntityState.Modified;
                      //        db.SaveChanges();
                      //    }
                      //}
                  }));
            }
        }
        // команда удаления
        public RelayCommand DeleteCommand
        {
            get
            {
                return deleteCommand ??
                  (deleteCommand = new RelayCommand((selectedItem) =>
                  {
                      //// если ни одного объекта не выделено, выходим
                      //if (selectedItem == null) return;
                      //// получаем выделенный объект
                      //Device device = selectedItem as Device;
                      //db.Devices.Remove(device);
                      //db.SaveChanges();
                  }));
            }
        }

        // команда добавления из файла Excel
        public RelayCommand LoadCommand
        {
            get
            {
                return loadCommand ??
                  (loadCommand = new RelayCommand((o) =>
                  {
                      LoadDevices loadDevices = new LoadDevices();

                      loadDevices.ProcessChanged += IncrementProgress;
                      loadDevices.SetMaxValProgressBar += SetMaxValue;
                      loadDevices.WorkComplite += Complite;
                      Visible = "Visible";
                      Thread thread = new Thread(loadDevices.CreateFromExcel);
                      thread.Start();
                      //// Отключаем отслеживание и проверку изменений для оптимизации вставки множества полей
                      //db.Configuration.AutoDetectChangesEnabled = false;
                      //db.Configuration.ValidateOnSaveEnabled = false;
                      //foreach (Device device in loadDevices.GetInfo())
                      //{
                      //    db.Devices.Add(device);
                      //}
                      //db.SaveChanges();
                      //db.Configuration.AutoDetectChangesEnabled = true;
                      //db.Configuration.ValidateOnSaveEnabled = true;
                  }));
            }
        }

        private int progressValue;
        private int maxValue;
        private string visible;

        public string Visible
        {
            get { return visible; }
            set
            {
                visible = value;
                OnPropertyChanged("Visible");
            }
        }

        public int ProgressValue
        {
            get { return progressValue; }
            set
            {
                progressValue = value;
                OnPropertyChanged("ProgressValue");
            }
        }

        public int MaxValue
        {
            get { return maxValue; }
            set
            {
                maxValue = value;
                OnPropertyChanged("MaxValue");
            }
        }

        public void IncrementProgress(int progress)
        {
            ProgressValue = progress;
        }

        public void SetMaxValue(int value)
        {
            MaxValue = value;
        }

        public void Complite(int status)
        {
            Visible = "Hidden";
            if(status == 0)
                MessageBox.Show("Завершено с ошибками");
            else MessageBox.Show("Успешно");
            Dispatcher.CurrentDispatcher.BeginInvoke(new System.Action(() =>
            {
                db.Mgtoes.Load();
                Mgtoes = db.Mgtoes.Local;
            }));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
