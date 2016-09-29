﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace StudentPractice.BusinessModel.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Labels {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Labels() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("StudentPractice.BusinessModel.Resources.Labels", typeof(Labels).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Μεταφορά ποινής από το υπάρχον Ιδρυματικό ή Τμηματικό ΓΠΑ.
        /// </summary>
        internal static string enBlockingReason_BlockCascade {
            get {
                return ResourceManager.GetString("enBlockingReason_BlockCascade", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Αφαιρέθηκε ο αντιστοιχισμένος φοιτητής ενώ είχαν περάσει 10 ημέρες από την ημ/νία της προδέσμευσης.
        /// </summary>
        internal static string enBlockingReason_RolledbackAssignmentOutOfTime {
            get {
                return ResourceManager.GetString("enBlockingReason_RolledbackAssignmentOutOfTime", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Έγινε αποδέσμευση της θέσης μετά από μια ημέρα από την ημ/νία της προδέσμευσης.
        /// </summary>
        internal static string enBlockingReason_RolledbackPreAssignment {
            get {
                return ResourceManager.GetString("enBlockingReason_RolledbackPreAssignment", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Έληξε ο χρόνος που είχε το Γραφείο για να αντιστοιχίσει τη θέση σε φοιτητή.
        /// </summary>
        internal static string enBlockingReason_TimeForAssignmentExpired {
            get {
                return ResourceManager.GetString("enBlockingReason_TimeForAssignmentExpired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Εισερχόμενη.
        /// </summary>
        internal static string enCallType_Incoming {
            get {
                return ResourceManager.GetString("enCallType_Incoming", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Εξερχόμενη.
        /// </summary>
        internal static string enCallType_Outgoing {
            get {
                return ResourceManager.GetString("enCallType_Outgoing", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Η θέση αποσύρθηκε από το ΦΥΠΑ και απελευθερώθηκε από το ΓΠΑ.
        /// </summary>
        internal static string enCancellationReason_CanceledGroupCascade {
            get {
                return ResourceManager.GetString("enCancellationReason_CanceledGroupCascade", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Η θέση αποσύρθηκε από το Helpdesk.
        /// </summary>
        internal static string enCancellationReason_FromHelpdesk {
            get {
                return ResourceManager.GetString("enCancellationReason_FromHelpdesk", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Έγινε ακύρωση της θέσης από το ΓΠΑ (ο φοιτητής δεν ολοκλήρωσε την πρακτική του).
        /// </summary>
        internal static string enCancellationReason_FromOffice {
            get {
                return ResourceManager.GetString("enCancellationReason_FromOffice", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Έγινε απόσυρση του group από το ΦΥΠΑ, οπότε ακυρώθηκαν αυτόματα οι μη προδεσμευμένες θέσεις.
        /// </summary>
        internal static string enCancellationReason_FromProvider {
            get {
                return ResourceManager.GetString("enCancellationReason_FromProvider", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Κανένας.
        /// </summary>
        internal static string enCancellationReason_None {
            get {
                return ResourceManager.GetString("enCancellationReason_None", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Έγινε απόσυρση του group από το ΦΥΠΑ, οπότε ακυρώθηκαν αυτόματα οι μη προδεσμευμένες θέσεις.
        /// </summary>
        internal static string enCancellationReason_ReprintingWithoutCharge {
            get {
                return ResourceManager.GetString("enCancellationReason_ReprintingWithoutCharge", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Πρύτανης.
        /// </summary>
        internal static string enCertifierType_AEIPresident {
            get {
                return ResourceManager.GetString("enCertifierType_AEIPresident", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Προϊστάμενος Τμήματος.
        /// </summary>
        internal static string enCertifierType_DepartmentChief {
            get {
                return ResourceManager.GetString("enCertifierType_DepartmentChief", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Πρόεδρος Τμήματος.
        /// </summary>
        internal static string enCertifierType_DepartmentPresident {
            get {
                return ResourceManager.GetString("enCertifierType_DepartmentPresident", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Πρόεδρος Ιδρύματος.
        /// </summary>
        internal static string enCertifierType_TEIPresident {
            get {
                return ResourceManager.GetString("enCertifierType_TEIPresident", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Email.
        /// </summary>
        internal static string enDispatchType_Email {
            get {
                return ResourceManager.GetString("enDispatchType_Email", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Fax.
        /// </summary>
        internal static string enDispatchType_Fax {
            get {
                return ResourceManager.GetString("enDispatchType_Fax", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SMS.
        /// </summary>
        internal static string enDispatchType_SMS {
            get {
                return ResourceManager.GetString("enDispatchType_SMS", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Ενημέρωση Φοιτητή για ολοκλήρωση θέσης Πρακτικής Άσκησης.
        /// </summary>
        internal static string enEmailType_CompletedPositionStudentNotification {
            get {
                return ResourceManager.GetString("enEmailType_CompletedPositionStudentNotification", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ΕΣΠΑ.
        /// </summary>
        internal static string enFundingType_ESPA {
            get {
                return ResourceManager.GetString("enFundingType_ESPA", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Ίδρυμα.
        /// </summary>
        internal static string enFundingType_Institution {
            get {
                return ResourceManager.GetString("enFundingType_Institution", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Φορέας Υποδοχής.
        /// </summary>
        internal static string enFundingType_InternshipProvider {
            get {
                return ResourceManager.GetString("enFundingType_InternshipProvider", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ΟΑΕΔ.
        /// </summary>
        internal static string enFundingType_OAED {
            get {
                return ResourceManager.GetString("enFundingType_OAED", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Άλλο.
        /// </summary>
        internal static string enFundingType_Other {
            get {
                return ResourceManager.GetString("enFundingType_Other", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Έχει κλείσει.
        /// </summary>
        internal static string enHandlerStatus_Closed {
            get {
                return ResourceManager.GetString("enHandlerStatus_Closed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Δεν χρειάζεται επικοινωνία.
        /// </summary>
        internal static string enHandlerStatus_NotHandledBySupervisor {
            get {
                return ResourceManager.GetString("enHandlerStatus_NotHandledBySupervisor", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Εκκρεμεί.
        /// </summary>
        internal static string enHandlerStatus_Pending {
            get {
                return ResourceManager.GetString("enHandlerStatus_Pending", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Γραφείο Αρωγής.
        /// </summary>
        internal static string enHandlerType_Helpdesk {
            get {
                return ResourceManager.GetString("enHandlerType_Helpdesk", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Ομάδα Παρακολούθησης.
        /// </summary>
        internal static string enHandlerType_Supervisor {
            get {
                return ResourceManager.GetString("enHandlerType_Supervisor", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Αστυνομική Ταυτότητα.
        /// </summary>
        internal static string enIdentificationType_ID {
            get {
                return ResourceManager.GetString("enIdentificationType_ID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Διαβατήριο.
        /// </summary>
        internal static string enIdentificationType_Passport {
            get {
                return ResourceManager.GetString("enIdentificationType_Passport", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Τμηματικό.
        /// </summary>
        internal static string enOfficeType_Departmental {
            get {
                return ResourceManager.GetString("enOfficeType_Departmental", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Ιδρυματικό.
        /// </summary>
        internal static string enOfficeType_Institutional {
            get {
                return ResourceManager.GetString("enOfficeType_Institutional", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Πολλαπλά Τμηματικό.
        /// </summary>
        internal static string enOfficeType_MultipleDepartmental {
            get {
                return ResourceManager.GetString("enOfficeType_MultipleDepartmental", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Με ελλειπή στοιχεία.
        /// </summary>
        internal static string enOfficeType_None {
            get {
                return ResourceManager.GetString("enOfficeType_None", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Από Γραφείο Πρακτικής.
        /// </summary>
        internal static string enPositionCreationType_FromOffice {
            get {
                return ResourceManager.GetString("enPositionCreationType_FromOffice", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Από Φορέα Υποδοχής.
        /// </summary>
        internal static string enPositionCreationType_FromProvider {
            get {
                return ResourceManager.GetString("enPositionCreationType_FromProvider", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Διεγραμμένη.
        /// </summary>
        internal static string enPositionGroupStatus_Deleted {
            get {
                return ResourceManager.GetString("enPositionGroupStatus_Deleted", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Δημοσιευμένη.
        /// </summary>
        internal static string enPositionGroupStatus_Published {
            get {
                return ResourceManager.GetString("enPositionGroupStatus_Published", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Αποσυρμένη.
        /// </summary>
        internal static string enPositionGroupStatus_Revoked {
            get {
                return ResourceManager.GetString("enPositionGroupStatus_Revoked", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Μη Δημοσιευμένη.
        /// </summary>
        internal static string enPositionGroupStatus_UnPublished {
            get {
                return ResourceManager.GetString("enPositionGroupStatus_UnPublished", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Αντιστοιχισμένη.
        /// </summary>
        internal static string enPositionStatus_Assigned {
            get {
                return ResourceManager.GetString("enPositionStatus_Assigned", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Ελεύθερη.
        /// </summary>
        internal static string enPositionStatus_Available {
            get {
                return ResourceManager.GetString("enPositionStatus_Available", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Ακυρωμένη.
        /// </summary>
        internal static string enPositionStatus_Canceled {
            get {
                return ResourceManager.GetString("enPositionStatus_Canceled", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Ολοκληρωμένη.
        /// </summary>
        internal static string enPositionStatus_Completed {
            get {
                return ResourceManager.GetString("enPositionStatus_Completed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Προδεσμευμένη.
        /// </summary>
        internal static string enPositionStatus_PreAssigned {
            get {
                return ResourceManager.GetString("enPositionStatus_PreAssigned", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Υπό διενέργεια.
        /// </summary>
        internal static string enPositionStatus_UnderImplementation {
            get {
                return ResourceManager.GetString("enPositionStatus_UnderImplementation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Μη Δημοσιευμένη.
        /// </summary>
        internal static string enPositionStatus_UnPublished {
            get {
                return ResourceManager.GetString("enPositionStatus_UnPublished", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Πλήρες ωράριο.
        /// </summary>
        internal static string enPositionType_FullTime {
            get {
                return ResourceManager.GetString("enPositionType_FullTime", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Μερικό ωράριο.
        /// </summary>
        internal static string enPositionType_PartTime {
            get {
                return ResourceManager.GetString("enPositionType_PartTime", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Μ.Κ.Ο..
        /// </summary>
        internal static string enProviderType_NGO {
            get {
                return ResourceManager.GetString("enProviderType_NGO", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Άλλο.
        /// </summary>
        internal static string enProviderType_Other {
            get {
                return ResourceManager.GetString("enProviderType_Other", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Ιδιωτικός Φορέας.
        /// </summary>
        internal static string enProviderType_PrivateCarrier {
            get {
                return ResourceManager.GetString("enProviderType_PrivateCarrier", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Δημόσιος Φορέας.
        /// </summary>
        internal static string enProviderType_PublicCarrier {
            get {
                return ResourceManager.GetString("enProviderType_PublicCarrier", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Δηλώθηκε από το Γραφείο Αρωγής.
        /// </summary>
        internal static string enReporterDeclarationType_FromHelpdesk {
            get {
                return ResourceManager.GetString("enReporterDeclarationType_FromHelpdesk", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Δηλώθηκε μόνος του (από Portal).
        /// </summary>
        internal static string enReporterDeclarationType_FromPortal {
            get {
                return ResourceManager.GetString("enReporterDeclarationType_FromPortal", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Εγγεγραμμένος.
        /// </summary>
        internal static string enReporterDeclarationType_FromRegistration {
            get {
                return ResourceManager.GetString("enReporterDeclarationType_FromRegistration", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Διδακτικό Προσωπικό / Επόπτης.
        /// </summary>
        internal static string enReporterType_FacultyMember {
            get {
                return ResourceManager.GetString("enReporterType_FacultyMember", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Γραφείο Πρακτικής.
        /// </summary>
        internal static string enReporterType_InternshipOffice {
            get {
                return ResourceManager.GetString("enReporterType_InternshipOffice", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Φορέας Υποδοχής.
        /// </summary>
        internal static string enReporterType_InternshipProvider {
            get {
                return ResourceManager.GetString("enReporterType_InternshipProvider", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Άλλο.
        /// </summary>
        internal static string enReporterType_Other {
            get {
                return ResourceManager.GetString("enReporterType_Other", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Φοιτητής.
        /// </summary>
        internal static string enReporterType_Student {
            get {
                return ResourceManager.GetString("enReporterType_Student", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Έχει απαντηθεί.
        /// </summary>
        internal static string enReportStatus_Answered {
            get {
                return ResourceManager.GetString("enReportStatus_Answered", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;img src=&apos;/_img/iconUserComment.png&apos; title=&apos;Έχει απαντηθεί&apos; align=&apos;absmiddle&apos; hspace=&apos;5&apos;/&gt;.
        /// </summary>
        internal static string enReportStatus_Answered_Icon {
            get {
                return ResourceManager.GetString("enReportStatus_Answered_Icon", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Έχει κλείσει.
        /// </summary>
        internal static string enReportStatus_Closed {
            get {
                return ResourceManager.GetString("enReportStatus_Closed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;img src=&apos;/_img/iconAcceptAll.png&apos; title=&apos;Έχει κλείσει&apos; align=&apos;absmiddle&apos; hspace=&apos;5&apos;/&gt;.
        /// </summary>
        internal static string enReportStatus_Closed_Icon {
            get {
                return ResourceManager.GetString("enReportStatus_Closed_Icon", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Εκκρεμεί.
        /// </summary>
        internal static string enReportStatus_Pending {
            get {
                return ResourceManager.GetString("enReportStatus_Pending", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;img src=&apos;/_img/iconPending.png&apos; title=&apos;Εκκρεμεί&apos; align=&apos;absmiddle&apos; hspace=&apos;5&apos;/&gt;.
        /// </summary>
        internal static string enReportStatus_Pending_Icon {
            get {
                return ResourceManager.GetString("enReportStatus_Pending_Icon", resourceCulture);
            }
        }
    }
}
