namespace StudentPractice.BusinessModel
{
    public enum enRegistrationType
    {
        None = 0,

        /// <summary>
        /// Με registration
        /// </summary>
        Membership = 1,

        /// <summary>
        /// Μέσω Shibboleth
        /// </summary>
        Shibboleth = 2,

        /// <summary>
        /// Με κλήση στην εφαρμογή της Ακαδημαϊκής Ταυτότητας
        /// </summary>
        AcademicIDNumber = 3,

        /// <summary>
        /// Με κλήση του API για αυτο-δήλωση
        /// </summary>
        Services = 4
    }
}
