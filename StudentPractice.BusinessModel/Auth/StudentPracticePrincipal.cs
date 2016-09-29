using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StudentPractice.BusinessModel
{
    public class StudentPracticeIdentity : GenericIdentity
    {
        public StudentPracticeIdentity(GenericIdentity identity)
            : base(identity) { }

        public StudentPracticeIdentity(string name)
            : base(name) { }

        public StudentPracticeIdentity(string name, string type)
            : base(name, type) { }

        public int ReporterID { get; set; }
        public string ContactName { get; set; }
    }

    public class StudentPracticePrincipal : GenericPrincipal
    {
        public StudentPracticePrincipal(IIdentity identity, string[] roles)
            : base(identity, roles) { }

        public StudentPracticeIdentity Identity { get; set; }
    }
}
