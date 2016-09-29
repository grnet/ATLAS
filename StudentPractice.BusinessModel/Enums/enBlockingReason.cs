namespace StudentPractice.BusinessModel
{
    public enum enBlockingReason
    {
        /// <summary>
        /// Κανένας
        /// </summary> 
        None = 0,

        /// <summary>
        /// Έγινε αποδέσμευση της θέσης μετά από μια ημέρα από την ημ/νία της προδέσμευσης
        /// </summary> 
        RolledbackPreAssignment = 1,

        /// <summary>
        /// Έληξε ο χρόνος που είχε το Γραφείο για να αντιστοιχίσει τη θέση σε φοιτητή
        /// </summary>
        TimeForAssignmentExpired = 2,

        /// <summary>
        /// Αφαιρέθηκε ο αντιστοιχισμένος φοιτητής ενώ είχαν περάσει 10 ημέρες από την ημ/νία της προδέσμευσης
        /// </summary>
        RolledbackAssignmentOutOfTime = 3,

        /// <summary>
        /// Μεταφορά ποινής από το υπάρχων Ιδρυματικό ή Τμηματικό ΓΠΑ
        /// </summary>
        BlockCascade = 4
    }
}