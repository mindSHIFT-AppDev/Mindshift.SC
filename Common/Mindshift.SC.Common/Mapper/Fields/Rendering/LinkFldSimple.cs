using Mindshift.SC.Common.Mapper.Fields.Base;
using Sitecore.Data;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mindshift.SC.Common.Mapper.Fields.Rendering
{
    public class LinkFld : BaseRenderingField
    {
        public LinkFld(string paramValue)
            : base(paramValue)
        {

        }

        public Item RawItem
        {
            get
            {
               
                if (!ParamValue.Contains("|"))
                {
                    Item item = Sitecore.Context.Database.GetItem(new ID(ParamValue));
                    return item;

                }

                return null;
            }
        }
    }
}
