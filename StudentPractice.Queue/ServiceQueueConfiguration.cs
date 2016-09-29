using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentPractice.Queue
{
    public class ServiceQueueConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("maxNoOfRetries", DefaultValue = 5, IsRequired = false)]
        public int MaxNoOfRetries
        {
            get
            {
                return (int)this["maxNoOfRetries"];
            }
            set
            {
                this["maxNoOfRetries"] = value;
            }
        }

        [ConfigurationProperty("processQueueInterval", DefaultValue = 0, IsRequired = false)]
        public int ProcessQueueInterval
        {
            get
            {
                return (int)this["processQueueInterval"];
            }
            set
            {
                this["processQueueInterval"] = value;

            }
        }

        [ConfigurationProperty("maxBatchSize", DefaultValue = 20, IsRequired = false)]
        public int MaxBatchSize
        {
            get
            {
                return (int)this["maxBatchSize"];
            }
            set
            {
                this["maxBatchSize"] = value;

            }
        }

        [ConfigurationProperty("processQueueOnInitialize", DefaultValue = false, IsRequired = false)]
        public bool ProcessQueueOnInitialize
        {
            get
            {
                return (bool)this["processQueueOnInitialize"];
            }
            set
            {
                this["processQueueOnInitialize"] = value;

            }
        }

        [ConfigurationProperty("machineName", DefaultValue = "", IsRequired = false)]
        public string MachineName
        {
            get { return (string)this["machineName"]; }
            set { this["machineName"] = value; }
        }
    }
}
