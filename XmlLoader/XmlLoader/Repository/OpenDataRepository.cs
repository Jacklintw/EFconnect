using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using XmlLoader.Models;

namespace XmlLoader.Repository
{
    class OpenDataRepository
    {
        public string ConnectionString{
            get
            {
                return @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + Directory.GetCurrentDirectory()+ @"\Database.mdf;Integrated Security=True";
            }
        }
        public List<OpenData> SelectAll(string country)
        {
            var result = new List<OpenData>();
            var connect = new System.Data.SqlClient.SqlConnection(ConnectionString);
            connect.Open();

            var command = new System.Data.SqlClient.SqlCommand("", connect);
            command.CommandText = string.Format(@"
                                  SELECT Id, 縣市,地區, 空氣品質指標, 空氣汙染指標物, 狀態
                                  FROM Opendata");
            if (!string.IsNullOrEmpty(country))
            {
               command.CommandText = $"{command.CommandText} WHERE 縣市 = N'{country}'";
            }

            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var item = new OpenData();
                item.Id = reader.GetInt32(0);
                item.縣市 = reader.GetString(1);
                item.空氣品質指標 = reader.GetString(2);
                item.空氣汙染指標物 = reader.GetString(3);
                item.狀態 = reader.GetString(4);
                result.Add(item);
            }

            connect.Close();
            return result;
        }

        public void Create(OpenData item)
        {
            var newitem = item;
            var connect = new System.Data.SqlClient.SqlConnection(ConnectionString);
            connect.Open();

            var command = new System.Data.SqlClient.SqlCommand("", connect);
            command.CommandText = string.Format(@"
                                  INSERT INTO Opendata(縣市,地區, 空氣品質指標, 空氣汙染指標物, 狀態) 
                                  VALUES (N'{0}', N'{1}', N'{2}', N'{3}', N'{4}')", 
                                  newitem.縣市,newitem.地區, newitem.空氣品質指標, newitem.空氣汙染指標物, newitem.狀態);
            command.ExecuteNonQuery();
            connect.Close();

        }

        public void Delete(string id)
        {
            var connect = new System.Data.SqlClient.SqlConnection(ConnectionString);
            connect.Open();

            var command = new System.Data.SqlClient.SqlCommand("", connect);
            command.CommandText = string.Format(@"DELETE FROM Opendata WHERE Id = N'{0}'", id);
            command.ExecuteNonQuery();
            connect.Close();
        }

    }
}
