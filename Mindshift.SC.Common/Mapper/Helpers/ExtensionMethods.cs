using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Sitecore.Mvc.Common;
using Sitecore.Mvc.Helpers;
using Sitecore;
using Sitecore.Data.Templates;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace Mindshift.SC.Common.Mapper.Helpers
{
    /*
    public static class ExtensionMethods
    {
        public static bool IsGuid(this String input)
        {
            try
            {
                new Guid(input);
                return true;
            }
            catch (ArgumentNullException) { }
            catch (FormatException) { }
            catch (OverflowException) { }
            return false;
        }

    }
    */
    public static class SitecoreHelper
    {
        public static HtmlString DynamicPlaceholder(this Sitecore.Mvc.Helpers.SitecoreHelper helper, string placeholderKey)
        {
            var currentRenderingId = RenderingContext.Current.Rendering.UniqueId;

            //string dynamicPlaceHolderName = GetDynamicKey(placeholderKey);
            string dynamicPlaceHolderName = string.Format("{0}~{1}", placeholderKey, currentRenderingId.ToString().Replace("-", ""));
            //return helper.Placeholder(string.Format("{0}_{1}", dynamicKey, currentRenderingId));
            return helper.Placeholder(dynamicPlaceHolderName);
        }

       
        public static EditFrameRendering BeginEditFrame<T>(this HtmlHelper<T> helper, string dataSource, string buttons, string title, string tooltip, string cssClass, object parameters)
        {
           
            EditFrameRendering frame = new EditFrameRendering(helper.ViewContext.Writer, dataSource, buttons, title, tooltip, cssClass, parameters);
            return frame;
        }


 
        public static bool IsDerived([NotNull] this Template template, [NotNull] ID templateId)
        {
            return template.ID == templateId || template.GetBaseTemplates().Any(baseTemplate => IsDerived(baseTemplate, templateId));
        }

        public static bool IsLayoutField([NotNull] this Field field)
        {
            return field.ID == FieldIDs.LayoutField || field.ID == FieldIDs.FinalLayoutField;
        }

        public static bool HasField([NotNull] this Item item, ID id)
        {
            if (item.Fields[id] == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }


}
