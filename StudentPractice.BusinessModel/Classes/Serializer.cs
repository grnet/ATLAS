using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace StudentPractice.BusinessModel
{
    public class Serializer<T>
    {
        private XmlSerializer xs;

        public Serializer()
        {
            xs = new XmlSerializer(typeof(T));
        }

        public string Serialize(T value)
        {
            if (value == null)
                return string.Empty;

            StringBuilder sb = new StringBuilder();
            xs.Serialize(XmlTextWriter.Create(sb, new XmlWriterSettings() { OmitXmlDeclaration = true }), value);

            return sb.ToString();
        }

        public T Deserialize(string xml)
        {
            if (string.IsNullOrEmpty(xml))
                return default(T);

            return (T)xs.Deserialize(new StringReader(xml));
        }

    }
}
