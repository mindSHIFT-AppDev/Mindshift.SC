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
    public partial class TreeListFld : MultiListFld
    {
        public TreeListFld(Item item, MultilistField field)
            : base(item, field)
        {
        }


    }
}
