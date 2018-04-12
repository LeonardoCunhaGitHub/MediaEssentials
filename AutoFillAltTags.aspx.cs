using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaEssentials.Common;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Managers;
using Sitecore.Mvc.Extensions;
using Sitecore.Resources.Media;

namespace MediaEssentials
{
    public partial class AutoFillAltTags : System.Web.UI.Page
    {
        public readonly MediaLibraryUtils MediaLibrary = new MediaLibraryUtils();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (IsPostBack) return;

            MediaLibrary.SetDatabaseDropDown(ddDataBase);

            MediaLibrary.SetMediaFoldersDropDown(ddMediaFolders, ddDataBase);

            chkOnlyEmptyAltTags.Checked = true;
        }

        protected void ddDataBase_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            MediaLibrary.SetMediaFoldersDropDown(ddMediaFolders, ddDataBase);
        }


        protected void btnAutofill_OnClick(object sender, EventArgs e)
        {
            //get selected folder
            var itemId = new ID(ddMediaFolders.SelectedValue);

            var db = Database.GetDatabase(ddDataBase.SelectedValue.ToLower());

            var selectedFolder = db.Items.GetItem(itemId);


            var mediaLibrary = db.GetItem(MediaLibraryUtils.MediaLibraryId);

            var allMediaItems = MediaLibrary.GetMediaItems(db, selectedFolder, mediaLibrary,
                chkIncludeSubFolders.Checked, chkIncludeSystemFolder.Checked);

            var output = new StringBuilder();

            if (allMediaItems.Count == 0)
            {
                output.Clear();
                output.AppendLine("No media ALT tag to be updated within the options set.");

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

            var totalMediaUpdated = 0;
            foreach (var m in allMediaItems)
            {
                //if it belongs to SYSTEM images and system is not included then do not proccess
                if (m.Paths.Path.ToLower().Contains(mediaLibrary.Paths.Path + "/system") &&
                    !chkIncludeSystemFolder.Checked) break;

                //if it matches any of the templates on excludedTemplate then do not proccess
                var match = excludedTemplates.FirstOrDefault(x => x.Contains(m.Template.ID.ToString()));

                if (match != null) continue;

                //loop all items and export them to a csv file 

                foreach (var l in installedLanguages)
                {
                    var item = db.GetItem(m.ID, l);
                    var mediaData = new MediaData(item);

                    mediaData.MediaItem.BeginEdit();

                    //only update if alt is empty
                    if (chkOnlyEmptyAltTags.Checked)
                    {
                        if (!mediaData.MediaItem.Alt.IsEmptyOrNull()) continue;

                    }


                    mediaData.MediaItem.BeginEdit();

                    mediaData.MediaItem.Alt = item.Name;
                    mediaData.MediaItem.EndEdit();

                    output.AppendLine("---- Item ----");
                    output.AppendLine("Name: " + item.Name);
                    output.AppendLine("ID: " + item.ID);
                    output.AppendLine("Path: " + item.Paths.Path);
                    output.AppendLine("Language: " + l.Name);
                    output.AppendLine("Alt: " + mediaData.MediaItem.Alt);
                    output.AppendLine();

                    totalMediaUpdated++;

                }

            }

            if (totalMediaUpdated == 0)
            {
                output.AppendLine("No media ALT tag to be updated within the options set.");

                return;
            }

            output.AppendLine();
            output.AppendLine("---- Total ----");
            output.AppendLine();
            output.AppendLine("Items Updated: " + totalMediaUpdated + " including all languages.");

            //output of last execution
            lbOutput.Text = output.ToString().Replace(Environment.NewLine, "<br />");
        }
    }
}