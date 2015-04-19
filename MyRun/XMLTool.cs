using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MyRun
{
    class XMLTool
    {
        String fileName;

        public XMLTool(String path)
        {
            this.fileName = path;
        }

        public Dictionary<String, String> getData()
        {
            Dictionary<String, String> result = new Dictionary<string,string>();

            XmlDocument doc = new XmlDocument();
            doc.Load(fileName);

            XmlNode xn = doc.SelectSingleNode("Map");
            XmlNodeList xnl = xn.ChildNodes;

            foreach(XmlNode xmlNode in xnl)
            {
                XmlNodeList list = xmlNode.ChildNodes;

                String key = list.Item(0).InnerText;
                String path = list.Item(1).InnerText;

                result.Add(key, path);
            }


            return result;
        }

        public void Add(String key, String path)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(this.fileName);

            XmlElement eleKey = doc.CreateElement("Key");
            XmlText keyText = doc.CreateTextNode(key);

            XmlElement elePath = doc.CreateElement("Path");
            XmlText pathText = doc.CreateTextNode(path);

            XmlNode newElem = doc.CreateNode("element", "Program", "");

            newElem.AppendChild(eleKey);
            newElem.LastChild.AppendChild(keyText);

            newElem.AppendChild(elePath);
            newElem.LastChild.AppendChild(pathText);

            XmlElement root = doc.DocumentElement;
            root.AppendChild(newElem);

            doc.Save(fileName);

        }

    }
}
