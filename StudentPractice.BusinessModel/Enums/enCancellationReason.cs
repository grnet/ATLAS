namespace StudentPractice.BusinessModel {
    public enum enCancellationReason {
        /// <summary>
        /// Κανένας
        /// </summary> 
        None = 0,

        /// <summary>
        /// Έγινε ακύρωση της θέσης από το ΓΠΑ (ο φοιτητής δεν ολοκλήρωσε την πρακτική του)
        /// </summary> 
        FromOffice = 1,

        /// <summary>
        /// Έγινε απόσυρση του group από το ΦΥΠΑ, οπότε ακυρώθηκαν αυτόματα οι μη προδεσμευμένες θέσεις
        /// </summary> 
        FromProvider = 2,

        /// <summary>
        /// Η θέση αποσύρθηκε από το ΦΥΠΑ και απελευθερώθηκε από το ΓΠΑ
        /// </summary>
        CanceledGroupCascade = 3,


        /// <summary>
        /// Η θέση αποσύρθηκε από το Helpdesk
        /// </summary>
        FromHelpdesk = 4
    }
}