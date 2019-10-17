using MediaEssentials.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Managers;
using Sitecore.Resources.Media;

namespace MediaEssentials
{
    public partial class MediaUpdates : System.Web.UI.Page
    {
        private readonly MediaLibraryUtils _mediaLibrary = new MediaLibraryUtils();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            lnkDashboard.NavigateUrl = ((Layout)this.Master)?.MediaEssentialsURL;

            calAfterDate.SelectedDate = DateTime.Today;

            _mediaLibrary.SetDatabaseDropDown(ddDataBase);

            _mediaLibrary.SetMediaFoldersDropDown(ddMediaFolders, ddDataBase);
        }

        protected void ddDataBase_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            _mediaLibrary.SetMediaFoldersDropDown(ddMediaFolders, ddDataBase);
        }


        protected void btnFindMedia_OnClick(object sender, EventArgs e)
        {
            //get selected folder
            var itemId = new ID(ddMediaFolders.SelectedValue);

            var db = Database.GetDatabase(ddDataBase.SelectedValue.ToLower());

            var selectedFolder = db.Items.GetItem(itemId);

            var mediaLibrary = db.GetItem(MediaLibraryUtils.MediaLibraryId);

            var allMediaItems = _mediaLibrary.GetMediaItems(db, selectedFolder, mediaLibrary,
                chkIncludeSubFolders.Checked, chkIncludeSystemFolder.Checked);

            var output = new StringBuilder();

            if (allMediaItems.Count == 0)
            {
                output.Clear();
                output.AppendLine("No media found within the options set.");

                //output of last execution
                lbOutput.Text = output.ToString().Replace(Environment.NewLine, "<br />");

                return;
            }

            //fill in output
            output.Clear();

            var installedLanguages = LanguageManager.GetLanguages(db);

            var excludedTemplates = new List<string>
            {
                TemplateIDs.MediaFolder.ToString(),
                TemplateIDs.MainSection.ToString(),
                TemplateIDs.Node.ToString()
            };
            //                                           "doc", "docx", "pdf", "xls", "xlsx"

            var selectedDate = calAfterDate.SelectedDate;


            var totalMediaIdentified = 0;
            foreach (var m in allMediaItems)
            {
                //if it belongs to SYSTEM images and system is not included then do not proccess
                if (m.Paths.Path.ToLower().Contains(mediaLibrary.Paths.Path + "/system") &&
                    !chkIncludeSystemFolder.Checked) break;

                //if it matches any of the templates on excludedTemplate then do not proccess
                var match = excludedTemplates.FirstOrDefault(x => x.Contains(m.Template.ID.ToString()));

                if (match != null) continue;

                //loop all items and check its size
                foreach (var l in installedLanguages)
                {
                    var add = false;
                    var item = db.GetItem(m.ID, l);
                    var mediaData = new MediaData(item);


                    var imgUploadedDate = mediaData.MediaItem.InnerItem.Statistics.Updated;

                    add = (imgUploadedDate.Date >= selectedDate.Date);
                    

                    if (!add) continue;

                    output.AppendLine("---- Item ----");
                    output.AppendLine("Name: " + item.Name);
                    output.AppendLine("ID: " + item.ID);
                    output.AppendLine("Path: " + item.Paths.Path);
                    output.AppendLine("Language: " + l.Name);
                    output.AppendLine("Uploaded Date: " + imgUploadedDate.Month + "/" + imgUploadedDate.Day + "/" + imgUploadedDate.Year);
                    output.AppendLine();

                    totalMediaIdentified++;

                }

            }

            if (totalMediaIdentified == 0)
            {
                output.AppendLine("No media items found within the options set.");

                //output of last execution
                lbOutput.Text = output.ToString().Replace(Environment.NewLine, "<br />");

                return;
            }

            output.AppendLine();
            output.AppendLine("---- Total ----");
            output.AppendLine();
            output.AppendLine("Media Items found: " + totalMediaIdentified + " including all languages.");

            //output of last execution
            lbOutput.Text = output.ToString().Replace(Environment.NewLine, "<br />");

        }
    }
}