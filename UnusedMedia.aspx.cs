using MediaEssentialsSitecoreModule.Common;
using Sitecore.Data;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediaEssentialsSitecoreModule
{
    public partial class UnusedMedia : System.Web.UI.Page
    {
        private readonly MediaLibraryUtils _mediaLibrary = new MediaLibraryUtils();


        private enum OutputTxt
        {
            Recycled,
            Deleted,
            Unused
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;


            _mediaLibrary.SetDatabaseDropDown(ddDataBase);

            _mediaLibrary.SetMediaFoldersDropDown(ddMediaFolders, ddDataBase);

            btnRecycleBin.Visible = false;

            btnDelete.Visible = false;
        }

        protected void ddDataBase_SelectedIndexChanged(object sender, EventArgs e)
        {
            _mediaLibrary.SetMediaFoldersDropDown(ddMediaFolders, ddDataBase);
        }


        protected void btnUnusedMedia_OnClick(object sender, EventArgs e)
        {

            btnRecycleBin.Visible = false;

            btnDelete.Visible = false;

            var unusedMedia = GetUnusedMediaBasedOnConfigurationSet();

            //if there is no unused media
            if (!Output(OutputTxt.Unused, unusedMedia)) return;

            btnRecycleBin.Visible = true;

            btnDelete.Visible = true;
        }


        protected void btnRecycleBin_OnClick(object sender, EventArgs e)
        {
            var unusedMedia = GetUnusedMediaBasedOnConfigurationSet();

            foreach (var item in unusedMedia)
            {
                item.Recycle();
            }

            Output(OutputTxt.Recycled, unusedMedia);
        }

        private List<Item> GetUnusedMediaBasedOnConfigurationSet()
        {
            //get selected folder
            var itemId = new ID(ddMediaFolders.SelectedValue);

            var db = Database.GetDatabase(ddDataBase.SelectedValue.ToLower());

            var selectedFolder = db.Items.GetItem(itemId);

            var mediaLibraryItem = db.GetItem(MediaLibraryUtils.MediaLibraryId);

            return _mediaLibrary.GetUnusedMedia(db, selectedFolder, mediaLibraryItem,
                chkIncludeSubFolders.Checked, chkIncludeSystemFolder.Checked);
        }

        protected void btnDelete_OnClick(object sender, EventArgs e)
        {
            var unusedMedia = GetUnusedMediaBasedOnConfigurationSet();

            foreach (var item in unusedMedia)
            {
                item.Delete();
            }

            Output(OutputTxt.Recycled, unusedMedia);
        }

        private bool Output(OutputTxt outputTxtStyle, List<Item> unusedMedia)
        {
            var output = new StringBuilder();

            if (unusedMedia.Count == 0)
            {
                output.Clear();
                output.AppendLine("There is no unused media found within the options set.");

                //output of last execution
                lbOutput.Text = output.ToString().Replace(Environment.NewLine, "<br />");

                return false;
            }

            //fill in output
            output.Clear();

            switch (outputTxtStyle)
            {
                case UnusedMedia.OutputTxt.Recycled:
                    output.AppendLine("Total of Unused Media RECYCLED: " + unusedMedia.Count);
                    break;
                case UnusedMedia.OutputTxt.Deleted:
                    output.AppendLine("Total of Unused Media DELETED: " + unusedMedia.Count);
                    break;
                case UnusedMedia.OutputTxt.Unused:
                    output.AppendLine("Total of Unused Media: " + unusedMedia.Count);
                    break;
            }


            output.AppendLine();
            output.AppendLine("---- List of Unused Items ----");
            output.AppendLine();

            foreach (var t in unusedMedia)
            {
                output.AppendLine("Item ID: " + t.ID);
                output.AppendLine("Item Name: " + t.Name);
                output.AppendLine("Item Path: " + t.Paths.Path);
                output.AppendLine();
            }


            //output of last execution
            lbOutput.Text = output.ToString().Replace(Environment.NewLine, "<br />");

            return true;
        }
    }
}