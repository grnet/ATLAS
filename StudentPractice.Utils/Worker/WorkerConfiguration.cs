using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace StudentPractice.Utils.Worker {
    public class WorkerConfiguration : ConfigurationSection {
        [ConfigurationProperty("processQueueOnInitialize", DefaultValue = false, IsRequired = false)]
        public bool ProcessQueueOnInitialize {
            get {
                return (bool)this["processQueueOnInitialize"];
            }
            set {
                this["processQueueOnInitialize"] = value;

            }
        }

        [ConfigurationProperty("processQueueInterval", DefaultValue = 60 * 6, IsRequired = false)]
        public int ProcessQueueInterval {
            get {
                return (int)this["processQueueInterval"];
            }
            set {
                this["processQueueInterval"] = value;

            }
        }

        [ConfigurationProperty("machineName", DefaultValue = "", IsRequired = false)]
        public string MachineName {
            get { return (string)this["machineName"]; }
            set { this["machineName"] = value; }
        }

        [ConfigurationProperty("workerItems", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(WorkerItemCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public WorkerItemCollection WorkerItems {
            get {
                WorkerItemCollection urlsCollection = (WorkerItemCollection)base["workerItems"];
                return urlsCollection;

            }
        }
    }
    public class WorkerItemCollection : ConfigurationElementCollection {
        public WorkerItemCollection() {
            WorkerItem workerItem = (WorkerItem)CreateNewElement();
            Add(workerItem);
        }

        public override ConfigurationElementCollectionType CollectionType {
            get {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        protected override ConfigurationElement CreateNewElement() {
            return new WorkerItem();
        }

        protected override Object GetElementKey(ConfigurationElement element) {
            return ((WorkerItem)element).Name;
        }

        public WorkerItem this[int index] {
            get {
                return (WorkerItem)BaseGet(index);
            }
            set {
                if (BaseGet(index) != null) {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        new public WorkerItem this[string Name] {
            get {
                return (WorkerItem)BaseGet(Name);
            }
        }

        public int IndexOf(WorkerItem url) {
            return BaseIndexOf(url);
        }

        public void Add(WorkerItem url) {
            BaseAdd(url);
        }
        protected override void BaseAdd(ConfigurationElement element) {
            BaseAdd(element, false);
        }

        public void Remove(WorkerItem url) {
            if (BaseIndexOf(url) >= 0)
                BaseRemove(url.Name);
        }

        public void RemoveAt(int index) {
            BaseRemoveAt(index);
        }

        public void Remove(string name) {
            BaseRemove(name);
        }

        public void Clear() {
            BaseClear();
        }

    }
    public class WorkerItem : ConfigurationSection {
        [ConfigurationProperty("name", DefaultValue = "", IsRequired = false)]
        public string Name {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        public DateTime RunAt {
            get {
                if (string.IsNullOrWhiteSpace(RunAtConfig))
                    return DateTime.Now;
                return DateTime.Parse(RunAtConfig);
            }
        }

        [ConfigurationProperty("runAt", IsRequired = false)]
        public string RunAtConfig {
            get {
                string[] val = ((string)this["runAt"]).Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                if (val.Length == 0)
                    return string.Empty;
                DateTime runAt;
                DateTime dtNow = DateTime.Now;
                int hour = int.Parse(val[0]);
                int minute = int.Parse(val[1]);
                //if (dtNow.Hour > hour || (dtNow.Hour == hour && dtNow.Minute > minute))
                //    runAt = new DateTime(dtNow.Year, dtNow.Month, dtNow.Day + 1, hour, minute, 0);
                //else
                runAt = new DateTime(dtNow.Year, dtNow.Month, dtNow.Day, hour, minute, 0);

                return runAt.ToString();
            }
            set {
                this["runAt"] = value;
            }
        }

    }
}