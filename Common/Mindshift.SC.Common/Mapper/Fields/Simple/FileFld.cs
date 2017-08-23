using Mindshift.SC.Common.Mapper.Fields.Base;
using Mindshift.SC.Common.Mapper.Helpers;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace Mindshift.SC.Common.Mapper.Fields.Simple
{
	public partial class FileFld : BaseField<FileField>
	{
        public FileFld(Item item, FileField field)
			: base(item, field)
		{
		}

        public static implicit operator MediaItem(FileFld fileField)
		{
			return ((fileField != null) ? fileField.MediaItem : null);
		}

		public MediaItem MediaItem
		{
			get
			{
				if (field == null) return null;
				if (item.Fields[field.InnerField.Name] == null) return null;
				return ((FileField)item.Fields[field.InnerField.Name]).MediaItem;
			}
		}

		public string MediaUrl
		{
			get { return LinkHelper.GetMediaUrl(MediaItem); }
		}
	}
}
