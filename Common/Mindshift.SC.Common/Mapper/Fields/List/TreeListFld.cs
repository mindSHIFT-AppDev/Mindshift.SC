using System.Collections.Generic;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Mindshift.SC.Common.Mapper.Fields.Base;
using Mindshift.SC.Common.Mapper.Base;

namespace Mindshift.SC.Common.Mapper.Fields.List
{
    public partial class TreeListFld<T> : MultiListFld<T> where T : BaseModel, new()
	{
        public TreeListFld(Item item, MultilistField field)
			: base(item, field)
		{
		}


        
        /*
        public static implicit operator List<Item>(TreeListFld<T> treelistField)
		{
			return treelistField.ListItems;
		}
        */

	}

    
}
