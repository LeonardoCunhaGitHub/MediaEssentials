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
        public string AutoFillAltTags = "";
        public string ExportMedia = "";
        public string MeVersion = "";
        public string UnusedMedia = "";
        public string MediaReferences = "";
        public string MediaUpdates = "";
        public string MediaSize = "";

        public string MediaEssentialsURL =  "http://" +
            HttpContext.Current.Request.Url.Host + @"/sitecore/admin/mediaessentials/default.aspx";



        protected void Page_Load(object sender, EventArgs e)
        {
            //This condition allows only Administrator to access this page.
            if (!Sitecore.Context.User.IsAdministrator)
                Response.Redirect("http://" + HttpContext.Current.Request.Url.Host +
                                  "/sitecore/login?returnUrl=%2fsitecore%2fadmin%2fmediaessentials%2f");


            SetActiveNavigation();

            lnkDashboard.NavigateUrl = MediaEssentialsURL;

            //var v = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            var v = new Version(2, 0, 0, 0);
            MeVersion = v.Major + "." + v.Minor;
        }


        #region ***** F U C T I O N S *****

        private void SetActiveNavigation()
        {
            var pagename = Convert.ToString(GetPageName()).ToLower();
            switch (pagename)
            {
                case "exportmedia.aspx":
                    ExportMedia = "active";
                    break;
                case "autofillalttags.aspx":
                    AutoFillAltTags = "active";
                    break;
                case "unusedmedia.aspx":
                    UnusedMedia = "active";
                    break;
                case "mediaReferences.aspx":
                    MediaReferences = "active";
                    break;
                case "mediaupdates.aspx":
                    MediaUpdates = "active";
                    break;
                case "mediasize.aspx":
                    MediaSize = "active";
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