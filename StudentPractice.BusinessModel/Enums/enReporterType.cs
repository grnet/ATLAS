namespace StudentPractice.BusinessModel
{
    public enum enReporterType
    {
        /// <summary>
        /// Admin Χρήστης
        /// </summary>
        AdminUser = 1,

        /// <summary>
        /// Γραφείο Αρωγής
        /// </summary>
        Helpdesk = 2,

        /// <summary>
        /// Φορέας Υποδοχής Πρακτικής Άσκησης
        /// </summary>
        InternshipProvider = 3,

        /// <summary>
        /// Γραφείο Πρακτικής Άσκησης Ιδρύματος
        /// </summary>
        InternshipOffice = 4,

        /// <summary>
        /// Φοιτητής
        /// </summary>
        Student = 5,

        /// <summary>
        /// Διδακτικό Προσωπικό / Επόπτης
        /// </summary>
        FacultyMember = 6,

        /// <summary>
        /// Διδακτικό Προσωπικό / Επόπτης
        /// </summary>
        Other = 7
    }
}
