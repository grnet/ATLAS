using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using Imis.Domain;
using System.Data;
using System.Threading;

namespace StudentPractice.BusinessModel
{
    public static class BusinessHelper
    {
        public static string GenerateVerificationCode()
        {
            return RandomNumber(10000000, 99999999);
        }

        private static int GenerateRandomNumber()
        {
            var byt = new byte[4];
            var rngCrypto = new RNGCryptoServiceProvider();

            rngCrypto.GetBytes(byt);
            int result = BitConverter.ToInt32(byt, 0);

            return new Random(result).Next(0, 9);
        }

        private static string RandomNumber(int min, int max)
        {
            var random = new Random();

            return random.Next(min, max).ToString();
        }

        /// <summary>
        /// CheckAFM: Ελέγχει αν ένα ΑΦΜ είναι σωστό
        /// </summary>
        /// <param name="cAfm">Το ΑΦΜ που θα ελέγξουμε</param>
        /// <returns>true = ΑΦΜ σωστό, false = ΑΦΜ Λάθος</returns>
        public static bool CheckAFM(string cAfm)
        {
            // Ελεγχος αν είναι όλο μηδενικά
            if (cAfm.Trim('0') == string.Empty)
                return false;

            int nExp = 1;

            // Ελεγχος αν περιλαμβάνει μόνο γράμματα
            long nAfm;
            if (!long.TryParse(cAfm, out nAfm))
                return false;

            // Ελεγχος μήκους ΑΦΜ
            cAfm = cAfm.Trim();
            int nL = cAfm.Length;
            if (nL != 9) return false;

            //Υπολογισμός αν το ΑΦΜ είναι σωστό
            int nSum = 0;
            int xDigit = 0;
            int nT = 0;

            for (int i = nL - 2; i >= 0; i--)
            {
                xDigit = int.Parse(cAfm.Substring(i, 1));
                nT = xDigit * (int)(Math.Pow(2, nExp));
                nSum += nT;
                nExp++;
            }

            xDigit = int.Parse(cAfm.Substring(nL - 1, 1));
            nT = nSum / 11;

            int k = nT * 11;
            k = nSum - k;

            if (k == 10)
                k = 0;

            if (xDigit != k)
                return false;

            return true;
        }

        public static string NameTrim(string name, int maxLength)
        {
            if (name.Length > maxLength)
            {
                return string.Format("{0}.", name.SubstringByLength(maxLength - 1));
            }

            return name;
        }

        public static InternshipPositionGroup ClonePositionGroup(InternshipPositionGroup group)
        {
            InternshipPositionGroup newGroup = new InternshipPositionGroup();

            newGroup.ProviderID = group.ProviderID;
            newGroup.IsVisibleToAllAcademics = group.IsVisibleToAllAcademics;
            newGroup.Title = group.Title;
            newGroup.Description = group.Description;
            newGroup.Duration = group.Duration;
            newGroup.CityID = group.CityID;
            newGroup.PrefectureID = group.PrefectureID;
            newGroup.CountryID = group.CountryID;
            newGroup.NoTimeLimit = group.NoTimeLimit;
            newGroup.StartDate = group.StartDate;
            newGroup.EndDate = group.EndDate;
            newGroup.PositionType = group.PositionType;
            newGroup.Supervisor = group.Supervisor;
            newGroup.SupervisorEmail = group.SupervisorEmail;
            newGroup.ContactPhone = group.ContactPhone;
            newGroup.TotalPositions = 1;
            newGroup.AvailablePositions = 0;
            newGroup.PreAssignedPositions = 0;
            newGroup.PositionGroupStatus = enPositionGroupStatus.UnPublished;
            newGroup.CityText = group.CityText;

            var physicalObjects = group.PhysicalObjects.ToList();

            foreach (var physicalObject in physicalObjects)
            {
                newGroup.PhysicalObjects.Add(physicalObject);
            }

            var academics = group.Academics.ToList();

            foreach (var academic in academics)
            {
                newGroup.Academics.Add(academic);
            }

            return newGroup;
        }

        public static enPositionTransferResult TransferPosition(InternshipPosition position, InternshipOffice newOffice)
        {
            var oldOffice = position.PreAssignedByMasterAccount;

            if (position.PositionStatus == enPositionStatus.UnPublished
                || position.PositionStatus == enPositionStatus.Available
                || (position.PositionStatus == enPositionStatus.Canceled && position.CancellationReason != enCancellationReason.FromOffice))
                return enPositionTransferResult.InvalidStatus;

            if (newOffice.VerificationStatus != enVerificationStatus.Verified)
                return enPositionTransferResult.NewOfficeIsNotVerified;

            if (newOffice.ID == oldOffice.ID)
                return enPositionTransferResult.NewOfficeIsTheSame;

            if (!newOffice.Academics.Any(x => x.ID == position.PreAssignedForAcademicID))
                return enPositionTransferResult.NewOfficeDoesNotServeAcademic;

            if ((position.PositionStatus == enPositionStatus.Assigned
                || position.PositionStatus == enPositionStatus.UnderImplementation
                || position.PositionStatus == enPositionStatus.Completed
                || (position.PositionStatus == enPositionStatus.Canceled && position.CancellationReason == enCancellationReason.FromOffice))
                && position.AssignedToStudent != null
                && !newOffice.Academics.Any(x => x.ID == position.AssignedToStudent.AcademicID))
                return enPositionTransferResult.NewOfficeDoesNotServeStudent;

            position.PreAssignedAt = DateTime.Now.Date;
            position.PreAssignedByOfficeID = newOffice.ID;
            position.PreAssignedByMasterAccountID = newOffice.MasterAccountID.HasValue ? newOffice.MasterAccountID.Value : newOffice.ID;
            position.DaysLeftForAssignment = StudentPracticeConstants.Default_MaxDaysForAssignment;

            InternshipPositionLog logEntry = new InternshipPositionLog();
            logEntry.InternshipPositionID = position.ID;
            logEntry.OldStatus = position.PositionStatus;
            logEntry.NewStatus = position.PositionStatus;
            logEntry.PreAssignedByOfficeID = newOffice.ID;
            logEntry.PreAssignedByMasterAccountID = newOffice.MasterAccountID.HasValue ? newOffice.MasterAccountID.Value : newOffice.ID;
            logEntry.PreAssignedForAcademicID = position.PreAssignedForAcademicID;
            logEntry.CreatedAt = DateTime.Now;
            logEntry.CreatedAtDateOnly = DateTime.Now.Date;
            logEntry.CreatedBy = Thread.CurrentPrincipal.Identity.Name;

            position.LogEntries.Add(logEntry);
            return enPositionTransferResult.Success;
        }

