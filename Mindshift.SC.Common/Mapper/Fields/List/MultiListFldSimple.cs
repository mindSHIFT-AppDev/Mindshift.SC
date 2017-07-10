﻿using Mindshift.SC.Common.Mapper.Fields.Base;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Mindshift.SC.Common.Mapper.Fields.List
{
    public class MultiListFld : BaseField<MultilistField>
   {

        public MultiListFld(Item item, MultilistField field)
            : base(item, field)
        {
            
        }

        
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
    }
}
