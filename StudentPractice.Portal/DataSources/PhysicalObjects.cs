using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using StudentPractice.BusinessModel;
using Imis.Domain;

namespace StudentPractice.Portal.DataSources
{
    public class PhysicalObjects : BaseDataSource<PhysicalObject>
    {
        [DataObjectMethod(DataObjectMethodType.Select)]
        public IEnumerable<PhysicalObject> GetAll()
        {
            return CacheManager.PhysicalObjects.GetItems();
        }
    }
}
