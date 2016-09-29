using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.IO;
using System.Reflection;

namespace StudentPractice.BusinessModel
{
    /// <summary>
    /// Η κλάση αυτή αντιπροσωπεύει ένα Document τα οποία και διαβάζουμε από ένα XML
    /// </summary>
    public class TransferDocument
    {
        public static List<TransferDocument> All
        {
            get
            {
                Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("StudentPractice.BusinessModel.Resources.TransferDocuments.xml");
                XDocument xdoc = XDocument.Load(stream);
                List<TransferDocument> documents = new List<TransferDocument>();
                foreach (XElement element in xdoc.Descendants("Document"))
                {
                    documents.Add(new TransferDocument() { ID = int.Parse(element.Attribute("ID").Value), IsRequired = bool.Parse(element.Attribute("isRequired").Value), RequiredByCategory = (int?)element.Attribute("requiredByCategory"), Description = element.Value });
                }
                return documents;
            }
        }
        public int ID { get; set; }
        public bool IsRequired { get; set; }
        public int? RequiredByCategory { get; set; }
        public string Description { get; set; }
    }
}