        #region [ Update Student Info ]

        public static void UpdateStudentInfoFromStudentCard(IUnitOfWork uow, int studentID)
        {
            var student = new StudentRepository(uow).Load(studentID);

            var details = StudentCardNumberService.GetStudentInfo(new AcademicIDCardRequest()
            {
                AcademicID = student.AcademicID.Value,
                StudentNumber = student.StudentNumber,
                ServiceCallerID = student.ID
            });

            if (details == null)
                throw new Exception(string.Format("Call to StudentCard returned with a failed status for studentID: {0}", studentID));
            else if (!details.Success)
                return;
            else
            {
                UpdateStudentAcademicInfo(student, details);
                UpdateStudentGeneralInfo(student, details);
            }
        }

        public static void UpdateAllStudentInfo(Student student, StudentDetailsFromAcademicID details)
        {
            if (!string.IsNullOrEmpty(details.GreekFirstName))
                student.GreekFirstName = details.GreekFirstName;
            if (!string.IsNullOrEmpty(details.GreekLastName))
                student.GreekLastName = details.GreekLastName;
            if (!string.IsNullOrEmpty(details.LatinFirstName))
                student.LatinFirstName = details.LatinFirstName;
            if (!string.IsNullOrEmpty(details.LatinLastName))
                student.LatinLastName = details.LatinLastName;
            if (details.IsNameLatin.HasValue)
                student.IsNameLatin = details.IsNameLatin;

            UpdateStudentAcademicInfo(student, details);
        }

        public static void UpdateStudentGeneralInfo(Student student, StudentDetailsFromAcademicID details)
        {
            if (string.IsNullOrEmpty(student.GreekFirstName) && !string.IsNullOrEmpty(details.GreekFirstName))
                student.GreekFirstName = details.GreekFirstName;
            if (string.IsNullOrEmpty(student.GreekLastName) && !string.IsNullOrEmpty(details.GreekLastName))
                student.GreekLastName = details.GreekLastName;
            if (string.IsNullOrEmpty(student.LatinFirstName) && !string.IsNullOrEmpty(details.LatinFirstName))
                student.LatinFirstName = details.LatinFirstName;
            if (string.IsNullOrEmpty(student.LatinLastName) && !string.IsNullOrEmpty(details.LatinLastName))
                student.LatinLastName = details.LatinLastName;
            if (!student.IsNameLatin.HasValue && details.IsNameLatin.HasValue)
                student.IsNameLatin = details.IsNameLatin;
        }

        public static void UpdateStudentAcademicInfo(Student student, StudentDetailsFromAcademicID details)
        {
            student.PreviousAcademicID = details.PreviousAcademicID;
            student.PreviousStudentNumber = details.PreviousStudentNumber;

            if (string.IsNullOrEmpty(details.AcademicIDNumber))
                return;

            if (string.IsNullOrEmpty(student.AcademicIDNumber) || !student.AcademicIDStatus.HasValue || !student.AcademicIDSubmissionDate.HasValue)
            {
                student.AcademicIDNumber = details.AcademicIDNumber;
                student.AcademicIDStatus = (enAcademicIDApplicationStatus)details.AcademicIDStatus;
                student.AcademicIDSubmissionDate = details.AcademicIDSubmissionDate;
            }
            else
            {
                if (student.AcademicIDSubmissionDate.Value < details.AcademicIDSubmissionDate.Value)
                {
                    student.AcademicIDNumber = details.AcademicIDNumber;
                    student.AcademicIDStatus = (enAcademicIDApplicationStatus)details.AcademicIDStatus;
                    student.AcademicIDSubmissionDate = details.AcademicIDSubmissionDate;
                }
            }
        }

        public static void UpdateInfo(this Student student, IUnitOfWork uow)
        {
            var response = StudentCardNumberService.GetStudentInfo(new AcademicIDCardRequest()
            {
                AcademicID = student.AcademicID.Value,
                StudentNumber = student.StudentNumber,
                ServiceCallerID = 1
            });

            if (response != null && response.Success)
            {
                BusinessHelper.UpdateStudentAcademicInfo(student, response);
                uow.Commit();
            }
        }

        #endregion
    }
}