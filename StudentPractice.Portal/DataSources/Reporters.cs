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
    public class Reporters : BaseDataSource<Reporter>
    {
        public int CountWithReporterCriteria(ReporterCriteria criteria)
        {
            return _RecordCount;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<Reporter> FindWithReporterCriteria(ReporterCriteria criteria, int startRowIndex, int maximumRows, string sortExpression)
        {
            int recordCount;

            criteria.StartRowIndex = startRowIndex;
            criteria.MaximumRows = maximumRows;
            criteria.Sort.Expression = sortExpression;

            using (IUnitOfWork uow = UnitOfWorkFactory.Create())
            {
                var reporters = new ReporterRepository(uow).FindWithCriteria(criteria, out recordCount);
                _RecordCount = recordCount;

                if (!criteria.UsePaging && criteria.MaximumRows > 0)
                {
                    return reporters.Skip(startRowIndex).Take(maximumRows).ToList();
                }

                return reporters;
            }
        }
    }
}
