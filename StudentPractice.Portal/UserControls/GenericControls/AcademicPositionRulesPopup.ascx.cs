using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.Portal.Controls;

namespace StudentPractice.Portal.UserControls.GenericControls
{
    public partial class AcademicPositionRulesPopup : BaseScriptControl
    {
        #region [ Properties ]

        public bool Modal
        {
            get { return dxpcPopup.Modal; }
            set { dxpcPopup.Modal = value; }
        }

        public string HeaderText
        {
            get { return dxpcPopup.HeaderText; }
            set { dxpcPopup.HeaderText = value; }
        }

        public Unit Width
        {
            get { return dxpcPopup.Width; }
            set { dxpcPopup.Width = value; }
        }

        public Unit Height
        {
            get { return dxpcPopup.Height; }
            set { dxpcPopup.Height = value; }
        }

        public bool RequireTermsAcceptance { get; set; }

        public override string ClientControlPath { get { return string.Empty; } }

        protected override string ClientControlName
        {
            get { return "StudentPractice.Portal.UserControls.GenericControls.AcademicPositionRulesPopup"; }
        }

        #endregion

        #region [ IScriptControl Members ]

        public override IEnumerable<ScriptDescriptor> GetScriptDescriptors()
        {
            var scd = new ScriptControlDescriptor(ClientControlName, ClientID);
            scd.AddProperty("popup", dxpcPopup.ClientID);
            scd.AddProperty("requireTermsAcceptance", RequireTermsAcceptance);
            scd.AddElementProperty("acceptTermsArea", acceptTermsArea.ClientID);
            scd.AddElementProperty("cancelArea", cancelArea.ClientID);
            scd.AddElementProperty("btnSubmit", btnSubmit.ClientID);
            scd.AddElementProperty("btnCancel", btnCancel.ClientID);
            scd.AddElementProperty("rulesArea", rulesArea.ClientID);

            yield return scd;
        }

        public override IEnumerable<ScriptReference> GetScriptReferences()
        {
            yield return new ScriptReference("~/UserControls/GenericControls/AcademicPositionRulesPopup.js");
        }

        #endregion
    }
}