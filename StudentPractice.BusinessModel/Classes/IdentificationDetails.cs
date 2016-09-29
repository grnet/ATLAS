using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StudentPractice.BusinessModel
{
    public class IdentificationDetails
    {
        public string IdNumber { get; set; }

        public enIdentificationType IdType { get; set; }

        public DateTime? IdIssueDate { get; set; }

        public string IdIssuer { get; set; }

        public IdentificationDetails(string idNumber, enIdentificationType idType, DateTime idIssueDate, string idIssuer)
        {
            IdNumber = idNumber;
            IdType = idType;
            IdIssueDate = IdIssueDate;
            IdIssuer = idIssuer;
        }

        public IdentificationDetails()
        {

        }
    }
}
