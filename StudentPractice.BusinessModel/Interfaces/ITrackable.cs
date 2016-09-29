using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StudentPractice.BusinessModel
{
    internal interface ITrackable
    {
        string ValueXML { get; set; }

        bool AllowTrackingChanges { get; set; }

    }
}
