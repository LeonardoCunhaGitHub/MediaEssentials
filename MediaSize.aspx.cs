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
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using Sitecore.Diagnostics;
using Sitecore.Mvc.Extensions;
using Sitecore.Resources.Media;
using Sitecore.Zip;

namespace MediaEssentials
{
    public partial class MediaSize : System.Web.UI.Page
    {

        private readonly MediaLibraryUtils _mediaLibrary = new MediaLibraryUtils();

        protected void Page_Load(object sender, EventArgs e)
        {
            var scriptManager = ScriptManager.GetCurrent(Page);
            scriptManager?.RegisterPostBackControl(btnDownload);

            if (IsPostBack) return;

            Session.Clear();

            _mediaLibrary.SetDatabaseDropDown(ddDataBase);

            _mediaLibrary.SetMediaFoldersDropDown(ddMediaFolders, ddDataBase);

            btnDownload.Visible = false;

        }

        protected void ddDataBase_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            _mediaLibrary.SetMediaFoldersDropDown(ddMediaFolders, ddDataBase);
        }


  
        protected void btnDownload_OnClick(object sender, EventArgs e)
        {
            if (Session["filePath"] == null || (string)Session["filePath"] == "") return;

            var f = (string)Session["exportFileNameWithExtension"];
            var p = (string)Session["filePath"];


            Response.Clear();
            Response.AppendHeader("content-disposition",
                "attachment; filename=\"" + f + "\""); //firefox has issue with filenames with SPACE so use this sintaxe
            Response.ContentType = "application/zip";
            Response.TransmitFile(p);
            Response.End();
        }

        protected void btnFilterMediaBySize_OnClick(object sender, EventArgs e)
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

            var totalMediaIdentified = 0;
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
                    //---------------------------------------------------------------------------

                    var size = (double)mediaData.MediaItem.Size / 1024 / 1024;
                   

                   
                    output.AppendLine("---- Item ----");
                    output.AppendLine("Name: " + item.Name);
                    output.AppendLine("ID: " + item.ID);
                    output.AppendLine("Path: " + item.Paths.Path);
                    output.AppendLine("Language: " + l.Name);
                    output.AppendLine("Size: " + size.ToString("##.#"));
                    output.AppendLine();

                    totalMediaIdentified++;

                }

            }

            if (totalMediaIdentified == 0)
            {
                output.AppendLine("No media items found within the options set.");

                return;
            }

            output.AppendLine();
            output.AppendLine("---- Total ----");
            output.AppendLine();
            output.AppendLine("Media Items found: " + totalMediaIdentified + " including all languages.");

            //output of last execution
            lbOutput.Text = output.ToString().Replace(Environment.NewLine, "<br />");
            //---------------------------------------------------------------------------
        }


     
    }
}