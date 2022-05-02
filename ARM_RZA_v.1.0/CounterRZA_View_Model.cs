using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Data;

namespace ARM_RZA_v._1._0
{
    class CounterRZA_View_Model : INotifyPropertyChanged
    {
        public CounterRZA_View_Model()
        {
            TextPath = @"D:\_Проекты\ARM_RZA_v.1.0\ARM_RZA_v.1.0\bin\Debug";
        }

        RelayCommand countRzaCommand;
        RelayCommand getPathCommand;

        private string textCountInfo;
        private string textInfo;
        private int CountFiles;
        private string path;


        public string TextPath
        {
            get { return path; }
            set
            {
                path = value;
                OnPropertyChanged("TextPath");
            }
        }

        public string TextCountInfo
        {
            get { return textCountInfo; }
            set
            {
                textCountInfo = value;
                OnPropertyChanged("TextCountInfo");
            }
        }
        public string TextInfo
        {
            get { return textInfo; }
            set
            {
                textInfo = value;
                OnPropertyChanged("TextInfo");
            }
        }
        private void SetTextCountInfo(int current)
        {
            TextCountInfo = "Файл № " + current + " из " + CountFiles;
        }

        private void SetCountFiles(int count)
        {
            CountFiles = count;
        }

        private void SetTextInfo(string text)
        {
            TextInfo = text;
        }

        //Коллекция для байндинга к гриду со списком файлов
        public ObservableCollection<FileListItem> FilesList { get; private set; }// = new BindingList<FileListItem>();
        private void SetFileListItems(ObservableCollection<FileListItem> fileListItems)
        {
            FilesList = fileListItems;
            OnPropertyChanged("FilesList");
        }

        //Коллекция для байндинга к гриду с подсчетом реле
        public ObservableCollection<CountRalayTypeItems> CountRelayCollection { get; private set; }
        private void SetCountRelayCollection (ObservableCollection<CountRalayTypeItems> countRalayCollection)
        {
            CountRelayCollection = countRalayCollection;
            OnPropertyChanged("CountRelayCollection");
        }

        // команда подсчет 
        public RelayCommand Count_Command
        {
            get
            {
                return countRzaCommand ??
                  (countRzaCommand = new RelayCommand((o) =>
                  {

                      if (!string.IsNullOrEmpty(TextPath))
                      {
                          CounterRelayFromDocs counterRelayFromDocs = new CounterRelayFromDocs();
                          counterRelayFromDocs.path = TextPath;

                          counterRelayFromDocs.SetCurrentNumbOfItem += SetTextCountInfo;
                          counterRelayFromDocs.SetMaxCountFiles += SetCountFiles;
                          counterRelayFromDocs.SetTextInfo += SetTextInfo;
                          counterRelayFromDocs.SetFileListItems += SetFileListItems;
                          counterRelayFromDocs.SendCountReleyCollection += SetCountRelayCollection;

                          Thread thread = new Thread(counterRelayFromDocs.Run);
                          thread.Start();
                      }
                  }));
            }
        }

        // команда открыть окно МГТО
        public RelayCommand GetPath_Command
        {
            get
            {
                return getPathCommand ??
                  (getPathCommand = new RelayCommand((o) =>
                  {

                      //Окно Открыть для указания папки

                      var dialog = new System.Windows.Forms.FolderBrowserDialog();

                      if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                      {
                          TextPath = dialog.SelectedPath;
                      }
                  }));
            }
        }

        public string TextCountInfo1 { get => textCountInfo; set => textCountInfo = value; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
