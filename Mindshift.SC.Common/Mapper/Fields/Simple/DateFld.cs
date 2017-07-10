using System;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Mindshift.SC.Common.Mapper.Fields.Base;

namespace Mindshift.SC.Common.Mapper.Fields.Simple
{
	public partial class DateFld : BaseField<DateField>
	{
        public DateFld(Item item, DateField field)
			: base(item, field)
		{
		}

		public static implicit operator DateTime(DateFld dateField)
		{
			return dateField.DateTime;
		}

		public DateTime DateTime
		{
			get
			{
				if (field == null) return DateTime.MinValue;
				if (item.Fields[field.InnerField.Name] == null) return DateTime.MinValue;
				return ((DateField)item.Fields[field.InnerField.Name]).DateTime;
			}
		}
	}
}
