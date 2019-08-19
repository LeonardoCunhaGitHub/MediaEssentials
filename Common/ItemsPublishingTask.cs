using Sitecore.Data.Items;
using Sitecore.Publishing;
using Sitecore.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MediaEssentials.Common
{
    public class ItemsPublishingTask
    {
        public void Execute(Item[] itemArray, CommandItem commandItem, ScheduleItem scheduleItem)
        {
            foreach (Item item in itemArray)
            {
                PublishItem(item);
            }
        }


        public void PublishItem(Item item)
        {
            try
            {
                PublishOptions p = new PublishOptions(Sitecore.Data.Database.GetDatabase("master"), Sitecore.Data.Database.GetDatabase("web"), PublishMode.SingleItem, Sitecore.Context.Language, DateTime.Now);
                p.RootItem = item;
                p.Deep = true; // Publishing subitems

                (new Publisher(p)).Publish();
            }
            catch (Exception e)
            {
                Sitecore.Diagnostics.Log.Error("Exception, PublishItem: " + e.Message, this);
            }
        }
    }
}