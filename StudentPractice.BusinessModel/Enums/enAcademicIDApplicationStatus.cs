using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StudentPractice.BusinessModel
{
    public enum enAcademicIDApplicationStatus
    {
        /// <summary>
        /// Δεν υπάρχει αίτηση
        /// </summary> 
        NoApplication = 0,

        /// <summary>
        /// Δεν έχει παραδοθεί στο φοιτητή
        /// </summary> 
        NotDeliveredToStudent = 1,

        /// <summary>
        /// Παραδόθηκε στο φοιτητή
        /// </summary> 
        DeliveredToStudent = 2,

        /// <summary>
        /// Ακυρώθηκε
        /// </summary> 
        Canceled = 3
    }
}
