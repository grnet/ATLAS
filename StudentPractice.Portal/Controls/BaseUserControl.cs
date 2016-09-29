using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace StudentPractice.Portal.Controls
{
    public class BaseUserControl<TPage> : UserControl where TPage : Page
    {
        public new TPage Page { get { return (TPage)base.Page; } }
    }
}
