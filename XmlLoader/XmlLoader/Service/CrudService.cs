using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Xml.Linq;
using XmlLoader.Models;

namespace XmlLoader.Service
{
    class CrudService
    {
        private Repository.OpenDataRepository _repository = new Repository.OpenDataRepository();

        public List<OpenData> loadData()
        {
            List<OpenData> result = new List<OpenData>();

            var xml = XElement.Load("data.xml");
            var nodes = xml.Descendants("row").ToList();

            for (var i = 0; i < nodes.Count; i++)
            {
                var node = nodes[i];
                OpenData item = new OpenData();

                item.地區 = node.Element("Col1").Value;
                item.縣市 = node.Element("Col2").Value;
                item.空氣品質指標 = node.Element("Col3").Value;
                item.空氣汙染指標物 = node.Element("Col4").Value;
                item.狀態 = node.Element("Col5").Value;

                result.Add(item);
            }
            return result;
        }
        public List<OpenData> FindDataFromDB(string country)
        {
            return _repository.SelectAll(country);
        }

        public void ImportToDb(List<OpenData> items)
        {

            items.ForEach(item =>
            {
                _repository.Create(item);
            });
        }

        public void Delete(string id)
        {
            _repository.Delete(id);
        }
    }
}
