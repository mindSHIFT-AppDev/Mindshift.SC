using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.GetPlaceholderRenderings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Mindshift.SC.DynamicPlaceholders.Pipelines {
	public class GetDynamicKeyAllowedRenderings : GetAllowedRenderings {
		//text that ends in a GUID
		//private const string DYNAMIC_KEY_REGEX = @"(.+)_[\d\w]{8}\-([\d\w]{4}\-){3}[\d\w]{12}";
		private const string DYNAMIC_KEY_REGEX = @"(.+)~[\d\w]{32}$";

		public new void Process(GetPlaceholderRenderingsArgs args) {
			Assert.IsNotNull(args, "args");

			string placeholderKey = args.PlaceholderKey;
			Regex regex = new Regex(DYNAMIC_KEY_REGEX);
			Match match = regex.Match(placeholderKey);
			if (match.Success && match.Groups.Count > 0) {
				placeholderKey = match.Groups[1].Value;
			} else {
				return;
			}
			// Same as Sitecore.Pipelines.GetPlaceholderRenderings.GetAllowedRenderings but with fake placeholderKey
			Item placeholderItem = null;
			if (ID.IsNullOrEmpty(args.DeviceId)) {
				placeholderItem = Client.Page.GetPlaceholderItem(placeholderKey, args.ContentDatabase,
																												 args.LayoutDefinition);
			} else {
				using (new DeviceSwitcher(args.DeviceId, args.ContentDatabase)) {
					placeholderItem = Client.Page.GetPlaceholderItem(placeholderKey, args.ContentDatabase,
																													 args.LayoutDefinition);
				}
			}
			List<Item> collection = null;
			if (placeholderItem != null) {
				bool flag;
				args.HasPlaceholderSettings = true;
				collection = this.GetRenderings(placeholderItem, out flag);
				if (flag) {
					args.CustomData["allowedControlsSpecified"] = true;
					args.Options.ShowTree = false;
				}
			}
			if (collection != null) {
				if (args.PlaceholderRenderings == null) {
					args.PlaceholderRenderings = new List<Item>();
				}
				args.PlaceholderRenderings.AddRange(collection);
			}
		}
	}
}
