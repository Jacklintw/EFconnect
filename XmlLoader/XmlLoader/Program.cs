using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using XmlLoader.Models;
using XmlLoader.Service;

namespace XmlLoader
{
    class Program
    {
        static void Main(string[] args)
        {
            CrudService importService = new CrudService();
            var datas = importService.loadData();
            //Console.Write(datas);
            Console.WriteLine("空氣品質指標(AQI)");
            Console.WriteLine("輸入0顯示完整資訊，輸入1顯示以空汙指標物分類，輸入2顯示以狀態分類");
            Console.Write("輸入3以輸出至LocalDB，輸入d刪除資料，輸入4以查詢資料:");
            var input = Console.ReadLine();
            //Console.Write(input);
            showData(datas, input);
            Console.Write("請按任意鍵結束...");
            Console.ReadKey();
        }
        private static string getValue(XElement node, string propertyName)
        {
            return node.Element(propertyName)?.Value?.Trim();
        }
        public static void showData(List<OpenData> nodes, string f)
        {
            CrudService _crudService = new CrudService();
            Console.WriteLine("共 " + nodes.Count + " 筆資料");
            if (f == "0") {                 
                nodes.ForEach(group =>
                {
                    var country = group.縣市;
                    var site = group.地區;
                    var AQI = group.空氣品質指標;
                    var pollutant = group.空氣汙染指標物;
                    var status = group.狀態;
                    if (pollutant == "")
                        pollutant = "無";
                    Console.WriteLine("縣市名：" + country + "\t地區：" + site + "\t空氣品質指標：" + AQI);
                    Console.WriteLine("空氣汙染指標物：" + pollutant);
                    Console.WriteLine("狀態：" + status);
                });
            }
            else if(f == "1"){ 
                nodes.GroupBy(node => node.空氣汙染指標物).ToList()
                    .ForEach(group =>
                    {
                        var key = group.Key;
                        if (key == "")
                            key = "無";
                        var groupDatas = group.ToList();
                        var msg = $"空氣汙染指標物:{key},共有{groupDatas.Count()}筆資料";
                        Console.WriteLine(msg);
                    });
            }
            else if(f == "2") {
                nodes.GroupBy(node => node.狀態).ToList()
                    .ForEach(group =>
                    {
                        var key = group.Key;
                        var groupDatas = group.ToList();
                        var msg = $"狀態:{key},共有{groupDatas.Count()}筆資料";
                        Console.WriteLine(msg);
                    });
            }
            else if(f == "3")
            {
                _crudService.ImportToDb(nodes);
                Console.WriteLine("資料已新增至LocalDB");
            }
            else if(f == "4")
            {
                Console.Write("請輸入欲查詢之縣市:");
                var in_country = Console.ReadLine();
                var result = _crudService.FindDataFromDB(in_country);
                Console.WriteLine("共 " + result.Count + " 筆資料");
                result.ForEach(group =>
                {
                    var country = group.縣市;
                    var AQI = group.空氣品質指標;
                    var pollutant = group.空氣汙染指標物;
                    var status = group.狀態;
                    if (pollutant == "")
                        pollutant = "無";
                    Console.WriteLine("縣市名：" + country + "\t空氣品質指標：" + AQI);
                    Console.WriteLine("空氣汙染指標物：" + pollutant);
                    Console.WriteLine("狀態：" + status);
                });
            }
            else if(f == "d")
            {
                Console.Write("請輸入欲刪除資料之ID:");
                var id = Console.ReadLine();
                _crudService.Delete(id);
                Console.WriteLine("資料已刪除!");
            }
            else
            {
                Console.WriteLine("測試用字串");

            }
        }
    }
}
