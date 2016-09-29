using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HelpDesk.Portal.Common
{
    public partial class SchoolSelectPopup : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            int methodID;

            if (int.TryParse(Request.QueryString["t"], out methodID))
            {
                switch (methodID)
                {
                    case 1:
                        odsAcademics.SelectMethod = "GetAll";
                        break;
                    case 2:
                        odsAcademics.SelectMethod = "GetAcademicsWithAtLeastOneTransferedStudent";
                        break;
                    case 3:
                        odsAcademics.SelectMethod = "GetAcademicsWithAtLeastOneLeavingStudent";
                        break;
                    default:
                        odsAcademics.SelectMethod = "GetAll";
                        break;
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            gv.CustomUnboundColumnData += new DevExpress.Web.ASPxGridView.ASPxGridViewColumnDataEventHandler(gv_CustomUnboundColumnData);
        }

        void gv_CustomUnboundColumnData(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "CityName")
            {
                e.Value = StudentPractice.Portal.CacheManager.Cities.Get((int)e.GetListSourceFieldValue("CityID")).Name;
            }
            else if (e.Column.FieldName == "PrefectureName")
            {
                e.Value = StudentPractice.Portal.CacheManager.Prefectures.Get((int)e.GetListSourceFieldValue("PrefectureID")).Name;
            }
        }
    }
}
