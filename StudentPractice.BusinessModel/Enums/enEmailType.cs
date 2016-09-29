namespace StudentPractice.BusinessModel
{
    public enum enEmailType
    {
        /// <summary>
        /// Custom Message
        /// </summary>
        CustomMessage = 1,

        /// <summary>
        /// Απάντηση σε Online Ερώτημα
        /// </summary>
        IncidentReportAnswer = 2,

        /// <summary>
        /// Email πιστοποίησης
        /// </summary>
        EmailVerification = 3,

        /// <summary>
        /// Email με κωδικό πρόσβασης
        /// </summary>
        ForgotPassword = 4,

        /// <summary>
        /// Πιστοποίηση Φορέα  Υποδοχής
        /// </summary>
        ProviderVerification = 5,

        /// <summary>
        /// Πιστοποίηση Γραφείου Πρακτικής
        /// </summary>
        OfficeVerification = 6,

        /// <summary>
        /// Ενημέρωση Φοιτητή για αντιστοιχισμένη θέση Πρακτικής Άσκησης
        /// </summary>
        AssignedPositionStudentNotification = 7,

        /// <summary>
        /// Ενημέρωση Φορέα Υποδοχής για αντιστοιχισμένη θέση Πρακτικής Άσκησης
        /// </summary>
        AssignedPositionProviderNotification = 8,

        /// <summary>
        /// Ενημέρωση Γραφείου Πρακτικής για νέες δημοσιευμένες θέσεις Πρακτικής Άσκησης τις οποίες μπορεί να προδεσμεύσει
        /// </summary>
        NewlyPublishedPositions = 9,

        /// <summary>
        /// Ενημέρωση χρήστη για το ερώτημα που έστειλε
        /// </summary>
        IncidentReportSubmitConfirmation = 10,

        /// <summary>
        /// Ενημέρωση Φοιτητή για ολοκλήρωση θέσης Πρακτικής Άσκησης
        /// </summary>
        CompletedPositionStudentNotification = 11
    }
}
