using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StudentPractice.BusinessModel
{
    public enum enQuestionnaireType
    {
        StudentForOffice = 0,   //Per position
        StudentForProvider,     //Per position
        StudentForAtlas,
        OfficeForStudent,       //Per position
        OfficeForProvider,      //Single
        OfficeForAtlas,
        ProviderForStudent,     //Per position
        ProviderForOffice,      //Single
        ProviderForAtlas
    }
}
