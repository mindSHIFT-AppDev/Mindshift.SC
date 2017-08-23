using System;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Mindshift.SC.Common.Mapper.Fields.Base;

namespace Mindshift.SC.Common.Mapper.Fields.Simple
{
	public partial class IntegerFld : BaseField<TextField>
	{
        public IntegerFld(Item item, TextField field)
			: base(item, field)
		{
		}

        public static implicit operator int(IntegerFld intField)
		{
			return intField.Integer;
		}

		public int Integer
		{
			get
			{
				if (field == null) return int.MinValue;

				if (item.Fields[field.InnerField.Name] == null) return int.MinValue;

				int intValue;
				if (Int32.TryParse(item[field.InnerField.Name], out intValue))
				{
					return intValue;
				}

				return int.MinValue;
			}
		}
	}
}
