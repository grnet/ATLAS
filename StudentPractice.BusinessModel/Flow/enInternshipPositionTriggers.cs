namespace StudentPractice.BusinessModel.Flow
{
    public enum enInternshipPositionTriggers
    {
        /// <summary>
        /// Δημοσίευση θέσης
        /// </summary>
        Publish,

        /// <summary>
        /// Προδέσμευση θέσης
        /// </summary>
        PreAssign,

        /// <summary>
        /// Ανάθεση θέσης σε φοιτητή
        /// </summary>
        Assign,

        /// <summary>
        /// Έναρξη υλοποίησης πρακτικής από φοιτητή
        /// </summary>
        BeginImplementation,

        /// <summary>
        /// Ολοκλήρωση πρακτικής άσκησης
        /// </summary>
        CompleteImplementation,

        /// <summary>
        /// Απο-δημοσίευση θέσης
        /// </summary>
        UnPublish,

        /// <summary>
        /// Επαναφορά θέσης σε κατάσταση "Ελεύθερη"
        /// </summary>
        RollbackPreAssignment,

        /// <summary>
        /// Διαγραφή αντιστοίχισης σε φοιτητή
        /// </summary>
        DeleteAssignment,

        /// <summary>
        /// Ακύρωση Θέσης
        /// </summary>
        Cancel,

        /// <summary>
        /// Κατευθείαν αντιστοίχιση και έναρξη εκτέλεσης
        /// </summary>
        AssignAndBeginImplementation,

        /// <summary>
        /// Επαναφορά από κατάσταση 'Ολοκληρωμένη' σε κατάσταση 'Υπό διενέργεια'
        /// </summary>
        RollbackCompletion,

        /// <summary>
        /// Επαναφορά από κατάσταση 'Ακυρωμένη' σε κατάσταση 'Υπό διενέργεια'
        /// </summary>
        RollbackCancellation,

        /// <summary>
        /// Επαναφορά από κατάσταση 'Ακυρωμένη' σε κατάσταση 'Ελεύθερη'
        /// </summary>
        RollbackRevoke
    }
}