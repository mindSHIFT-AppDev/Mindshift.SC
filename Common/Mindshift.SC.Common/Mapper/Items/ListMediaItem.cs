using Mindshift.SC.Common.Mapper.Base;
using Mindshift.SC.Common.Mapper.Helpers;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mindshift.SC.Common.Mapper.Items
{
    public class ListMediaItem : BaseModel, IRenderingModel
    {
        
        protected MediaItem mediaItem;
        //private List<MediaItem> mediaItems;

        void IRenderingModel.Initialize(Rendering rendering)
        {
            InnerItem = rendering.Item;
        }

        public ListMediaItem()
            : base()
        {

        }

        public ListMediaItem(Item item)
        {
            InnerItem = item;
        }

        
        public MediaItem MediaItem
        {
            get
            {
                if(mediaItem != null)
                {
                    return mediaItem;
                }

                mediaItem = (Sitecore.Data.Items.MediaItem)InnerItem;
                return mediaItem;

            }
        }
        
        public string MediaUrl
        {
            get { return LinkHelper.GetMediaUrl(MediaItem); }
        }


    }
}
