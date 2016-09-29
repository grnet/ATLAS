using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using StudentPractice.BusinessModel;
using Imis.Domain;

namespace StudentPractice.Portal.DataSources
{
    public class Academics : BaseDataSource<Academic>
    {
        [DataObjectMethod(DataObjectMethodType.Select)]
        public IEnumerable<Academic> GetAll()
        {
            return CacheManager.Academics.GetItems();
        }


        [DataObjectMethod(DataObjectMethodType.Select)]
        public IEnumerable<Academic> GetAllActive()
        {
            return CacheManager.Academics.GetItems().Where(x => x.IsActive).ToList();
        }
    }
}
