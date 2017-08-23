using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mindshift.SC.Common.Mapper.Base
{
    public class BaseModel
    {
        protected Item InnerItem;
        

        public void SetModel(Item item)
        {
            InnerItem = item;

        }

        public Item SCItem
        {
            get
            {
                return InnerItem;
            }
        }
    }
}
