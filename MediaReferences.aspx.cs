using MediaEssentials.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MediaEssentials
{
    public partial class MediaReferences : System.Web.UI.Page
    {

        private readonly MediaLibraryUtils _mediaLibrary = new MediaLibraryUtils();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            _mediaLibrary.SetDatabaseDropDown(ddDataBase);

            _mediaLibrary.SetMediaFoldersDropDown(ddMediaFolders, ddDataBase);
        }

        protected void ddDataBase_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            _mediaLibrary.SetMediaFoldersDropDown(ddMediaFolders, ddDataBase);
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {

        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {

        }
    }
}