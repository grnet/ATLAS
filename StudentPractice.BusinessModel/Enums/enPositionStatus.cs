namespace StudentPractice.BusinessModel
{
    public enum enPositionStatus
    {
        /// <summary>
        /// Μη δημοσιευμένη
        /// </summary> 
        UnPublished = 0,

        /// <summary>
        /// Δημοσιευμένη - Ελεύθερη προς δέσμευση από ΓΠΑ
        /// </summary> 
        Available = 1,

        /// <summary>
        /// Προδεσμευμένη
        /// </summary> 
        PreAssigned = 2,

        /// <summary>
        /// Αντιστοιχισμένη
        /// </summary> 
        Assigned = 3,

        /// <summary>
        /// Υπό διενέργεια
        /// </summary> 
        UnderImplementation = 4,

        /// <summary>
        /// Ολοκληρωμένη
        /// </summary> 
        Completed = 5,

        /// <summary>
        /// Ακυρωμένη
        /// </summary> 
        Canceled = 6
    }
}
