using System.Collections.Generic;
using System.Linq;

namespace StudentPractice.BusinessModel
{
    public class AcademicRepository : BaseRepository<Academic>
    {
        #region [ Base .ctors ]

        public AcademicRepository() : base() { }

        public AcademicRepository(Imis.Domain.IUnitOfWork uow) : base(uow) { }

        #endregion

        public List<Academic> FindByInstitutionID(int institutionID)
        {
            return BaseQuery.Where(x => x.InstitutionID == institutionID).ToList();
        }

        public bool AnyHavePositionRules()
        {
            return BaseQuery.Any(x => !string.IsNullOrEmpty(x.PositionRules));
        }

    }
}