using System;
using System.Web.UI;
using System.ComponentModel;
using StudentPractice.BusinessModel;

namespace StudentPractice.Portal.UserControls.GenericControls
{
    public partial class LanguageBar : UserControl
    {
        public bool DisableValidation
        {
            get { return !btnEnglish.CausesValidation; }
            set
            {
                btnEnglish.CausesValidation = !value;
                btnGreek.CausesValidation = !value;
            }
        }

        public string ValidationGroup
        {
            get { return btnGreek.ValidationGroup; }
            set 
            {
                btnEnglish.ValidationGroup = value;
                btnGreek.ValidationGroup = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LanguageBind();
        }

        private void LanguageBind()
        {
            btnEnglish.Visible = true;
            btnGreek.Visible = true;
            if (LanguageService.GetUserLanguage() == enLanguage.Greek)
                btnGreek.Visible = false;
            else if (LanguageService.GetUserLanguage() == enLanguage.English)
                btnEnglish.Visible = false;
        }

        protected void btnGreek_Click(object sender, EventArgs e)
        {
            LanguageService.SetUserLanguage(enLanguage.Greek);
            LanguageBind();
        }

        protected void btnEnglish_Click(object sender, EventArgs e)
        {
            LanguageService.SetUserLanguage(enLanguage.English);
            LanguageBind();
        }
    }
}