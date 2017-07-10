using Mindshift.SC.Common.Mapper.Base;
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
    public class ListFld<T> : BaseRenderingField where T : BaseModel, new()
    {
        protected List<T> _list;

        public ListFld(string paramValue) : base(paramValue)
        {

        }


        public List<T> ListItems
        {
            get
            {

                _list = new List<T>();
                
                if(ParamValue.Contains("|"))
                {
                    string[] arr = ParamValue.Split('|');
                    foreach(string s in arr)
                    {
                        Item item = Sitecore.Context.Database.GetItem(new ID(s));
                        T obj = new T();
                        obj.SetModel(item);
                        _list.Add(obj);
                    }

                }
                else if (!String.IsNullOrEmpty(ParamValue))
                {
                    Item item = Sitecore.Context.Database.GetItem(new ID(ParamValue));
                    T obj = new T();
                    obj.SetModel(item);
                    _list.Add(obj);
                }
                
                return _list;

            }
        }
    }
}
