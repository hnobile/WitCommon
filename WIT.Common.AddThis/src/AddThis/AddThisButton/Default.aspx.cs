using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WIT.Common.AddThisButton.UserControls;
using WIT.Common.AddThisButton;

namespace AddThisButton
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AddThisButton1.IncludeMoreServices = AddThisButton2.IncludeMoreServices =
                WellKnownKeys.ui_service_facebook + "," +
                WellKnownKeys.ui_service_email + "," +
                WellKnownKeys.ui_service_favorites + "," +
                WellKnownKeys.ui_service_twitter;

            AddThisButton1.MenuServices = AddThisButton2.MenuServices =
                WellKnownKeys.ui_service_facebook + "," +
                WellKnownKeys.ui_service_email + "," +
                WellKnownKeys.ui_service_favorites + "," +
                WellKnownKeys.ui_service_twitter;
        }
    }
}
