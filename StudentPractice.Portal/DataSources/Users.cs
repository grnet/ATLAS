using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.Web.Security;
using StudentPractice.BusinessModel;
using Imis.Domain;

namespace StudentPractice.Portal.DataSources
{
    [DataObject(true)]
    public class Users
    {
        private int _RecordCount = 0;

        public int CountAdminUsersWithCriteria(Criteria<AdminUser> criteria)
        {
            return _RecordCount;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<AdminUser> FindAdminUsersWithCriteria(Criteria<AdminUser> criteria, int startRowIndex, int maximumRows, string sortExpression)
        {
            int recordCount;

            criteria.StartRowIndex = startRowIndex;
            criteria.MaximumRows = maximumRows;
            criteria.Sort.Expression = sortExpression;

            using (IUnitOfWork uow = UnitOfWorkFactory.Create())
            {
                var adminUsers = new AdminUserRepository(uow).FindWithCriteria(criteria, out recordCount);
                _RecordCount = recordCount;
                return adminUsers;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public IList<MembershipUser> FindUsersInRoles(string username, string[] roleNames)
        {
            IList<MembershipUser> result = new List<MembershipUser>();

            foreach (string roleName in roleNames)
            {
                IList<MembershipUser> users = FindUsersInRole(username, roleName);

                foreach (MembershipUser user in users)
                {
                    result.Add(user);
                }
            }

            return result;
        }

        #region Helper Methods

        private IList<MembershipUser> FindUsersInRole(string username, string roleName)
        {
            string[] users = Roles.FindUsersInRole(roleName, "%" + username + "%");

            List<MembershipUser> result = new List<MembershipUser>(users.Length);

            foreach (string u in users)
            {
                result.Add(Membership.GetUser(u));
            }

            return result;
        }

        #endregion
    }
}