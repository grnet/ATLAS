using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;
using System.Threading;
using StudentPractice.Mails;
using StudentPractice.Utils;
using Imis.Domain;

namespace StudentPractice.Portal.Secure.Admin
{
    public partial class RefreshCache : System.Web.UI.Page
    {
        protected void btnRefreshCache_Click(object sender, EventArgs e)
        {
            CacheManager.Refresh();

            fm.Text = "Η Cache ανανεώθηκε επιτυχώς";
        }
    }
}