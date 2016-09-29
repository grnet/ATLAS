using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StudentPractice.BusinessModel;
using Imis.Domain;
using Imis.Web.Controls;
using log4net;
using System.Web.Security;
using System.Web.UI;
using System.Threading;

namespace StudentPractice.Portal.Controls
{
    public class BasePortalPage<TMaster> : BaseEntityPortalPage where TMaster : MasterPage
    {
        public new TMaster Master { get { return (TMaster)base.Master; } }
    }

    public class BaseEntityPortalPage<T> : BaseEntityPortalPage
    {
        protected virtual void Fill() { }

        protected override void OnPreInit(EventArgs e)
        {
            Fill();
            base.OnPreInit(e);
        }

        public T Entity { get; set; }
    }

    public class BaseEntityPortalPage<T, TMaster> : BaseEntityPortalPage<T> where TMaster : MasterPage
    {
        public new TMaster Master { get { return (TMaster)base.Master; } }
    }

    public class BaseEntityPortalPage : DomainEntityPage
    {
        public new StudentPracticePrincipal User
        {
            get
            {
                if (Thread.CurrentPrincipal.Identity.IsAuthenticated)
                    return base.User as StudentPracticePrincipal;
                else
                    return new StudentPracticePrincipal(Thread.CurrentPrincipal.Identity, new string[] { }) { Identity = new StudentPracticeIdentity(Thread.CurrentPrincipal.Identity.Name, Thread.CurrentPrincipal.Identity.AuthenticationType) };
            }
        }

        public BaseEntityPortalPage()
        {

        }

        #region [ Overrides ]

        protected override Func<IUnitOfWork> GetUowFactoryMethod()
        {
            return () => { return UnitOfWorkFactory.Create(); };
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            //Notify
            if (!string.IsNullOrEmpty(_notifyMsg))
                ClientScript.RegisterStartupScript(GetType(), s_notifyKey + "_redirectNotification", string.Format("Imis.Lib.notify('{0}')", _notifyMsg), true);
        }

        #endregion

        #region [ Notify ]

        private string _notifyMsg = string.Empty;
        private const string s_notifyKey = "_imis_notifyMsg";

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            if (Session[s_notifyKey] != null)
            {
                _notifyMsg = (string)Session[s_notifyKey];
                Session.Remove(s_notifyKey);
            }
        }

        protected void Notify(string msg)
        {
            ClientScript.RegisterStartupScript(GetType(), s_notifyKey + "_onDemandNotification", string.Format("Imis.Lib.notify('{0}');", msg), true);
        }

        protected void NotifyOnNextRequest(string notifyMsg)
        {
            Session[s_notifyKey] = notifyMsg;
        }

        protected void RedirectAndNotify(string redirectUrl, string notifyMsg)
        {
            Session[s_notifyKey] = notifyMsg;
            Response.Redirect(redirectUrl, true);
        }

        #endregion
    }

    //public class BaseEntityPortalPage : DomainEntityPage
    //{
    //    bool _PreventCaching = false;

    //    public bool PreventCaching
    //    {
    //        get { return _PreventCaching; }
    //        set { _PreventCaching = value; }
    //    }

    //    public new RolePrincipal User
    //    {
    //        get { return base.User as RolePrincipal; }
    //    }

    //    private ApplicationUser _appUser;
    //    public ApplicationUser AppUser
    //    {
    //        get
    //        {
    //            if (!User.Identity.IsAuthenticated)
    //                return null;

    //            if (_appUser == null)
    //                _appUser = new ApplicationUser(User.Identity.Name);

    //            return _appUser;
    //        }
    //        set { _appUser = value; }
    //    }

    //    public BaseEntityPortalPage()
    //    {

    //    }

    //    protected override void OnPreRender(EventArgs e)
    //    {
    //        if (PreventCaching)
    //        {
    //            Response.Cache.SetCacheability(HttpCacheability.NoCache);
    //            Response.Cache.SetNoStore();
    //            Response.AddHeader("Pragma", "no-cache");
    //            Response.Expires = -1;
    //        }

    //        base.OnPreRender(e);
    //    }

    //    protected override Func<IUnitOfWork> GetUowFactoryMethod()
    //    {
    //        return () => { return UnitOfWorkFactory.Create(); };
    //    }
    //}
}