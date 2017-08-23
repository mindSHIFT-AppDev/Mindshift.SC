using Mindshift.SC.Common.Mapper.Fields.Base;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mindshift.SC.Common.Mapper.Fields.List
{
    public class DropListFldSimple: BaseField<GroupedDroplistField>
	{
        public DropListFldSimple(Item item, GroupedDroplistField field)
            : base(item, field)
		{
		}


	}
}
