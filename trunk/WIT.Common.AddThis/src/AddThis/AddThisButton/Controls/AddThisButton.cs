using System.Linq;
using System.Text;
using System.Web.UI;

namespace WIT.Common.AddThis.Controls
{
    public partial class AddThisButton : System.Web.UI.Control
    {
        #region Properties

        public string UserName
        {
            get
            {
                if (ViewState[this.UniqueID + ".UserName"] == null)
                {
                    return string.Empty;
                }
                return (string)ViewState[this.UniqueID + ".UserName"];
            }
            set
            {
                ViewState.Add(this.UniqueID + ".UserName", value);
            }
        }

        /// <summary>
        /// URL to share, by default if its not setted will use current location.
        /// </summary>
        public string URL
        {
            get
            {
                if (ViewState[this.UniqueID + ".URL"] == null)
                {
                    return string.Empty;
                }
                return (string)ViewState[this.UniqueID + ".URL"];
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
                if (ViewState[this.UniqueID + ".Title"] == null)
                {
                    return string.Empty;
                }
                return (string)ViewState[this.UniqueID + ".Title"];
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
                if (ViewState[this.UniqueID + ".SelectedLanguage"] == null)
                {
                    return WellKnownKeys.ui_language_en;
                }
                return (string)ViewState[this.UniqueID + ".SelectedLanguage"];
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
                if (ViewState[this.UniqueID + ".TextMore"] == null)
                {
                    return string.Empty;
                }
                return (string)ViewState[this.UniqueID + ".TextMore"];
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
                if (ViewState[this.UniqueID + ".MenuServices"] == null)
                {
                    return string.Empty;
                }
                return (string)ViewState[this.UniqueID + ".MenuServices"];
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
                if (ViewState[this.UniqueID + ".IncludeMoreServices"] == null)
                {
                    return string.Empty;
                }
                return (string)ViewState[this.UniqueID + ".IncludeMoreServices"];
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
                if (ViewState[this.UniqueID + ".ExcludeMoreServices"] == null)
                {
                    return string.Empty;
                }
                return (string)ViewState[this.UniqueID + ".ExcludeMoreServices"];
            }
            set
            {
                ViewState.Add(this.UniqueID + ".ExcludeMoreServices", value);
            }
        }

        /// <summary>
        /// name of template to use for emailed shares
        /// </summary>
        public string EmailTemplateName
        {
            get
            {
                if (ViewState[this.UniqueID + ".EmailTemplateName"] == null)
                {
                    return string.Empty;
                }
                return (string)ViewState[this.UniqueID + ".EmailTemplateName"];
            }
            set
            {
                ViewState.Add(this.UniqueID + ".EmailTemplateName", value);
            }
        }

        /// <summary>
        /// Separator template, can be html or simple text.
        /// </summary>
        public string Separator
        {
            get
            {
                if (ViewState[this.UniqueID + ".Separator"] == null)
                {
                    return string.Empty;
                }
                return (string)ViewState[this.UniqueID + ".Separator"];
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
                if (ViewState[this.UniqueID + ".AddThisButtonAPI"] == null)
                {
                    if (string.IsNullOrEmpty(UserName))
                    {
                        return "http://s7.addthis.com/js/250/addthis_widget.js?pub=xa-4a60bb071de35881";
                    }
                    else
                    {
                        return "http://s7.addthis.com/js/250/addthis_widget.js#username=" + UserName;
                    }
                }
                else
                {
                    return (string)(ViewState[this.UniqueID + ".AddThisButtonAPI"]);
                }
            }
            set
            {
                ViewState.Add(this.UniqueID + ".AddThisButtonAPI", value);
            }
        }

        public string Style
        {
            get
            {
                if (ViewState[this.UniqueID + ".Style"] == null)
                {
                    return string.Empty;
                }
                return (string)ViewState[this.UniqueID + ".Style"];
            }
            set
            {
                ViewState.Add(this.UniqueID + ".Style", value);
            }
        }

        public string CssClass
        {
            get
            {
                if (ViewState[this.UniqueID + ".CssClass"] == null)
                {
                    return string.Empty;
                }
                return (string)ViewState[this.UniqueID + ".CssClass"];
            }
            set
            {
                ViewState.Add(this.UniqueID + ".CssClass", value);
            }
        }

