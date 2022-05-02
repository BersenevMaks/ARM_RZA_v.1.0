using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using Word = Microsoft.Office.Interop.Word;

namespace ARM_RZA_v._1._0
{
    class CounterRelayFromDocs
    {

        private SQLiteConnection connection;

        private List<string> files;
        private List<RelayDevice_Class> relayDevices;

        SQL_Base sql_base;

        // поля для и переменные для ворда
        Word._Application application;
        Word._Document document;
        Object missingObj = System.Reflection.Missing.Value;
        Object trueObj = true;
        Object falseObj = false;
        bool isOpenWord = false;
        //======


        public void Run()
        { getFilesFromDocDirectory(); }

        public string path = "";
        /// <summary>
        /// Заполнение списка файлов из папки
        /// </summary>

        private void getFilesFromDocDirectory()
        {
            try
            {
                //if (Directory.Exists(path) == false)
                //{
                //    Directory.CreateDirectory(md + "\\MetBaseFiles\\" + textOfCity);
                //}
                if (!string.IsNullOrEmpty(path))
                {
                    string[] searchPatterns = "*.doc?".Split('|');//"*.xls?|*.doc?|*.pdf".Split('|');
                    files = new List<string>();
                    ObservableCollection<FileListItem> fileListItems = new ObservableCollection<FileListItem>();

                    foreach (string sp in searchPatterns)
                    {

                        files = Directory.GetFiles(path, sp, SearchOption.AllDirectories).Where(x => new FileInfo(x).LastWriteTime.Date > new DateTime(2018, 1, 1)).ToList();
                        foreach (string _file in files)
                        {
                            fileListItems.Add(new FileListItem
                            {
                                FilePath = _file,
                                Date = new FileInfo(_file).LastWriteTime.Date.ToShortDateString()
                            });
                        }
                    }
                    files.Sort();

                    //вызов события для отображения во вьюшке максимального количества найденных файлов
                    SetMaxCountFiles(files.Count);

                    //установка текущего обрабатываемого элемента
                    SetCurrentNumbOfItem(0);

                    //вызов события для отображения во вьюшке
                    SetFileListItems(fileListItems);

                    SetTextInfo("Подключение к базе данных...");
                    connection = new SQLiteConnection("Data Source=armbase.db; version=3");
                    connection.Open();
                    SetTextInfo("Подключение к базе установлено.");
                    
                    // обработка ворда
                    Process();

                    if (connection.State == System.Data.ConnectionState.Open) connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка №1\n" + ex.ToString());
                if (connection.State == System.Data.ConnectionState.Open) connection.Close();
            }

        }

        private void Process()
        {
            //
            if (files.Count > 0)
            {
                try
                {
                    int currentFile = 1;
                    foreach (string _file in files)
                    {
                        SetTextInfo("Обрабатывается файл " + _file);
                        SetCurrentNumbOfItem(currentFile++);
                        GetInfoFromWord(_file);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка №2\n" + ex.ToString());
                }
            }
            if (connection.State == System.Data.ConnectionState.Open) connection.Close();
            MessageBox.Show("Обработка завершена");
        }

        private void GetInfoFromWord(string filePath)
        {
            //создаем обьект приложения word
            application = new Word.Application();
            // создаем путь к файлу
            Object templatePathObj = filePath;
            Word.Tables tables;
            string temp = "";
            string ps_power = "";
            string ps_name = "";
            string prisoed = "";
            string date = "";

            RelayDevice_Class relayDevice;
            relayDevices = new List<RelayDevice_Class>();

            int ColName = 0; //колонка "наименование"
            int ColType = 0; //колонка "тип"

            int StartRow = 0; //начальное значение для поиска типов реле

            try
            {
                //orgname = new Regex(@".+(?=[\s_\.]\d+[\._]\d+[\._]\d+\.[\w\d]{3,4}$)|(?<=[\\/]|^)[\w\s]+(?=\.xlsx?)").Match(Path.GetFileName(filePath)).Value;
                //infoOrg.OrgName = orgname;

                document = application.Documents.Open(ref templatePathObj, ref missingObj,
                    ref missingObj, ref missingObj, ref missingObj, ref missingObj,
                    ref missingObj, ref missingObj, ref missingObj, ref missingObj,
                    ref missingObj, ref missingObj, ref missingObj, ref missingObj,
                    ref missingObj, ref missingObj);

                tables = document.Tables;
                isOpenWord = true;

                int paragraph = 5;
                if (paragraph > document.Paragraphs.Count) paragraph = document.Paragraphs.Count;

                for (int i = 1; i <= paragraph; i++)
                {
                    if (string.IsNullOrEmpty(ps_name))
                        ps_name = new Regex(@"(?<=ПС\s*(?:[1,2,3][1,2,5][0]?\s*[\\,\/]+\s*(?:6|[1,3][5,0])(?:[\\,\/]+\s*(?:10|6))*?\s*.?)?)\w+(?=[\s$,\.])", RegexOptions.IgnoreCase).Match(document.Paragraphs[i].Range.Text.Replace('"', ' ')).Value.ToString();

                    if (string.IsNullOrEmpty(ps_power))
                        ps_power = new Regex(@"ПС\s*[1,2,3][1,2,5][0]?\s*[\\,\/]+\s*(?:6|[1,3][5,0])(?:[\\,\/]+\s*(?:10|6))*?(?=\s*.?\w+.?)", RegexOptions.IgnoreCase).Match(document.Paragraphs[i].Range.Text.Replace('"', ' ')).Value.ToString();

                    if (string.IsNullOrEmpty(date))
                        date = new Regex(@"(?<=[пП]ротокол\s+от\s*)\d\d\.\d\d\.(?:\d\d\d\d|\d\d)(?=\s|г|$)", RegexOptions.IgnoreCase).Match(document.Paragraphs[i].Range.Text).Value.ToString();

                }

                for (int t = 1; t <= tables.Count; t++)
                {
                    ColName = 0;
                    ColType = 0;

                    Word.Table wTab = tables[t];
                    int cCelCol = wTab.Columns.Count;
                    int cCelRow = wTab.Rows.Count;

                    int Max = cCelCol * cCelRow;

                    for (int j = 1; j <= cCelRow; j++) //строки
                    {
                        int jj = j;
                        for (int i = 1; i <= cCelCol; i++) //столбцы
                        {

                            try
                            {
                                temp = wTab.Cell(jj, i).Range.Text;
                            }
                            catch { continue; }
                            temp = temp.Replace("\r\a", string.Empty).Trim();
                            if (new Regex(@"^назнач", RegexOptions.IgnoreCase).IsMatch(temp))
                            { ColName = i; j = cCelRow; StartRow = jj; }
                            else if (new Regex(@"Тип", RegexOptions.IgnoreCase).IsMatch(temp))
                            {
                                ColType = i;
                                break;
                            }
                        }
                    }

                    if (ColName > 0 && ColType > 0)
                    {
                        for (int jj = StartRow + 1; jj <= cCelRow; jj++) //строки со следующей после заголовка
                        {
                            relayDevice = new RelayDevice_Class();
                            try
                            {
                                temp = wTab.Cell(jj, ColName).Range.Text;
                            }
                            catch { continue; }

                            temp = temp.Replace("\r\a", string.Empty).Trim();
                            if (temp != "")
                            {
                                relayDevice.Purpose = new Regex(@"\s\s+", RegexOptions.IgnoreCase).Replace(temp, "");
                            }

                            try
                            {
                                temp = wTab.Cell(jj, ColType).Range.Text;
                            }
                            catch { continue; }
                            temp = temp.Replace("\r\a", string.Empty).Trim();
                            if (temp != "")
                            {
                                relayDevice.RelayType = new Regex(@"\s", RegexOptions.IgnoreCase).Replace(temp, "");
                            }

                            if (!string.IsNullOrEmpty(ps_power))
                                relayDevice.PS_power = ps_power;

                            if (!string.IsNullOrEmpty(ps_name))
                                relayDevice.PS_name = ps_name;

                            if (!string.IsNullOrEmpty(date))
                                relayDevice.Date = date;

                            if (!string.IsNullOrEmpty(relayDevice.RelayType) && !string.IsNullOrEmpty(relayDevice.Purpose))
                            {
                                relayDevices.Add(relayDevice);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка №3\n" + ex.ToString());
                if (isOpenWord)
                {
                    document.Close();
                    application.Quit(missingObj, missingObj, missingObj);
                    isOpenWord = false;
                }
            }
            if (isOpenWord)
            {
                document.Close();
                application.Quit(missingObj, missingObj, missingObj);
                isOpenWord = false;
            }

            //Запись в базу данных перечня найденных реле в отдельном потоке
            sql_base = new SQL_Base();

            //подписка на событие обновления списка найденных реле в документах
            sql_base.SendRelayCollection += SendRelayCollection;

            //запуск нового потока для работы с базой данных
            Thread threadWriteToBase = new Thread(new ParameterizedThreadStart(sql_base.WriteRelayDevices));
            threadWriteToBase.Start(relayDevices);
            
        }

        private void SendRelayCollection(ObservableCollection<CountRalayTypeItems> CountRelayCollection)
        {
            SendCountReleyCollection(CountRelayCollection);
        }

        /// <summary>
        /// событие для установки номера текущего обрабатываемого элемента
        /// </summary>
        public event Action<int> SetCurrentNumbOfItem; //установить текущее значение прогрессбара

        /// <summary>
        /// событие для установки общего количества найденных файлов
        /// </summary>
        public event Action<int> SetMaxCountFiles; //установить максимальное значение для прогрессбара

        /// <summary>
        /// событие для отображения общей информации в статусной строке
        /// </summary>
        public event Action<string> SetTextInfo; //

        /// <summary>
        /// Событие для отображения списка найденных файлов во всех указанных папках
        /// </summary>
        public event Action<ObservableCollection<FileListItem>> SetFileListItems;

        /// <summary>
        /// Событие для отображения списка с подсчетом реле из обрабатываемых документов
        /// </summary>
        public event Action<ObservableCollection<CountRalayTypeItems>> SendCountReleyCollection;
    }
}

