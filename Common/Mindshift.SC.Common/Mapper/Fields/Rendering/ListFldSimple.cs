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
    public class ListFld : BaseRenderingField
    {
        protected List<Item> _list;

        public ListFld(string paramValue) : base(paramValue)
        {

        }

       
        public List<Item> RawItems
        {
            get
            {

                _list = new List<Item>();
                
                if(ParamValue.Contains("|"))
                {
                    string[] arr = ParamValue.Split('|');
                    foreach(string s in arr)
                    {
                        Item item = Sitecore.Context.Database.GetItem(new ID(ParamValue));
                        _list.Add(item);
                    }

                }
                
                return _list;

            }
        }
    }
}
