using Mindshift.SC.Common.Mapper.Fields.Base;
using Mindshift.SC.Common.Mapper.Helpers;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Resources.Media;
using Sitecore.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mindshift.SC.Common.Mapper.Fields.Rendering
{
    public class ImgFld : BaseRenderingField
    {
        private MediaItem _imageItem;

        public ImgFld(string paramValue)
            : base(paramValue)
        {

        }

        
		public MediaItem MediaItem
		{
			get
			{
                if (!String.IsNullOrEmpty(ParamValue) && !ParamValue.Contains("|") && _imageItem == null)
                {
                    var imageId = XmlUtil.GetAttribute("mediaid", XmlUtil.LoadXml(ParamValue));
                    _imageItem = Sitecore.Context.Database.GetItem(imageId);
                }

                return _imageItem;
			}
		}

		public string MediaUrl
		{
			get
			{
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

        public string Title
        { 
            get
            {
                return MediaItem.Title;
            }
        }

        
        
    }
}
