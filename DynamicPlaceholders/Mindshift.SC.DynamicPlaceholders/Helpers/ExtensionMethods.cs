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

namespace Mindshift.SC.DynamicPlaceholders.Helpers {
	public static class SitecoreHelper {
		public static HtmlString DynamicPlaceholder(this Sitecore.Mvc.Helpers.SitecoreHelper helper, string placeholderKey) {
			var currentRenderingId = RenderingContext.Current.Rendering.UniqueId;
			string dynamicPlaceHolderName = string.Format("{0}~{1}", placeholderKey, currentRenderingId.ToString().Replace("-", ""));
			return helper.Placeholder(dynamicPlaceHolderName);
		}

		public static bool IsDerived([NotNull] this Template template, [NotNull] ID templateId) {
			return template.ID == templateId || template.GetBaseTemplates().Any(baseTemplate => IsDerived(baseTemplate, templateId));
		}

	}


}
