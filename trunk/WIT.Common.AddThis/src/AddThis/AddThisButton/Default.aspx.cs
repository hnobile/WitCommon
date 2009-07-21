using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WIT.Common.AddThis
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AddThisButton1.MenuServices =
                WellKnownKeys.ui_service_facebook + "," +
                WellKnownKeys.ui_service_email + "," +
                WellKnownKeys.ui_service_twitter + "," +
                WellKnownKeys.ui_service_favorites + ",";

            AddThisButton2.MenuServices =
                WellKnownKeys.ui_service_facebook + "," +
                WellKnownKeys.ui_service_email + "," +
                WellKnownKeys.ui_service_twitter + "," +
                WellKnownKeys.ui_service_favorites + ",";
        }
    }
}
