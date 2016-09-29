using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StudentPractice.BusinessModel;
using System.ComponentModel;
using Imis.Domain;
using System.Data;

namespace StudentPractice.Portal.DataSources
{
    public class Students : BaseDataSource<Student>
    {
        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<Student> FindWithOfficePositionCount(Criteria<Student> criteria, int startRowIndex, int maximumRows, string sortExpression)
        {
            int recordCount;

            criteria.StartRowIndex = startRowIndex;
            criteria.MaximumRows = maximumRows;
            if (!string.IsNullOrEmpty(sortExpression))
            {
                criteria.Sort.Expression = sortExpression;
            }

            using (IUnitOfWork uow = UnitOfWorkFactory.Create())
            {
                var results = new StudentRepository(uow).FindWithOfficePositionCount(criteria, out recordCount);
                _RecordCount = recordCount;

                if (!criteria.UsePaging && criteria.MaximumRows > 0)
                {
                    return results.Skip(startRowIndex).Take(maximumRows).ToList();
                }

                return results;
            }
        }
    }
}
