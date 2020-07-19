using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
namespace Model
{
    public class DataSources : List<DataSource>
    {
        public DataSources() { }
        public DataSources(XElement oRaw)
        {
            if (oRaw != null)
            {
                foreach (XElement oRow in oRaw.Elements())
                {
                    Add(new DataSource(oRow));
                }
            }
        }
        public XElement toXMLElement()
        {
            XElement oRoot = new XElement("DataSources");
            ForEach(x => oRoot.Add(x.toXMLNode()));
            return oRoot;
        }
    }
    public class DataSource
    {
        public Datasourcetype Type { set; get; }
        public string ServerUrl { set; get; }
        public string ProjectName { set; get; }
        public string User { set; get; }
        public bool ExcludeTasks { set; get; }
        public DataSource() { }
        public DataSource(XElement oRaw)
        {
            Datasourcetype oType;
            if (Enum.TryParse<Datasourcetype>(oRaw.Element("Type").Value, out oType))
                Type = oType;
            ServerUrl = oRaw.Element("ServerUrl").Value;
            ProjectName = oRaw.Element("ProjectName").Value;
            User = oRaw.Element("User").Value;
            if (bool.TryParse(oRaw.Element("ExcludeTasks")?.Value, out bool bResult))
                ExcludeTasks = bResult;
        }
        public XElement toXMLNode()
        {
            return new XElement("DataSource",
                        new XElement("Type",
                                        Type),
                        new XElement("ServerUrl",
                                        ServerUrl),
                        new XElement("ProjectName",
                                        ProjectName),
                        new XElement("User",
                                        User),
                        new XElement("ExcludeTasks",
                                        ExcludeTasks)
                                );

        }
    }
    public enum Datasourcetype
    {
        Outlook,
        TFS
    }
}