        public string BookMarkURL
        {
            get
            {
                if (ViewState[this.UniqueID + ".BookMarkURL"] == null)
                {
                    if (string.IsNullOrEmpty(UserName))
                    {
                        return "http://www.addthis.com/bookmark.php?v=250&pub=xa-4a60bb071de35881";
                    }
                    else
                    {
                        return "http://www.addthis.com/bookmark.php?v=250&amp;username=" + UserName;
                    }
                }
                else
                {
                    return (string)ViewState[this.UniqueID + ".BookMarkURL"];
                }
            }
            set
            {
                ViewState.Add(this.UniqueID + ".BookMarkURL", value);
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Renders HTML Similar to this:
        /// <div class="addthis_toolbox addthis_default_style" 
        ///     <%= (!string.IsNullOrEmpty(URL)? "addthis:url='" + URL + "'" : ((Request.Url != null)? "addthis:url='" + Request.Url.ToString() + "'" : string.Empty)) %>
        ///     <%= !string.IsNullOrEmpty(Title)? "addthis:title='" + Title + "'" : string.Empty %>
        ///     <%= !string.IsNullOrEmpty(SelectedLanguage)? "addthis:ui_language='" + SelectedLanguage + "'" : string.Empty %>>
        ///         <%= BuildMenuServices() %>
        ///         <%= !string.IsNullOrEmpty(Separator)? "<span class='addthis_separator'>" + Separator + "</span>" : string.Empty %>
        ///         <a href="http://www.addthis.com/bookmark.php?v=250&pub=xa-4a60bb071de35881" class="addthis_button_expanded"><%= TextMore %></a>
        /// </div>
        /// </summary>
        /// <param name="writer"></param>
        private void BuildHtml(HtmlTextWriter writer)
        {
            writer.WriteLine("<div class='" + ((!string.IsNullOrEmpty(CssClass)) ? CssClass : "addthis_toolbox addthis_default_style") + "'" + ((!string.IsNullOrEmpty(Style)) ? "style='" + Style + "'" : string.Empty));
                writer.WriteLine((!string.IsNullOrEmpty(URL) ? "addthis:url='" + URL + "'" : ((this.Page.Request.Url != null) ? "addthis:url='" + this.Page.Request.Url.ToString() + "'" : string.Empty)));
                writer.WriteLine(!string.IsNullOrEmpty(Title) ? "addthis:title='" + Title + "'" : string.Empty);
                writer.WriteLine(!string.IsNullOrEmpty(EmailTemplateName) ? "addthis:email_template='" + EmailTemplateName + "'" : string.Empty);
                writer.WriteLine((!string.IsNullOrEmpty(SelectedLanguage) ? "addthis:ui_language='" + SelectedLanguage + "'" : string.Empty) + ">");
                writer.WriteLine("<table><tr>");
                writer.WriteLine(BuildMenuServices() + (!string.IsNullOrEmpty(Separator) ? "<td><span class='addthis_separator'>" + Separator + "</span></td>" : string.Empty) + "<td><a href='" + BookMarkURL + "' class='addthis_button_expanded'>" + TextMore + "</a></td>");
                writer.WriteLine("</tr></table>");
            writer.WriteLine("</div>");
        }

        private void BuildConfigurationAndSharingOptions()
        {
            if (!this.Page.ClientScript.IsClientScriptBlockRegistered(this.Page.GetType(), "AddThisButtonConfigurationScript"))
            {
                StringBuilder script = new StringBuilder();
                script.Append("<script type='text/javascript' language='javascript'>");
                /*#### CONFIGURATION ####*/
                script.Append("var addthis_config=");
                script.Append("{");
                script.Append(!string.IsNullOrEmpty(IncludeMoreServices) ? "services_expanded: '" + IncludeMoreServices + "'" + (!string.IsNullOrEmpty(ExcludeMoreServices)?",":"") : string.Empty);
                    script.Append(!string.IsNullOrEmpty(ExcludeMoreServices) ? "services_exclude: '" + ExcludeMoreServices + "'" : string.Empty);
                script.Append("};");
                /*#### SHARING ##########*/
                script.Append("var addthis_share=");
                script.Append("{");
                script.Append("};");
                script.Append("</script>");

                this.Page.ClientScript.RegisterClientScriptBlock(this.Page.GetType(), "AddThisButtonConfigurationScript", script.ToString());
            }
        }

        private void RegisterAddThisButtonAPI()
        {
            if (!this.Page.ClientScript.IsClientScriptBlockRegistered(this.Page.GetType(), "AddThisButtonAPI"))
            {
                StringBuilder script = new StringBuilder();
                script.Append("<script type='text/javascript' src='" + AddThisButtonAPI + "'></script>");
                this.Page.ClientScript.RegisterClientScriptBlock(this.Page.GetType(), "AddThisButtonAPI", script.ToString());
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
            BuildHtml(writer);
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
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
                    if (!string.IsNullOrEmpty(service))
                    {
                        htmlResult += "<td><a class='addthis_button_" + service + "'></a></td>";
                    }
                }
                return htmlResult;
            }
            return string.Empty;
        }
        #endregion
    }
}
