using Mindshift.SC.Common.Mapper.Fields.Base;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace Mindshift.SC.Common.Mapper.Fields.List
{
	public partial class GroupedDroplinkFld: BaseField<GroupedDroplinkField>
	{
        public GroupedDroplinkFld(Item item, GroupedDroplinkField field)
			: base(item, field)
		{
		}

        public static implicit operator Item(GroupedDroplinkFld droplinkField)
		{
			return droplinkField.Item;
		}

		public Item Item
		{
			get
			{
				if (field == null) return null;
				if (item.Fields[field.InnerField.Name] == null) return null;
				return ((GroupedDroplinkField)item.Fields[field.InnerField.Name]).TargetItem;
			}
		}
	}
}
