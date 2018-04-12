using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace MediaEssentials.Common
{
    public class MediaLibraryUtils
    {
        public const string MediaLibraryId = "{3D6658D8-A0BF-4E75-B3E2-D050FABCF4E1}";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dd"></param>
        public void SetDatabaseDropDown(DropDownList dd)
        {
            var databases = Factory.GetDatabaseNames();

            dd.Items.Clear();

            foreach (var db in databases)
            {
                var mediaLibrary = Database.GetDatabase(db).GetItem(MediaLibraryId);

                if (mediaLibrary == null) continue;

                var allMediaItems = mediaLibrary.GetChildren().ToList();

                //if there is no media then don't list this database
                if (allMediaItems.Count > 0) dd.Items.Add(new ListItem(db, db));
            }
        }

        /// <summary>
        /// fill in media folder based on database selected on dropdown
        /// </summary>
        public void SetMediaFoldersDropDown(DropDownList dd, DropDownList ddDatabase)
        {
            var db = Database.GetDatabase(ddDatabase.SelectedValue.ToLower());

            var mediaLibrary = db.GetItem(MediaLibraryId);

            //load the media library folder dropdown
            var allMediaItems = mediaLibrary.GetChildren().ToList();

            //clear dropdown and include the media library root
            dd.Items.Clear();

            AddItemToDropDown(dd, mediaLibrary);

            //add items to dropdown
            foreach (var i in allMediaItems)
            {
                //only include media folders into the dropdown
                if (i.TemplateID != TemplateIDs.MediaFolder &&
                    i.TemplateID != TemplateIDs.Node) continue;

                //do not include system folders into the dropdown
                if (!i.Paths.Path.ToLower().Contains(mediaLibrary.Paths.Path + "/system")) AddItemToDropDown(dd, i);
            }
        }

        public static void AddItemToDropDown(DropDownList dd, Item i)
        {
            if (dd != null) dd.Items.Add(new System.Web.UI.WebControls.ListItem(i.Paths.Path.Replace("/sitecore/", ""), i.ID.ToString()));
        }


        public List<Item> GetMediaItems(Database db, Item selectedFolder, Item mediaLibrary, bool includeSubFolders,
            bool includeSystemFolder)
        {

            //postback will clear allMediaItems so we need to get values again
            var allMediaItems = includeSubFolders
                ? db.SelectItems(selectedFolder.Paths.Path + "//*").ToList()
                : new List<Item>(selectedFolder.GetChildren()
                    .Where(x => x.TemplateID != TemplateIDs.MediaFolder && x.TemplateID != TemplateIDs.Node && x.TemplateID != TemplateIDs.MainSection));

            //if user selected media library root and included system
            if (includeSystemFolder && selectedFolder.Paths.Path == mediaLibrary.Paths.Path) return allMediaItems;

            //if user selected media library root and system is not included
            if (!includeSystemFolder && selectedFolder.Paths.Path == mediaLibrary.Paths.Path)
            {
                var listNoSystem = new List<Item>();
                foreach (var i in allMediaItems)
                {
                    var systemPath = mediaLibrary.Paths.Path.ToLower() + "/system";
                    if (!i.Paths.Path.ToLower().Contains(systemPath))
                    {
                        listNoSystem.Add(i);
                    }

                }

                return listNoSystem;
                //return new List<Item>(allMediaItems.Where(x => x.Paths.Path.Contains(mediaLibrary.Paths.Path + "/system")));
            }

            //if user did not select media library root and system is included then add the System folder
            if (includeSystemFolder && selectedFolder.Paths.Path != mediaLibrary.Paths.Path)
            {
                allMediaItems.AddRange(db.SelectItems(mediaLibrary.Paths.Path + "/system//*"));
            }

            return allMediaItems;
        }

        public List<Item> GetUnusedMedia(Database db, Item selectedFolder, Item mediaLibrary, bool includeSubFolders,
            bool includeSystemFolder)
        {
            var allMedia = GetMediaItems(db, selectedFolder, mediaLibrary, includeSubFolders, includeSystemFolder);

            var unusedMedia = new List<Item>();


            //get total of images exported excluding the pre-defined templates below
            var excludedTemplates = new List<string>
            {
                TemplateIDs.MediaFolder.ToString(),
                TemplateIDs.MainSection.ToString(),
                TemplateIDs.Node.ToString()
            };

            foreach (var media in allMedia)
            {


                //if it matches any of the templates on excludedTemplate then do not proccess
                var match = excludedTemplates.FirstOrDefault(x => x.Contains(media.Template.ID.ToString()));
                if (match != null) continue;

                if (Globals.LinkDatabase.GetReferrerCount(media) == 0)
                {
                    unusedMedia.Add(media);
                }

            }

            return unusedMedia;
        }
    }
}