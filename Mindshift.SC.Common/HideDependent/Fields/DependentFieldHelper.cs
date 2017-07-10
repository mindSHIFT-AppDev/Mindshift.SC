using System;
using System.Collections.Generic;
using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Shell.Applications.ContentEditor;

namespace Mindshift.SC.Common.HideDependent.Fields {
	public static class DependentFieldHelper {
		public static void SetControlProperties(IHideDependentField control) {
			if (String.IsNullOrEmpty(control.Source))
				return;

			var control2 = control as LookupEx;
			foreach (var subcontrol in control2.Controls) {
				(subcontrol as Sitecore.Web.UI.HtmlControls.Control).Attributes["test"] = "frank";
			}


			if (!String.IsNullOrEmpty(control.Source.HideByDefault()) || !String.IsNullOrEmpty(control.Source.ValuesToHide())) {
				control.Attributes["onchange"] = "HideDependentFields(this, true)";
				control.Attributes["data-hide-default"] = control.Source.HideByDefault();
				control.Attributes["data-hide-count"] = control.Source.HideCount().ToString();

				if (control is HideDependentCheckbox) {
					control.Class = "scContentControlCheckbox hide-dependent-fields";
				}
				else {
					control.Attributes["data-hide-values"] = control.Source.ValuesToHide();
					control.Class = "scContentControl hide-dependent-fields";

					var database = Sitecore.Data.Database.GetDatabase("master"); // TODO: how do I know if it's master or web? Context is core!

					var sourceValueList = database.GetItem(control.Source.DataSource()).GetChildren();
					var jsonList = new List<string>();
					foreach (Item sourceValue in sourceValueList) {
						if (!string.IsNullOrWhiteSpace(sourceValue["Fields To Hide"])) {
							var sourceFields = sourceValue["Fields To Hide"].Split("|".ToCharArray());
							var sourceFieldNames = new List<string>();
							foreach (var sourceField in sourceFields) {
								var sourceFieldItem = database.GetItem(sourceField);
								var sourceFieldName = (string.IsNullOrWhiteSpace(sourceFieldItem["Title"]) ? sourceFieldItem.Name : sourceFieldItem["Title"]);

								if (!string.IsNullOrWhiteSpace(sourceFieldItem["Short description"])) sourceFieldName += " - " + sourceFieldItem["Short description"]; // add help text!

								sourceFieldNames.Add(sourceFieldName);
							}

							var sourceGuid = sourceValue.ID.ToString().Replace("{", "").Replace("}", "");
							jsonList.Add("{ 'source': '" + sourceGuid + "', 'fields': ['" + string.Join("','", sourceFieldNames) + "'] }");
						}
					}
					control.Attributes["data-hide-fields"] = "[" + string.Join(",", jsonList) + "]";

				}
			}
		}

		private static string HideByDefault(this string Source) {
			return StringUtil.ExtractParameter("HideByDefault", Source).Trim();
		}

		private static int HideCount(this string Source) {
			var hideCount = StringUtil.ExtractParameter("HideCount", Source).Trim();
			return MainUtil.GetInt(hideCount, 0);
		}

		private static string ValuesToHide(this string Source) {
			return StringUtil.ExtractParameter("ValuesToHide", Source).Trim();
		}
		private static string DataSource(this string Source) {
			return StringUtil.ExtractParameter("DataSource", Source).Trim();
		}

	}
}