using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imis.Domain.EF.Extensions;

namespace StudentPractice.BusinessModel
{
    public class SubmittedQuestionnaireRepository : BaseRepository<SubmittedQuestionnaire>
    {
        #region [ Base .ctors ]

        public SubmittedQuestionnaireRepository() : base() { }

        public SubmittedQuestionnaireRepository(Imis.Domain.IUnitOfWork uow) : base(uow) { }

        #endregion

        public bool Exists(int reporterID, enQuestionnaireType qType)
        {
            return BaseQuery.Any(x => x.ReporterID == reporterID
                && x.Questionnaire.QuestionnaireTypeInt == (int)qType);
        }

        public bool Exists(int reporterID, enQuestionnaireType qType, int entityID)
        {
            return BaseQuery.Any(x => x.ReporterID == reporterID
                && x.Questionnaire.QuestionnaireTypeInt == (int)qType
                && x.EntityID == entityID);
        }

        public bool Exists(int reporterID, enQuestionnaireType qType, int entityID, int positionID)
        {
            return BaseQuery.Any(x => x.ReporterID == reporterID
                && x.Questionnaire.QuestionnaireTypeInt == (int)qType
                && x.EntityID == entityID
                && x.PositionID == positionID);
        }

        public SubmittedQuestionnaire FindOne(int reporterID, enQuestionnaireType qType)
        {
            return BaseQuery.Include(x => x.QuestionnaireAnswers)
                .FirstOrDefault(x => x.ReporterID == reporterID
                    && x.Questionnaire.QuestionnaireTypeInt == (int)qType);
        }

        public SubmittedQuestionnaire FindOne(int reporterID, enQuestionnaireType qType, int entityID)
        {
            return BaseQuery.Include(x => x.QuestionnaireAnswers)
                .FirstOrDefault(x => x.ReporterID == reporterID
                    && x.Questionnaire.QuestionnaireTypeInt == (int)qType
                    && x.EntityID == entityID);
        }

        public SubmittedQuestionnaire FindOne(int reporterID, enQuestionnaireType qType, int entityID, int positionID)
        {
            return BaseQuery.Include(x => x.QuestionnaireAnswers)
                .FirstOrDefault(x => x.ReporterID == reporterID
                    && x.Questionnaire.QuestionnaireTypeInt == (int)qType
                    && x.EntityID == entityID
                    && x.PositionID == positionID);
        }

        public List<SubmittedQuestionnaire> FindByTypeAndReporterID(enQuestionnaireType type, int reporterID)
        {
            return BaseQuery
                .Where(x => x.Questionnaire.QuestionnaireTypeInt == (int)type && x.ReporterID == reporterID)
                .ToList();
        }

        public List<SubmittedQuestionnaire> FindByTypeReporterIDAndEntityIDs(enQuestionnaireType type, int reporterID, IEnumerable<int?> entityIDs)
        {
            return BaseQuery
                .Where(x => x.Questionnaire.QuestionnaireTypeInt == (int)type && x.ReporterID == reporterID)
                .Where(x => entityIDs.Contains(x.EntityID))
                .ToList();
        }

        public List<SubmittedQuestionnaire> FindByTypeReporterIDAndPositionIDs(enQuestionnaireType type, int reporterID, IEnumerable<int?> positionIDs)
        {
            return BaseQuery
                .Where(x => x.Questionnaire.QuestionnaireTypeInt == (int)type && x.ReporterID == reporterID)
                .Where(x => positionIDs.Contains(x.PositionID))
                .ToList();
        }
    }
}
