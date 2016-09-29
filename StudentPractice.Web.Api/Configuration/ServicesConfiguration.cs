using System;
using System.Configuration;
using System.Globalization;

namespace StudentPractice.Web.Api
{
    public class ServicesConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("ticketExpiration", IsRequired = false, DefaultValue = 2)]
        public int TicketExpiration
        {
            get { return (int)this["ticketExpiration"]; }
            set { this["ticketExpiration"] = value; }
        }

        [ConfigurationProperty("maximumItemsReturned", DefaultValue = "200", IsRequired = false)]
        public int MaximumItemsReturned
        {
            get { return (int)this["maximumItemsReturned"]; }
            set { this["maximumItemsReturned"] = value; }
        }

        [ConfigurationProperty("defaultPhoneNumber", DefaultValue = "200", IsRequired = false)]
        public string DefaultPhoneNumber
        {
            get { return (string)this["defaultPhoneNumber"]; }
            set { this["defaultPhoneNumber"] = value; }
        }

        [ConfigurationProperty("typeOfLogging", DefaultValue = "None", IsRequired = false)]
        public string TypeOfLogging
        {
            get { return (string)this["typeOfLogging"]; }
            set { this["typeOfLogging"] = value; }
        }
    }
}
