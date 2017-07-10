using Mindshift.SC.Common.Mapper.Base;
using Mindshift.SC.Common.Mapper.Fields.Base;
using Sitecore.Data;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mindshift.SC.Common.Mapper.Helpers;
using Sitecore.ContentSearch.Utilities;

namespace Mindshift.SC.Common.Mapper.Fields.Rendering
{
	[Obsolete("This is deprecated, please use LookFld instead.")]
    public class LinkFld<T> : BaseRenderingField where T : BaseModel, new()
    {
        public LinkFld(string paramValue)
            : base(paramValue)
        {

        }

        public T ListItem
        {
            get
            {
                T obj = null;
                if (ParamValue != null && !ParamValue.Contains("|"))
                {
                    if (ParamValue.IsGuid())
                    {
                        Item item = Sitecore.Context.Database.GetItem(new ID(ParamValue));
                        obj = new T();
                        obj.SetModel(item);
                    }
                }

                return obj;
            }
        }

        public Item RawItem
        {
            get
            {

                if (ParamValue != null && !ParamValue.Contains("|"))
                {
                    Item item = Sitecore.Context.Database.GetItem(new ID(ParamValue));
                    return item;

                }

                return null;
            }
        }

    }
}
