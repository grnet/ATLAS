using System.Collections.Generic;
using System.Data.Objects.DataClasses;
using Imis.Domain;
using System.Linq.Expressions;
using System;

namespace StudentPractice.BusinessModel
{
    public class Criteria<T> : Imis.Domain.EF.DomainCriteria<T> where T : EntityObject
    {
    }
}