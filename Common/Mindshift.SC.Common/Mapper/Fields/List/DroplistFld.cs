using Mindshift.SC.Common.Mapper.Base;
using Mindshift.SC.Common.Mapper.Fields.Base;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace Mindshift.SC.Common.Mapper.Fields.List
{
    public partial class DroplistFld<T> : DropListFldSimple where T : BaseModel, new()
	{
        public DroplistFld(Item item, GroupedDroplistField field)
            : base(item, field)
		{
		}

        public T LinkedItem
        {
            get
            {

                T obj = new T();
                Item i = ((GroupedDroplistField)item.Fields[field.InnerField.Name]).InnerField.Item;
                if(item != null)
                {
                        obj.SetModel(i);
                       
                }

                return obj;

            }
        }

	}
}
