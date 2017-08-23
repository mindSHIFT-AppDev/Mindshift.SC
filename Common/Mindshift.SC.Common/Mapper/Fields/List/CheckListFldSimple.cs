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
    public partial class CheckListFld : MultiListFld
    {

        public CheckListFld(Item item, MultilistField field)
            : base(item, field)
        {

        }

        public static implicit operator List<Item>(CheckListFld checklistField)
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
