using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Pipelines;
using Sitecore.Diagnostics;
using Sitecore.Events;
using Sitecore.Data.Fields;

namespace Mindshift.SC.Common.SaveHooks {
	public class ItemDisabled {
		private static readonly object Locker = new object();
		static bool intialized = false;

		private static readonly HashSet<string> dicItems = new HashSet<string>();

		public void OnItemSaved(object sender, EventArgs args) {


			var eventArgs = args as SitecoreEventArgs;
			Assert.IsNotNull(eventArgs, "eventArgs");
			//Sitecore.Data.Items.Item item = eventArgs.Parameters[0] as Sitecore.Data.Items.Item;
			Item item = Event.ExtractParameter(args, 0) as Item;
			Assert.IsNotNull(item, "item");
			var id = item.ID.ToString();
			if (dicItems.Contains(id)) return;

			lock (dicItems) {
				dicItems.Add(id);
			}

			// when any of the schedules are updated, let's re-create the thread.
			if (item.Fields["Enabled"] != null && FieldTypeManager.GetField(item.Fields["Enabled"]) is CheckboxField) { // if it has an enabled checkbox field and it's not "1"
				var map = item["__Style"]
					.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
					.Select(x => x.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries))
					.ToDictionary(p => p[0], p => p[1]);

				if (item["Enabled"] != "1") {

					if (map.ContainsKey("text-decoration")) {
						map["text-decoration"] = "line-through";
					} else {
						map.Add("text-decoration", "line-through");
					}

				} else {
					if (map.ContainsKey("text-decoration") && map["text-decoration"] == "line-through") {
						map.Remove("text-decoration");
					}
				}

				item.Editing.BeginEdit();
				using (new EditContext(item)) {
					item.Fields["__Style"].Value = string.Join(";", map.Select(x => x.Key + ":" + x.Value).ToArray());
				}
				item.Editing.EndEdit();

				// refresh the content tree to reflect this change.
				String refresh = String.Format("item:refreshchildren(id={0})", item.Parent.ID);
				Sitecore.Context.ClientPage.SendMessage(this, refresh);
			}

			lock (dicItems) {
				dicItems.Remove(id);
			}





			//sendEmail(item, itemChanges);


		}
	}
}