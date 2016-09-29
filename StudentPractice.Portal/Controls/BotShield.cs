using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using BotDetect;
using BotDetect.Web;
using BotDetect.Web.UI;
using AjaxControlToolkit;
using StudentPractice.BusinessModel;

namespace StudentPractice.Portal.Controls
{
    [ToolboxData("<{0}:BotShield runat=server></{0}:BotShield>")]
    public class BotShield : WebControl, INamingContainer
    {
        #region [ Constants ]

        public const string LABEL_ID = "lblFormShield";
        public const string TEXT_ID = "txtFormShield";
        public const string SHIELD_ID = "formShield";
        public const string VALIDATOR_ID = "valFormShield";
        public const string VALIDATOR_EXTENDER_ID = "valFormShieldExtender";

        public const string SCRIPT_FUNCTION = "ChangeBotShieldInput";
        public const string SCRIPT_VALIDATION_FUNCTION = "ValidateBotShieldInput";
        public const string SCRIPT_KEY = "StudentPractice.Portal.Controls.ChangeBotShieldInput";

        #endregion

        private ITemplate contentTemplate;

        [TemplateContainer(typeof(BotShield))]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ITemplate ContentTemplate
        {
            get 
            { 
                return contentTemplate; 
            }
            set 
            { 
                contentTemplate = value; 
            }
        }

        protected virtual ITemplate CreateDefaultTemplate()
        {
            return new LanapTemplate();
        }

        protected override void CreateChildControls()
        {
            Controls.Clear();
            ITemplate tmpl = contentTemplate != null ? contentTemplate : CreateDefaultTemplate();
            tmpl.InstantiateIn(this);
        }

        protected override HtmlTextWriterTag TagKey
        {
            get
            {
                return HtmlTextWriterTag.Div;
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            Captcha captcha = FindControl(SHIELD_ID) as Captcha;

            if (captcha != null)
            {
                Array imageStyles = typeof(ImageStyle).GetEnumValues();
                captcha.ImageStyle = (ImageStyle)imageStyles.GetValue(new Random().Next(imageStyles.Length));
            }


            if (this.Page != null && !this.Page.ClientScript.IsClientScriptIncludeRegistered(this.GetType(), SCRIPT_KEY))
            {
                this.Page.ClientScript.RegisterClientScriptInclude(this.GetType(), SCRIPT_KEY, this.Page.ClientScript.GetWebResourceUrl(this.GetType(), "StudentPractice.Portal.Controls.BotShield.js"));

                if (captcha != null)
                    this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), SCRIPT_KEY + "1", string.Format("var BS_MAX_LENGTH = {0}; var DEFAULT_BS_ERROR_MESSAGE = '{1}';", captcha.CodeLength, Resources.Botshield.ErrorMessage), true);
            }

            captcha.UseSmallIcons = true;
            captcha.Locale = LanguageService.GetUserLanguage() == enLanguage.English 
                ? "en-US" 
                : "en-GB";

            if (Page.IsPostBack)
            {
                BaseValidator validator = FindControl(VALIDATOR_ID) as BaseValidator;
                if (!validator.IsValid)
                {
                    //ForceDraw();
                    ITextControl txt = FindControl(TEXT_ID) as ITextControl;
                    txt.Text = "";
                }
            }
        }

        public string ValidationGroup
        {
            get
            {
                return (FindControl(VALIDATOR_ID) as BaseValidator).ValidationGroup;
            }
            set { (FindControl(VALIDATOR_ID) as BaseValidator).ValidationGroup = value; }
        }
    }

    sealed class LanapTemplate : ITemplate
    {
        private Label label = new Label();
        private Table table = new Table();
        private Captcha captcha = new Captcha();
        private TextBox txtFormShield = new TextBox();
        private CustomValidator valFormShield = new CustomValidator();

        internal LanapTemplate()
        {
            label.Text = string.Format("<b>{0}</b>", Resources.Botshield.Text);
            label.ID = BotShield.LABEL_ID;
            label.AssociatedControlID = "txtFormShield";

            TableRow row;
            TableCell cell;

            table.Rows.Add(row = new TableRow());

            row.Cells.Add(cell = new TableCell());
            captcha.ID = BotShield.SHIELD_ID;
            cell.Controls.Add(captcha);

            row.Cells.Add(cell = new TableCell());
            txtFormShield.ID = BotShield.TEXT_ID;
            txtFormShield.Attributes["onkeyup"] = BotShield.SCRIPT_FUNCTION + "(this)";
            cell.Controls.Add(txtFormShield);

            valFormShield.ID = BotShield.VALIDATOR_ID;
            valFormShield.ErrorMessage = Resources.Botshield.ErrorMessage;
            valFormShield.ClientValidationFunction = BotShield.SCRIPT_VALIDATION_FUNCTION;
            valFormShield.ControlToValidate = txtFormShield.ClientID;
            valFormShield.SetFocusOnError = false;
            valFormShield.ValidateEmptyText = true;
            valFormShield.ServerValidate += delegate(object sender, ServerValidateEventArgs e)
            {
                Control ctl = ((Control)sender).Parent;
                e.IsValid = ((Captcha)ctl.FindControl(BotShield.SHIELD_ID)).Validate(
                    ((TextBox)ctl.FindControl(BotShield.TEXT_ID)).Text);
            };
            valFormShield.Display = ValidatorDisplay.Dynamic;
            valFormShield.Text = string.Format("<img src=\"/_img/error.gif\" class=\"errortip\" runat=\"server\" title=\"{0}\" />", valFormShield.ErrorMessage);
            cell.VerticalAlign = VerticalAlign.Bottom;
            cell.Controls.Add(valFormShield);
        }

        public void InstantiateIn(Control container)
        {
            container.Controls.Add(label);
            container.Controls.Add(table);
        }
    }
}
