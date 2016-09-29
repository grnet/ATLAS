using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;

namespace StudentPractice.Portal.Secure.InternshipProviders
{
    public partial class InternshipPositionExample : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e) {
            if (LanguageService.GetUserLanguage() == enLanguage.English)
                imgExample.ImageUrl = "~/_img/example_FYPAEN.png";
            else
                imgExample.ImageUrl = "~/_img/example_FYPA.jpg";

        }
    }
}