using Mindshift.SC.Common.Mapper.Base;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mindshift.SC.Common.Mapper.Items
{
    public class MediaIncludeItem : BaseModel, IRenderingModel
    {

        private List<ListMediaItem> mediaItems;

        void IRenderingModel.Initialize(Rendering rendering)
        {
            InnerItem = rendering.Item;
        }

        public MediaIncludeItem()
            : base()
        {

        }

        public MediaIncludeItem(Item item)
        {
            InnerItem = item;
        }

        public List<ListMediaItem> MediaItems
        {

            get
            {
                if (mediaItems == null)
                {
                    mediaItems = new List<ListMediaItem>();
                    if (InnerItem.TemplateName.ToLower() == "media folder")
                    {
                        if (InnerItem.HasChildren)
                        {
                            foreach (Item i in InnerItem.Children)
                            {
                                ListMediaItem lmi = new ListMediaItem(i);
                                mediaItems.Add(lmi);
                            }
                        }
                    }
                    else
                    {
                        ListMediaItem lmi = new ListMediaItem(InnerItem);
                         mediaItems.Add(lmi);
                    }

                    return mediaItems;
                }
                else
                {
                    return mediaItems;
                }

            }
        }
    }
}
