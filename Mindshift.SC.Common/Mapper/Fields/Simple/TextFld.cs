using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Mindshift.SC.Common.Mapper.Fields.Base;
using System.Web;

namespace Mindshift.SC.Common.Mapper.Fields.Simple
{
    public class TextFld : BaseField<TextField>
    {
        public TextFld(Item item, TextField field)
            : base(item, field)
		{

		}

        /*
        public static implicit operator string(TextFld textField)
		{
			return ((textField != null) ? textField.Text : null);
		}
        */
		public HtmlString Text
		{
			get { return Rendered; }
		}
	}
}
