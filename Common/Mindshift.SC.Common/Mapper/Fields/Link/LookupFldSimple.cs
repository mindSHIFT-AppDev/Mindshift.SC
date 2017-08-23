using Mindshift.SC.Common.Mapper.Fields.Base;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mindshift.SC.Common.Mapper.Fields.Link
{
    public partial class LookupFld : BaseField<LookupField>
    {
        private string _paramValue = null;

        public LookupFld(Item item, LookupField field)
            : base(item, field)
        {
        }

        public LookupFld(string paramValue)
            : base(null, null)
        {
            _paramValue = paramValue;
        }

        public static implicit operator Item(LookupFld lookupField)
        {
            return lookupField.RawItem;
        }

        

        public Item RawItem
        {
            get
            {
                if (field == null) return null;
                return field.TargetItem;
            }
        }
    }
}
