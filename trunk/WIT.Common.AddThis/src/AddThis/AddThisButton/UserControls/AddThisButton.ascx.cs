using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace WIT.Common.AddThisButton.UserControls
{
    public partial class AddThisButton : System.Web.UI.UserControl
    {
        #region Properties
        /// <summary>
        /// URL to share, by default if its not setted will use current location.
        /// </summary>
        public string URL
        {
            get
            {
                try
                {
                    return (string)ViewState[this.UniqueID + ".URL"];
                }
                catch
                {
                    return string.Empty;
                }
            }
            set
            {
                ViewState.Add(this.UniqueID + ".URL", value);
            }
        }

        /// <summary>
        /// Title of shared object
        /// </summary>
        public string Title
        {
            get
            {
                try
                {
                    return (string)ViewState[this.UniqueID + ".Title"];
                }
                catch
                {
                    return string.Empty;
                }
            }
            set
            {
                ViewState.Add(this.UniqueID + ".Title", value);
            }
        }

        /// <summary>
        /// For forcing the menu to use a particular language, specified via ISO code. 
        /// For example, setting this to "sv" would show the menu in Swedish. Note: Regardless of the 
        /// number of times it's specified, only one language is supported per page.
        /// </summary>
        public string SelectedLanguage
        {
            get
            {
                try
                {
                    return (string)ViewState[this.UniqueID + ".SelectedLanguage"];
                }
                catch
                {
                    return WellKnownKeys.ui_language_en;
                }
            }
            set
            {
                ViewState.Add(this.UniqueID + ".SelectedLanguage", value);
            }
        }

        /// <summary>
        /// The text of the button that shows the other sevices in the pop up window.
        /// </summary>
        public string TextMore
        {
            get
            {
                try
                {
                    return (string)ViewState[this.UniqueID + ".TextMore"];
                }
                catch
                {
                    return string.Empty;
                }
            }
            set
            {
                ViewState.Add(this.UniqueID + ".TextMore", value);
            }
        }

        /// <summary>
        /// The quick services items to show in the html.
        /// </summary>
        public string MenuServices
        {
            get
            {
                try
                {
                    return (string)ViewState[this.UniqueID + ".MenuServices"];
                }
                catch
                {
                    return string.Empty;
                }
            }
            set
            {
                ViewState.Add(this.UniqueID + ".MenuServices", value);
            }
        }

        /// <summary>
        /// Services to use in the expanded menu. Useful if very few services are desired -- specifying a long list via 
        /// services_exclude could be tiresome, and wouldn't catch a new service added later. 
        /// For example, setting this to 'bebo,misterwong,netvibes' would result in only those three services 
        /// appearing in the expanded menu. Always global.
        /// </summary>
        public string IncludeMoreServices
        {
            get
            {
                try
                {
                    return (string)ViewState[this.UniqueID + ".IncludeMoreServices"];
                }
                catch
                {
                    return string.Empty;
                }
            }
            set
            {
                ViewState.Add(this.UniqueID + ".IncludeMoreServices", value);
            }
        }

        /// <summary>
        /// Services to exclude from all menus. For example, setting this to 'facebook,myspace' would hide Facebook 
        /// and MySpace on all our menus. Always global.
        /// </summary>
        public string ExcludeMoreServices
        {
            get
            {
                try
                {
                    return (string)ViewState[this.UniqueID + ".ExcludeMoreServices"];
                }
                catch
                {
                    return string.Empty;
                }
            }
            set
            {
                ViewState.Add(this.UniqueID + ".ExcludeMoreServices", value);
            }
        }

        /// <summary>
        /// Separator template, can be html or simple text.
        /// </summary>
        public string Separator
        {
            get
            {
                try
                {
                    return (string)ViewState[this.UniqueID + ".Separator"];
                }
                catch
                {
                    return string.Empty;
                }
            }
            set
            {
                ViewState.Add(this.UniqueID + ".Separator", value);
            }
        }

        /// <summary>
        /// The version of the used AddThisButtonAPI.
        /// Default: http://s7.addthis.com/js/250/addthis_widget.js?pub=xa-4a60bb071de35881
        /// </summary>
        public string AddThisButtonAPI
        {
            get
            {
                string addThisButtonAPI = (string)ViewState[this.UniqueID + ".AddThisButtonAPI"];
                return (!string.IsNullOrEmpty(addThisButtonAPI)) ? addThisButtonAPI : "http://s7.addthis.com/js/250/addthis_widget.js?pub=xa-4a60bb071de35881";
            }
            set
            {
                ViewState.Add(this.UniqueID + ".AddThisButtonAPI", value);
            }
        }

        private bool CanBuildConfigurationAndSharingOptions { get; set; }
        #endregion

        #region Methods
        private void BuildConfigurationAndSharingOptions()
        {
            if (!this.Page.ClientScript.IsClientScriptBlockRegistered(this.Page.GetType(), "AddThisButtonConfigurationScript"))
            {
                StringBuilder script = new StringBuilder();
                script.Append("<script type='text/javascript' language='javascript'>");
                script.Append("var addthis_config=");
                script.Append("{");
                script.Append(!string.IsNullOrEmpty(IncludeMoreServices) ? "services_expanded: '" + IncludeMoreServices + "'," : string.Empty);
                script.Append(!string.IsNullOrEmpty(ExcludeMoreServices) ? "services_exclude: '" + ExcludeMoreServices + "'," : string.Empty);
                script.Append("};");
                script.Append("var addthis_share={};");
                script.Append("</script>");

                this.Page.ClientScript.RegisterClientScriptBlock(this.Page.GetType(), "AddThisButtonConfigurationScript", script.ToString());
            }
        }
        private void RegisterAddThisButtonAPI()
        {
            if (!this.Page.ClientScript.IsClientScriptBlockRegistered(this.Page.GetType(), "AddThisButtonAPI"))
            {
                StringBuilder script = new StringBuilder();
                script.Append("<script type='text/javascript' src='" + AddThisButtonAPI +"'></script>");
                this.Page.ClientScript.RegisterClientScriptBlock(this.Page.GetType(), "AddThisButtonAPI", script.ToString());
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BuildConfigurationAndSharingOptions();
            RegisterAddThisButtonAPI();
        }

        protected string BuildMenuServices()
        {
            if (!string.IsNullOrEmpty(MenuServices) && MenuServices.Contains(','))
            {
                string htmlResult = string.Empty;
                string[] menuServices = MenuServices.Split(',');
                foreach (string service in menuServices)
                {
                    htmlResult += "<a class='addthis_button_" + service + "'></a>";
                }
                return htmlResult;
            }
            return string.Empty;
        }
        #endregion
    }
}