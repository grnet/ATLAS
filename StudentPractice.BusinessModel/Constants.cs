using System;

namespace StudentPractice.BusinessModel
{
    public static class StudentPracticeConstants
    {
        public const int DEFAULT_SUBSYSTEM_ID = 1;
        public const int Default_MaxDaysForAssignment = 10;
        public const int Default_BlockingDays = 4;
        public const int GreeceCountryID = 117;
        public const string GreeceCountryName = "Ελλάδα";
        public const int CyprusCountryID = 109;
        public const string CyprusCountryName = "Κύπρος";

        public const int QuestionnaireFreeTextMaxLength = 500;
    }

    public static class RoleNames
    {
        public const string MasterProvider = "MasterProvider";
        public const string ProviderUser = "ProviderUser";
        public const string MasterOffice = "MasterOffice";
        public const string OfficeUser = "OfficeUser";
        public const string Student = "Student";

        public const string Helpdesk = "Helpdesk";
        public const string SuperHelpdesk = "SuperHelpdesk";
        public const string Supervisor = "Supervisor";

        public const string Reports = "Reports";
        public const string SuperReports = "SuperReports";

        public const string SystemAdministrator = "SystemAdministrator";
    }

    public static class AutomaticIncidentTypes
    {
        public const int UnknownUserGeneralInfo = 1;
        public const int StudentRegistration = 3;
    }

    public static class AutomaticIncidentReportMessages
    {
        public const string UserUnLocked = "Ξεκλείδωμα χρήστη με username {0} από το χρήστη {1}";
        public const string UserRejected = "Απόρριψη από το χρήστη {0}";
        public const string UserRestored = "Επαναφορά από το χρήστη {0}";
        public const string UserEmailChanged = "Αλλαγή e-mail χρήστη με username {0} από το χρήστη {1} από {2} σε {3}";
        public const string RegistrationAttemptWithExistingVerifiedAccount = "Προσπάθεια εγγραφής ενώ υπάρχει ήδη πιστοποιημένος χρήστης με Αριθμό Πανελληνίων {0}";
    }
}
