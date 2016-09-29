using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StudentPractice.Portal.Secure.Reports
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void sdsStatistics_Selected(object sender, SqlDataSourceStatusEventArgs e)
        {
            e.Command.Transaction.Commit();
        }

        protected void sdsStatistics_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Connection.Open();
            e.Command.Transaction = e.Command.Connection.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
        }
    }
}