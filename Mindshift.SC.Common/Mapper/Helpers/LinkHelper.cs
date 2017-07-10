using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Web;
using Sitecore.Sites;
using Sitecore.Data;

namespace Mindshift.SC.Common.Mapper.Helpers
{
    public class LinkHelper
    {
        public static string GetMediaUrl(MediaItem mediaItem)
        {
            if (mediaItem == null) return string.Empty;
            return Sitecore.StringUtil.EnsurePrefix('/', Sitecore.Resources.Media.MediaManager.GetMediaUrl(mediaItem));
        }

        public static string GetItemURL(Item item)
        {
            
            UrlOptions options = (UrlOptions)LinkManager.GetDefaultUrlOptions().Clone();
            options.SiteResolving = true;
            options.EncodeNames = true;
            options.Language = item.Language;
            return LinkManager.GetItemUrl(item, options);
        }

        public static string GetLinkFieldUrl(LinkField field)
        {
            if (field == null) return string.Empty;

            //If it is an internal link return the URL to the item
            if (field.IsInternal)
            {
                Item targetItem;
                if(Sitecore.Context.Database == null)
                {
                    targetItem = field.TargetItem;
                }
                else
                {
                    targetItem = Sitecore.Context.Database.GetItem(field.TargetID);
                }

                if (targetItem == null)
                {
                    return string.Empty;
                }

                SiteInfo siteInfo = SiteContextFactory.Sites
                                        .Where(s => s.RootPath != "" && targetItem.Paths.Path.ToLower().StartsWith(s.RootPath.ToLower()))
                                        .OrderByDescending(s => s.RootPath.Length)
                                        .FirstOrDefault();

                SiteContext siteContext = SiteContextFactory.GetSiteContext(siteInfo.Name);

                UrlOptions options = (UrlOptions)LinkManager.GetDefaultUrlOptions().Clone();
                options.SiteResolving = true;
                options.EncodeNames = true;
                options.Site = siteContext;
                options.Language = targetItem.Language;

                return LinkManager.GetItemUrl(targetItem, options);
            }

            //If it is a media link, return the media path
            if (field.IsMediaLink)
            {
                if (field.TargetItem == null) return string.Empty;
                return Sitecore.StringUtil.EnsurePrefix('/', Sitecore.Resources.Media.MediaManager.GetMediaUrl(field.TargetItem));
            }

            //Return the url if it is not a 
            if (field.Url == null) return string.Empty;
            return field.Url;
        }
    }
}
