using Mindshift.SC.Common.Mapper.Fields.Base;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace Mindshift.SC.Common.Mapper.Fields.Simple
{
	public partial class CheckBoxFld : BaseField<CheckboxField>
	{
        public CheckBoxFld(Item item, CheckboxField field)
			: base(item, field)
		{
		}

        public static implicit operator bool(CheckBoxFld dateField)
		{
			return dateField.Checked;
		}

		public bool Checked
		{
			get
			{
				if (field == null) return false;
                if (field.InnerField.Name =="" || item.Fields[field.InnerField.Name] == null) return false;
				return ((CheckboxField)item.Fields[field.InnerField.Name]).Checked;
			}
		}
	}
}
