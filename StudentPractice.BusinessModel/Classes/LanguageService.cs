using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Security;

namespace StudentPractice.BusinessModel
{
    public static class LanguageService
    {
        private const string LanguageCookieName = "__UserLanguage";

        private static bool IsForeign(int? countryID)
        {
            if (!countryID.HasValue)
                return false;

            return (countryID.Value != StudentPracticeConstants.GreeceCountryID && countryID.Value != StudentPracticeConstants.CyprusCountryID);
        }

        public static void InitUserLanguage()
        {
            if (HttpContext.Current != null && HttpContext.Current.Request != null && HttpContext.Current.Response != null)
            {
                var cookie = HttpContext.Current.Request.Cookies[LanguageCookieName];
                if (cookie != null)
                {
                    enLanguage lang = enLanguage.Greek;
                    Enum.TryParse(cookie.Value, out lang);
                    SetCurrentCulture(lang);
                    return;
                }

                if (Thread.CurrentPrincipal.Identity.IsAuthenticated)
                {
                    Reporter reporter = null;
                    using (var uow = UnitOfWorkFactory.Create())
                    {
                        reporter = new ReporterRepository(uow).FindByUsername(Thread.CurrentPrincipal.Identity.Name);
                        if (reporter != null && reporter is InternshipProvider)
                        {
                            var provider = (InternshipProvider)reporter;
                            if (!IsForeign(provider.CountryID) && provider.Language == enLanguage.English)
                            {
                                provider.Language = enLanguage.Greek;
                                uow.Commit();
                            }
                            else if (IsForeign(provider.CountryID) && provider.Language.HasValue && provider.Language.Value == enLanguage.Greek)
                            {
                                provider.Language = enLanguage.English;
                                uow.Commit();
                            }
                        }
                    }

                    if (reporter != null)
                    {
                        var lang = reporter.Language.HasValue ? reporter.Language.Value : enLanguage.Greek;

                        SetCookie(lang);
                        SetCurrentCulture(lang);
                    }
                    else
                    {
                        SetCookie(enLanguage.Greek);
                        SetCurrentCulture(enLanguage.Greek);
                    }
                }
                else
                {
                    SetCookie(enLanguage.Greek);
                    SetCurrentCulture(enLanguage.Greek);
                }
            }
        }

        public static enLanguage GetUserLanguage()
        {
            if (Thread.CurrentThread.CurrentCulture.Name == "en-US")
                return enLanguage.English;
            else
                return enLanguage.Greek;
        }

        public static void SetUserLanguage(enLanguage lang)
        {
            if (HttpContext.Current != null && HttpContext.Current.Request != null && HttpContext.Current.Response != null)
            {
                using (var uow = UnitOfWorkFactory.Create())
                {
                    var reporter = new ReporterRepository(uow).FindByUsername(Thread.CurrentPrincipal.Identity.Name);
                    if (reporter != null)
                    {
                        reporter.Language = lang;
                        uow.Commit();
                    }
                }
                SetCookie(lang);
                SetCurrentCulture(lang);
                HttpContext.Current.Response.Redirect(HttpContext.Current.Request.RawUrl);
            }
        }

        public static void SetUserLanguageNoRedirect(enLanguage lang)
        {
            if (HttpContext.Current != null && HttpContext.Current.Request != null && HttpContext.Current.Response != null)
            {
                using (var uow = UnitOfWorkFactory.Create())
                {
                    var reporter = new ReporterRepository(uow).FindByUsername(Thread.CurrentPrincipal.Identity.Name);
                    if (reporter != null)
                    {
                        reporter.Language = lang;
                        uow.Commit();
                    }
                }
                SetCookie(lang);
                SetCurrentCulture(lang);
            }
        }

        #region [ Helpers ]

        private static void SetCookie(enLanguage lang)
        {
            if (HttpContext.Current.Response.Cookies.AllKeys.Contains(LanguageCookieName))
                HttpContext.Current.Response.Cookies.Set(new HttpCookie(LanguageCookieName, lang.ToString()));
            else
                HttpContext.Current.Response.Cookies.Add(new HttpCookie(LanguageCookieName, lang.ToString()));
        }

        private static void SetCurrentCulture(enLanguage lang)
        {
            switch (lang)
            {
                case enLanguage.English:
                    Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                    Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
                    break;
                case enLanguage.Greek:
                default:
                    Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("el-GR");
                    Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("el-GR");
                    break;
            }
        }

        #endregion
    }
}