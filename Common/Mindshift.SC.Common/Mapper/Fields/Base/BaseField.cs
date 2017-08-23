using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Web.UI.WebControls;
using System.Web;


namespace Mindshift.SC.Common.Mapper.Fields.Base
{
	public abstract class BaseField<T> where T : CustomField
	{
		protected T field;
		protected Item item;



		protected BaseField(Item item, T field)
		{
			this.field = field;
			this.item = item;
		}

		public string Raw
		{
			get
			{
                if (field == null)
                {
                    return string.Empty;
                }
				return field.Value;
			}
		}

		public HtmlString Rendered
		{
			get
			{
                if (field == null || field.InnerField == null || string.IsNullOrEmpty(field.InnerField.Name))
                {
                    return new HtmlString("");
                }
				return new HtmlString(FieldRenderer.Render(item, field.InnerField.Name));
			}
		}

		public T Field
		{
			get
			{
				return field;
			}
		}

		public HtmlString RenderWithParameters(string parameters)
		{
            if (item == null || field == null)
            {
                return new HtmlString("");
            }
            else
            {
                if (field.InnerField.Name != "")
                {
                    return new HtmlString(FieldRenderer.Render(item, field.InnerField.Name, parameters)); 
                }
                else
                {
                    return new HtmlString("");
                }
            }
		}

	}
}
