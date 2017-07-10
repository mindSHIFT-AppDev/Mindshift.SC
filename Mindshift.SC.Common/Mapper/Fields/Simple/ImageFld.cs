using Mindshift.SC.Common.Mapper.Fields.Base;
using Mindshift.SC.Common.Mapper.Helpers;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Web.UI.WebControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;


namespace Mindshift.SC.Common.Mapper.Fields.Simple
{
    [Serializable]
    public class ImageFld : BaseField<ImageField>
    {
        public ImageFld(Item item, ImageField field)
			: base(item, field)
		{
		}

        public static implicit operator MediaItem(ImageFld imageField)
		{
			return ((imageField != null) ? imageField.MediaItem : null);
		}

		public MediaItem MediaItem
		{
			get
			{
                if (field == null)
                {
                    return null;
                }

                if (field.InnerField == null)
                {
                    return null;
                }

                if (field.InnerField.Name == "")
                {
                    return null;
                }

                if (item.Fields[field.InnerField.Name] == null)
                {
                    return null;
                }

				return ((ImageField)item.Fields[field.InnerField.Name]).MediaItem;
			}
		}

		public string MediaUrl
		{
			get
			{
                if (MediaItem == null)
                {
                    return "";
                }

				return LinkHelper.GetMediaUrl(MediaItem);
			}
		} 

        public string Alt 
        {
            get
            {
                return MediaItem.Alt; 
            }
        }
        //attribute is Height
        public string Height
        {
            get
            {
                return field.Height;
            }
        }

        //attribute is Width
        public string Width
        {
            get
            {
                return field.Width;
            }
        }
        /*
        public string Border { get; set; }
        public string Class { get; set; }
        public int Height { get; set; }
        public int HSpace { get; set; }
        public string Src { get; set; }
        public int VSpace { get; set; }
        public int Width { get; set; }
        public Guid MediaId { get; set; }
         */
        public string Title
        { 
            get
            {
                return MediaItem.Title;
            }
        }

        public HtmlString RenderWithAttributes()
        {
            string attributes = "";

            if (field.Height != "")
            {
                attributes += " height=" + field.Height + "&";
            }

            if (field.Width != "")
            {
                attributes += " width=" + field.Width + "&";
            }

            if (MediaItem.Alt != "")
            {
                attributes += " alt=" + MediaItem.Alt + "&";
            }
            return new HtmlString(FieldRenderer.Render(item, field.InnerField.Name, attributes)); 
                
           // return new HtmlString(attributes);
        }

    }
}
