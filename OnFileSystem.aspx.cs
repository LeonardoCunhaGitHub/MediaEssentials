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
using System.IO;

namespace MediaEssentials
{
    public partial class MediaReferences : System.Web.UI.Page
    {

        private readonly MediaLibraryUtils _mediaLibrary = new MediaLibraryUtils();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            lnkDashboard.NavigateUrl = ((Layout)this.Master)?.MediaEssentialsURL;

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
            // "doc", "docx", "pdf", "xls", "xlsx"


            var totalMediaIdentified = 0;
            foreach (var m in allMediaItems)
            {
                //if it belongs to SYSTEM images and system is not included then do not proccess
                if (m.Paths.Path.ToLower().Contains(mediaLibrary.Paths.Path + "/system") &&
                    !chkIncludeSystemFolder.Checked) break;

                //if it matches any of the templates on excludedTemplate then do not proccess
                var match = excludedTemplates.FirstOrDefault(x => x.Contains(m.Template.ID.ToString()));

                if (match != null) continue;

                //loop all items
                foreach (var l in installedLanguages)
                {
                    var add = false;
                    var item = db.GetItem(m.ID, l);
                    var mediaData = new MediaData(item);


                    var path = mediaData.MediaItem.FilePath;

                    add = !string.IsNullOrWhiteSpace(path);


                    if (!add) continue;

                    var size = mediaData.MediaItem.Size;

                    output.AppendLine("---- Item ----");
                    output.AppendLine("Name: " + item.Name);
                    output.AppendLine("ID: " + item.ID);
                    output.AppendLine("Item Path: " + item.Paths.Path);
                    output.AppendLine("File Path: " + path);
                    output.AppendLine("Language: " + l.Name);
                    output.AppendLine("Size: [" + size + " bytes] [" + (double)(size / 1000.00) + " Kb] [" + (float)(size / 1000000.00) + " Mb]");

                    if (chkUploadFileToDB.Checked)
                    {

                        try
                        {
                            path = HttpContext.Current.Server.MapPath(path);
                            output.AppendLine("Checking Path: " + path);
                            output.AppendLine("File Exists: " + System.IO.File.Exists(path));

                            //path = HttpContext.Current.Server.MapPath(path);


                            var fileInfo = new FileInfo(path);
                            if (fileInfo.Exists)
                            {
                                using (var fileStream = fileInfo.Open(FileMode.Open))
                                {
                                    var mediaCreator = new MediaCreator();
                                    var mediaItemFullPath = item.Paths.Path;
                                    var mediaCreatorOptions = new MediaCreatorOptions
                                    {
                                        Database = db,
                                        Language = Sitecore.Context.Language,
                                        Versioned = false,
                                        Destination = mediaItemFullPath,
                                        FileBased = false,
                                        IncludeExtensionInItemName = false,
                                        AlternateText = item.Name,
                                        OverwriteExisting = true
                                    };

                                    string fileName = item.Name + "." + mediaData.Extension;
                                    mediaCreator.AttachStreamToMediaItem(fileStream,
                                                                         mediaItemFullPath,
                                                                         fileName,
                                                                         mediaCreatorOptions);

                                    output.AppendLine("File Uploaded Successfuly");
                                }
                            }
                        }
                        catch (Exception exc)
                        {
                            output.AppendLine("Exception: " + exc.Message);
                        }
                    }

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