using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Windows;

namespace ARM_RZA_v._1._0
{
    class SQL_Base
    {
        private SQLiteConnection connection;

        public void WriteRelayDevices(object obj)
        {
            try
            {
                if (obj is List<RelayDevice_Class> relayDevices)
                {
                    connection = new SQLiteConnection("Data Source=armbase.db; version=3");
                    connection.Open();

                    foreach (RelayDevice_Class relayDevice in relayDevices)
                    {
                        SQLiteCommand command = connection.CreateCommand();

                        if (string.IsNullOrEmpty(relayDevice.PS_power)) relayDevice.PS_power = "пс";

                        if (string.IsNullOrEmpty(relayDevice.PS_name)) relayDevice.PS_name = "нет данных";

                        command.CommandText = "insert into Relays (RelayType, Purpose, PS_power, PS_name, Prisoed, ProtocolDate) values('" + relayDevice.RelayType + "','" + relayDevice.Purpose + "','" + relayDevice.PS_power + "','" + relayDevice.PS_name + "','','" + relayDevice.Date + "');";

                        command.ExecuteNonQuery();

                        GetRelayCollectionFromDataBase();
                    }
                    if (connection.State == System.Data.ConnectionState.Open)
                        connection.Close();
                }
            }
            catch
            {
                if (connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
            }
        }

        private void GetRelayCollectionFromDataBase()
        {
            if (connection.State == System.Data.ConnectionState.Closed) connection.Open();

            try
            {
                SQLiteCommand command = connection.CreateCommand();

                command.CommandText = "select RelayType, count(RelayType) from Relays group by RelayType";

                SQLiteDataReader sQLiteDataReader = command.ExecuteReader();

                ObservableCollection<CountRalayTypeItems> countRalaysCollection = new ObservableCollection<CountRalayTypeItems>();

                if (sQLiteDataReader.HasRows)
                {
                    while (sQLiteDataReader.Read())
                    {
                        if (sQLiteDataReader.GetValues().Count == 2)
                        {
                            var relayType = sQLiteDataReader.GetValue(0) as string;
                            var countRelayOfType = Convert.ToString(sQLiteDataReader.GetValue(1) as Int64?);

                            countRalaysCollection.Add(new CountRalayTypeItems
                            {
                                RelayType = relayType,
                                CountRelayOfType = countRelayOfType
                            });
                        }
                    }
                }

                if (countRalaysCollection.Count > 0)
                    SendRelayCollection(countRalaysCollection);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка №4.\n" + ex.ToString());
            }
        }

        public event Action<ObservableCollection<CountRalayTypeItems>> SendRelayCollection;
    }
}
