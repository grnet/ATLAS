using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.Reflection;
using System.Xml;

namespace StudentPractice.BusinessModel
{
    public class DOY
    {
        private static XElement _xDoc = null;
        public static XElement DOYsXml
        {
            get
            {
                if (_xDoc == null)
                {
                    Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("StudentPractice.BusinessModel.Resources.DOYs.xml");
                    _xDoc = XDocument.Load(XmlReader.Create(stream)).Element("DOYs");
                }
                return _xDoc;
            }
        }
    }
}
