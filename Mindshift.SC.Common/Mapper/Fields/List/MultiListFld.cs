using Mindshift.SC.Common.Mapper.Base;
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
    public class MultiListFld<T> : MultiListFld where T : BaseModel, new()
    {
        protected List<T> _list;

        public MultiListFld(Item item, MultilistField field)
            : base(item, field)
        {

        }

        public List<T> ListItems
        {
            get
            {
               
                _list = new List<T>();
                Item[] items = ((MultilistField)item.Fields[field.InnerField.Name]).GetItems();
                foreach (Item i in items)
                {
                    if (i != null)
                    {
                        //object obj = Convert.ChangeType(i, typeof(T));
                        T obj = new T();
                        obj.SetModel(i);
                        _list.Add(obj);
                    }
                }

                return _list;

            }
        }

        /*
        public List<Item> RawItems
        {
            get
            {
                if (field == null) return new List<Item>();
                if (item.Fields[field.InnerField.Name] == null) return new List<Item>();
                return ((MultilistField)item.Fields[field.InnerField.Name]).GetItems().ToList();
            }
        }

        /// <summary>
        /// Returns the ID values of the field as a list of strings
        /// </summary>
        public List<string> Ids
        {
            get
            {
                if (field == null)
                {
                    return new List<string>();
                }

                if (item.Fields[field.InnerField.Name] == null)
                {
                    return new List<string>();
                }

                if (string.IsNullOrEmpty(Raw))
                {
                    return new List<string>();
                }

                List<string> itemIds = new List<string>();
                foreach (string id in Raw.Split('|'))
                {
                    if (string.IsNullOrEmpty(id))
                    {
                        continue;
                    }

                    itemIds.Add(id);
                }

                return itemIds;
            }
        }
         */
    }
}
