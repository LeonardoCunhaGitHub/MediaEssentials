using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using MediaEssentials.Common;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.IO;
using Sitecore.Zip;

namespace MediaEssentials
{
    public partial class ExportMedia : Page
    {
        private readonly MediaLibraryUtils _mediaLibrary = new MediaLibraryUtils();

        private string _exportFileNameWithExtension = "";

        private string _filePath = "";

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


        protected void ddDataBase_SelectedIndexChanged(object sender, EventArgs e)
        {
            _mediaLibrary.SetMediaFoldersDropDown(ddMediaFolders, ddDataBase);

        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            if (Session["filePath"] == null || (string) Session["filePath"] == "") return;

            var f = (string) Session["exportFileNameWithExtension"];
            var p = (string) Session["filePath"];


            Response.Clear();
            Response.AppendHeader("content-disposition",
                "attachment; filename=\"" + f + "\""); //firefox has issue with filenames with SPACE so use this sintaxe
            Response.ContentType = "application/zip";
            Response.TransmitFile(p);
            Response.End();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            //clear session
            Session.Clear();

            btnDownload.Visible = false;

            //get selected folder
            var itemId = new ID(ddMediaFolders.SelectedValue);

            var db = Database.GetDatabase(ddDataBase.SelectedValue.ToLower());

            var selectedFolder = db.Items.GetItem(itemId);


            //set folder to export file
            var exportfolderName = Settings.DataFolder + "/MediaEssentials/ExportMedia/" + selectedFolder.Name;

            _exportFileNameWithExtension = selectedFolder.Name + ".zip";

            FileUtil.CreateFolder(FileUtil.MapPath(exportfolderName));


            //map file path
            _filePath = FileUtil.MapPath(FileUtil.MakePath(exportfolderName, _exportFileNameWithExtension, '/'));

            var mediaLibraryItem = db.GetItem(MediaLibraryUtils.MediaLibraryId);

            var allMediaItems = _mediaLibrary.GetMediaItems(db, selectedFolder, mediaLibraryItem,
                chkIncludeSubFolders.Checked, chkIncludeSystemFolder.Checked);


            //get total of images exported excluding the pre-defined templates below
            var excludedTemplates = new List<string>
            {
                TemplateIDs.MediaFolder.ToString(),
                TemplateIDs.MainSection.ToString(),
                TemplateIDs.Node.ToString()
            };

            var items = allMediaItems.ToArray();

            var totalImagesExported = items
                .Select(i => excludedTemplates.FirstOrDefault(x => x.Contains(i.Template.ID.ToString())))
                .Count(match => match == null);


            var output = new StringBuilder();

            if (totalImagesExported == 0)
            {
                output.Clear();
                output.AppendLine("There is no media to export within the options set.");

                //output of last execution
                lbOutput.Text = output.ToString().Replace(Environment.NewLine, "<br />");

                return;
            }

            

            //fill in output
            output.Clear();

            output.AppendLine("Total of Images Exported: " + totalImagesExported);

            output.AppendLine("File Location on Server: " + exportfolderName);

            output.AppendLine();
            output.AppendLine("---- Media Items Exported ----");
            output.AppendLine();

            var templates = items.GroupBy(x => x.TemplateID);


            foreach (IGrouping<Sitecore.Data.ID,Item> item in templates)
            {
                if (item.Key == TemplateIDs.MediaFolder ||
                    item.Key == TemplateIDs.MainSection ||
                    item.Key == TemplateIDs.Node) continue;

                foreach (var i in item)
                {
                    output.AppendLine("Item ID: " + i.ID);
                    output.AppendLine("Item Name: " + i.Name);
                    output.AppendLine("Item Path: " + i.Paths.Path);
                    output.AppendLine();
                }
                
            }


            ZipWriter = new ZipWriter(_filePath);

            foreach (var i in items)
            {
                if (!chkIncludeSystemFolder.Checked && i.Paths.Path.ToLower()
                        .Contains(mediaLibraryItem.Paths.Path.ToLower() + "/system")) continue;

                ProcessMediaItems(i );
            }

            ZipWriter.Dispose();


            Session["filePath"] = _filePath;
            Session["exportFileNameWithExtension"] = _exportFileNameWithExtension;

            btnDownload.Visible = true;

            //output of last execution
            lbOutput.Text = output.ToString().Replace(Environment.NewLine, "<br />");
        }


        #region ***** F U C T I O N S *****

        protected ZipWriter ZipWriter { get; private set; }

        public void ProcessMediaItems(Item rootMediaItem )
        {
            if (rootMediaItem.TemplateID == TemplateIDs.MediaFolder ||
                rootMediaItem.TemplateID == TemplateIDs.MainSection ||
                rootMediaItem.TemplateID == TemplateIDs.Node) return;

            var mediaItem = new MediaItem(rootMediaItem);
            ZipFile(mediaItem);

        }

        private void ZipFile(MediaItem mediaItem)
        {
            Assert.ArgumentNotNull(mediaItem, "mediaItem");

            using (var stream = mediaItem.GetMediaStream())
            {
                if (stream == null)
                {
                    lbOutput.Text = @"Cannot find media data for item '{mediaItem.MediaPath}'";
                    return;
                }

                var mediaExtension = string.IsNullOrEmpty(mediaItem.Extension) ? "" : "." + mediaItem.Extension;

                ZipWriter.AddEntry(mediaItem.MediaPath.Substring(1) + mediaExtension, stream);
            }
        }

        #endregion
    }
}