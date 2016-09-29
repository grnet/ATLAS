using StudentPractice.BusinessModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentPractice.Portal
{   
    public class RedirectFromPortal : IHttpHandler
    {
        public enum enRedirectTo
        {   
            Register = 1,            
            Login = 2,            
            ContactForm = 3
        }

        public void ProcessRequest(HttpContext context)
        {
            int redirectToInt;
            if (int.TryParse(context.Request.QueryString["id"], out redirectToInt) && redirectToInt > 0 && redirectToInt < 4)
            {
                int languageInt;
                if (int.TryParse(context.Request.QueryString["language"], out languageInt) && languageInt >= 0 && languageInt <= 1)
                {
                    enLanguage language = (enLanguage)languageInt;
                    
                    LanguageService.SetUserLanguageNoRedirect(language);

                    switch ((enRedirectTo)redirectToInt)
                    {
                        case enRedirectTo.Register:
                            context.Response.Redirect("~/Common/ProviderRegistration.aspx?c=2");
                            break;
                        case enRedirectTo.Login:
                            if (language == enLanguage.Greek)
                            {
                                context.Response.Redirect("~/Default.aspx");
                            }
                            else
                            {
                                context.Response.Redirect("~/DefaultEN.aspx");
                            }
                            break;
                        case enRedirectTo.ContactForm:
                            if (language == enLanguage.Greek)
                            {
                                context.Response.Redirect(string.Format("{0}/Contact.aspx", Config.StudentPracticePortalURL));
                            }
                            else
                            {
                                context.Response.Redirect(string.Format("{0}/ContactEn.aspx", Config.StudentPracticePortalURL));
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}