using System;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Mindshift.SC.Common.Mapper.Fields.Base;
using Mindshift.SC.Common.HideDependent.Fields;

namespace Mindshift.SC.Common.Mapper.Fields.Simple
{
	public partial class TimeFld : BaseField<TextField>
	{
        public TimeFld(Item item, TextField field)
			: base(item, field)
		{
		}

		public static implicit operator DateTime(TimeFld timeField)
		{
			return timeField.DateTime;
		}

		public DateTime DateTime
		{
			get
			{
				if (field == null) return DateTime.MinValue;
				if (item.Fields[field.InnerField.Name] == null) return DateTime.MinValue;
				if (string.IsNullOrWhiteSpace(item[field.InnerField.Name])) return DateTime.MinValue;
				var ret = DateTime.MinValue;
				DateTime.TryParse(item[field.InnerField.Name], out ret);
				if (!ret.Equals(DateTime.MinValue)) {
					ret = ret.ToUniversalTime(); // if it has a value, it's not in GMT.
				}
				return ret;
			}
		}
	}
}
