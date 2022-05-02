using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Windows;
using Excel = Microsoft.Office.Interop.Excel;

namespace ARM_RZA_v._1._0
{
    public class LoadDevices
    {
        MGTOContext db;
        public void CreateFromExcel()
        {
            db = new MGTOContext();
            db.Devices.Load();
            db.DevTypes.Load();
            db.Prisoeds.Load();
            db.PS.Load();
            db.T2020.Load();
            db.T2021.Load();
            db.T2022.Load();
            db.T2023.Load();
            db.T2024.Load();
            db.T2025.Load();

            // Получить объект приложения Excel.
            Excel.Application excel_app = new Excel.Application();
            try
            {
                // Сделать Excel видимым (необязательно).
                excel_app.Visible = false;

                // Откройте рабочую книгу только для чтения.
                Excel.Workbook workbook = excel_app.Workbooks.Open(
                    Directory.GetCurrentDirectory() + "\\mgto.xlsx",
                    Type.Missing, true, Type.Missing, Type.Missing,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                    Type.Missing, Type.Missing);

                // Получить первый рабочий лист.
                Excel.Worksheet sheet = (Excel.Worksheet)workbook.Sheets[1];

                //
                List<Device> Devices = new List<Device>();

                string[] temp = new string[21];

                //Чтение данных
                Excel.Range range;
                Excel.Range last = sheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
                int lr = last.Row; //last row
                lr = 500;
                SetMaxValProgressBar(lr);
                for (int i = 3; i <= lr; i++)
                {
                    int x = 0;
                    for (int j = 2; j <= 7; j++)
                    {
                        range = (Excel.Range)sheet.Cells[i, j];
                        if (range != null)
                        {
                            if (range.Value2 != null)
                            {
                                temp[x] = range.Value2;
                            }
                            else temp[x] = "";
                            x++;
                        }
                    }
                    range = (Excel.Range)sheet.Cells[i, 9];
                    if (range != null)
                    {
                        if (range.Value2 != null)
                        {
                            temp[x] = range.Value2;
                        }
                        else temp[x] = "";
                        x++;
                    }

                    for (int j = 12; j <= 18; j++)
                    {
                        range = (Excel.Range)sheet.Cells[i, j];
                        if (range != null)
                        {
                            if (range.Value2 != null)
                            {
                                temp[x] = range.Value2.ToString();
                            }
                            else temp[x] = "0";
                            x++;
                        }
                    }

                    range = (Excel.Range)sheet.Cells[i, 54];
                    if (range != null)
                    {
                        if (range.Value2 != null)
                        {
                            temp[x] = range.Value2;
                        }
                        else temp[x] = "";
                        x++;
                    }

                    if (temp.Length > 0)
                        if (temp[3] != null && temp[3] != "0")
                        {
                            //обработка "ПС"
                            string ps = temp[2];
                            PS Ps = db.PS.Where(c => c.PS_name == ps).FirstOrDefault();
                            if (Ps == null)
                            {
                                Ps = new PS()
                                {
                                    PS_name = string.IsNullOrEmpty(temp[2]) ? "Нет данных" : temp[2]
                                };

                                db.PS.Add(Ps);
                                db.Entry(Ps).State = EntityState.Added;
                                db.SaveChanges();
                            }
                            //обработка "Присоединения"
                            string pris = temp[3];
                            Prisoed prisoed = db.Prisoeds.Where(c => c.Pris == pris || c.PSId == Ps.ID).FirstOrDefault();
                            if (prisoed == null)
                            {
                                prisoed = new Prisoed()
                                {
                                    Pris = string.IsNullOrEmpty(temp[3]) ? "Нет данных" : temp[3],
                                    PSId = Ps.ID
                                };

                                db.Prisoeds.Add(prisoed);
                                db.Entry(prisoed).State = EntityState.Added;
                                db.SaveChanges();
                            }

                            //обработка "Типа УРЗА"
                            string type = temp[5];
                            DevType devType = db.DevTypes.Where(c => c.Dev_type == type).FirstOrDefault();
                            if (devType == null)
                            {
                                devType = new DevType()
                                {
                                    Dev_type = string.IsNullOrEmpty(temp[5]) ? "Нет данных" : temp[5]
                                };

                                db.DevTypes.Add(devType);
                                db.Entry(devType).State = EntityState.Added;
                                db.SaveChanges();
                            }

                            var device = new Device()
                            {
                                Res = temp[0],
                                PS_type = temp[1],
                                PrisoedId = prisoed.ID,
                                Dev_name = temp[4],
                                Dev_typeId = devType.ID,
                                Terminal_type = temp[6],
                                Uprav = temp[7],
                                Vedom = temp[8],
                                Napr = Convert.ToDouble(temp[9]),
                                Year_create = Convert.ToInt32(temp[10]),
                                Year_start = Convert.ToInt32(temp[11]),
                                Cicle = Convert.ToInt32(temp[12]),
                                Last_year_vosst = Convert.ToInt32(temp[13]),
                                Ispol_type = temp[14]
                            };

                            db.Devices.Add(device);
                            db.Entry(device).State = EntityState.Added;
                            db.SaveChanges();

                            //добавление информации по годам обслуживания
                            int indexYear = 0;
                            string tmp;
                            for (int j = 23; j <= 28; j++)
                            {
                                range = (Excel.Range)sheet.Cells[i, j];
                                if (range != null)
                                {
                                    if (range.Value2 != null)
                                    {
                                        tmp = range.Value2;
                                    }
                                    else tmp = "";

                                    TO To = db.TOes.Where(c => c.Vid_TO == tmp).FirstOrDefault();
                                    if (To == null && !string.IsNullOrEmpty(tmp))
                                    {
                                        To = new TO()
                                        {
                                            Vid_TO = tmp
                                        };

                                        db.TOes.Add(To);
                                        db.Entry(To).State = EntityState.Added;
                                        db.SaveChanges();
                                    }
                                    if (To != null)
                                        switch (indexYear)
                                        {
                                            case 0:
                                                db.T2020.Add(new T2020
                                                {
                                                    Device_Id = device.ID,
                                                    Vid_TO_Id = To.ID
                                                });
                                                break;
                                            case 1:
                                                db.T2021.Add(new T2021
                                                {
                                                    Device_ID = device.ID,
                                                    Vid_TO_Id = To.ID
                                                });
                                                break;
                                            case 2:
                                                db.T2022.Add(new T2022
                                                {
                                                    Device_ID = device.ID,
                                                    Vid_TO_Id = To.ID
                                                });
                                                break;
                                            case 3:
                                                db.T2023.Add(new T2023
                                                {
                                                    Device_ID = device.ID,
                                                    Vid_TO_Id = To.ID
                                                });
                                                break;
                                            case 4:
                                                db.T2024.Add(new T2024
                                                {
                                                    Device_ID = device.ID,
                                                    Vid_TO_Id = To.ID
                                                });
                                                break;
                                            case 5:
                                                db.T2025.Add(new T2025
                                                {
                                                    Device_ID = device.ID,
                                                    Vid_TO_Id = To.ID
                                                });
                                                break;
                                        }
                                }
                                indexYear++;
                            }
                        }
                    ProcessChanged(i - 3);
                }

                // Закройте книгу без сохранения изменений.
                workbook.Close(false, Type.Missing, Type.Missing);
                WorkComplite(1);
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); WorkComplite(0); }
            // Закройте сервер Excel.
            excel_app.Quit();

        }

        public event Action<int> ProcessChanged; //установить текущее значение прогрессбара

        public event Action<int> SetMaxValProgressBar; //установить максимальное значение для прогрессбара

        public event Action<int> WorkComplite; //сигнал завершения работы
    }
}
