using Mindshift.SC.Common.Mapper.Base;
using Mindshift.SC.Common.Mapper.Fields.Base;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.ContentSearch.Utilities;

namespace Mindshift.SC.Common.Mapper.Fields.Link
{
	public partial class LookupFld<T> : LookupFld where T : BaseModel, new()
	{
		public LookupFld(Item item, LookupField field)
			: base(item, field)
		{
		}

		public LookupFld(string paramValue)
			: base(null, null)
		{
			_paramValue = paramValue;
		}

		public static implicit operator Item(LookupFld<T> lookupField)
		{
			return lookupField.RawItem;
		}

		private string _paramValue = null;

		public new Item RawItem
		{
			get
			{

				if (_paramValue != null && !_paramValue.Contains("|"))
				{
					Item item = Sitecore.Context.Database.GetItem(new ID(_paramValue));
					return item;

				}
				else
				{
					return base.RawItem;
				}
			}
		}

		public T LinkedItem
		{
			get
			{
				T obj = null;
				if (_paramValue != null)
				{
					obj = new T();
					if (!_paramValue.Contains("|") && _paramValue.IsGuid())
					{
						Item item = Sitecore.Context.Database.GetItem(new ID(_paramValue));
						obj.SetModel(item);
					}
				}
				else if (field != null && field.TargetItem != null)
				{
					obj = new T();
					obj.SetModel(field.TargetItem);
				}

				return obj;
			}
		}

	}
}
