using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MediaEssentials
{
    public partial class Layout : MasterPage
    {
        public string Autofillalttags = "";
        public string Editalttags = "";
        public string Exportmedia = "";
        public string MeVersion = "";
        public string Unusedmedia = "";


        protected void Page_Load(object sender, EventArgs e)
        {
            //This condition allows only Administrator to access this page.
            if (!Sitecore.Context.User.IsAdministrator)
                Response.Redirect("http://" + HttpContext.Current.Request.Url.Host +
                                  "/sitecore/login?returnUrl=%2fsitecore%2fadmin%2fmediaessentials%2f");

            SetActiveNavigation();
            //var v = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            var v = new Version(1, 2, 0, 0);
            MeVersion = v.Major + "." + v.Minor;
        }


        #region ***** F U C T I O N S *****

        private void SetActiveNavigation()
        {
            var pagename = Convert.ToString(GetPageName()).ToLower();
            switch (pagename)
            {
                case "exportmedia.aspx":
                    Exportmedia = "active";
                    break;
                case "autofillalttags.aspx":
                    Autofillalttags = "active";
                    break;
                case "unusedmedia.aspx":
                    Unusedmedia = "active";
                    break;
            }
        }

        private object GetPageName()
        {
            return Request.Url.ToString().Split('/').Last();
        }

        #endregion
    }
}