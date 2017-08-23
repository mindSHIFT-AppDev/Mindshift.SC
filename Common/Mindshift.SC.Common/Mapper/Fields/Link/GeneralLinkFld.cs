using Mindshift.SC.Common.Mapper.Fields.Base;
using Mindshift.SC.Common.Mapper.Helpers;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using System.Web;

namespace Mindshift.SC.Common.Mapper.Fields.Link
{
	public partial class GeneralLinkFld : BaseField<LinkField>
	{
        public GeneralLinkFld(Item item, LinkField field)
			: base(item, field)
		{
		}

		public static implicit operator string(GeneralLinkFld generalLinkField)
		{
			return generalLinkField.Url;
		}

		public string Url
		{
			get
			{
                if (field == null || field.InnerField == null || string.IsNullOrEmpty(field.InnerField.Name))
                {
                    return null;
                }

                if (item.Fields[field.InnerField.Name] == null)
                {
                    return null;
                }

				return (LinkHelper.GetLinkFieldUrl(item.Fields[field.InnerField.Name]));
			}
		}

        //attribute is Text
        public string Description
        {
            get
            {
                return field.Text;
            }
        }

        //attribute is target
        public string Target
        {
            get
            {
                return field.Target;
            }
        }

        //attribute is title
        public string AlternateText
        {
            get
            {
                return field.Title;
            }
        }

        public HtmlString RenderAttributes()
        {
            string attributes = "";

            if (Description != "")
            {
                attributes += " text=\"" + Description + "\"";
            }

            if (Target != "")
            {
                attributes += " target=\"" + Target + "\"";
            }

            if (AlternateText != "")
            {
                attributes += " title=\"" + AlternateText + "\"";
            }

            return new HtmlString(attributes);
        }

        
	}
}
