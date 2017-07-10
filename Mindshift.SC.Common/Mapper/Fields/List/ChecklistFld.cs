using System.Collections.Generic;
using System.Linq;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Mindshift.SC.Common.Mapper.Fields.Base;
using Mindshift.SC.Common.Mapper.Base;

namespace Mindshift.SC.Common.Mapper.Fields.List
{
	public partial class CheckListFld<T> : MultiListFld<T> where T: BaseModel, new()
	{

        public CheckListFld(Item item, MultilistField field)
            : base(item, field)
		{

		}

        public static implicit operator List<Item>(CheckListFld<T> checklistField)
		{
			return checklistField.CheckedItems;
		}

		public List<Item> CheckedItems
		{
			get
			{
                return base.RawItems;
			}
		}
	}

    
}
