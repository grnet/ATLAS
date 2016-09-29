namespace StudentPractice.BusinessModel
{
    public enum enHandlerStatus
    {
        /// <summary>
        /// Δεν έχει ανατεθεί σε supervisor
        /// </summary>
        NotHandledBySupervisor = 0,

        /// <summary>
        /// Εκκρεμεί
        /// </summary>
        Pending = 1,

        /// <summary>
        /// Έχει κλείσει
        /// </summary>
        Closed = 2
    }
}
