using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Pipelines;
using Sitecore.Diagnostics;
using Sitecore.Events;

namespace Mindshift.SC.AutoPublish {
	public class CallAutoPublish {
		private static readonly object Locker = new object();
		static bool intialized = false;
		static Dictionary<string, Publish_Schedule> publishSchedules;
		static Database master = Sitecore.Configuration.Factory.GetDatabase("master");

		public virtual void Process(PipelineArgs args) {
			lock (Locker) {
				if (!intialized) { // to prevent double initialization.
					LogHelper.Info("Intializing Schedules");
					UpdateSchedule();
					intialized = true;
				}
			}
		}

		public void OnItemSaved(object sender, EventArgs args) {


			var eventArgs = args as SitecoreEventArgs;
			Assert.IsNotNull(eventArgs, "eventArgs");
			//Sitecore.Data.Items.Item item = eventArgs.Parameters[0] as Sitecore.Data.Items.Item;
			Item item = Event.ExtractParameter(args, 0) as Item;
			Assert.IsNotNull(item, "item");

			// when any of the schedules are updated, let's re-create the thread.
			if (item.TemplateName == "Publish Schedule") {
				LogHelper.Info("Re-intializing Schedule due to schedule modification");
				UpdateSchedule(item);
			}




			//sendEmail(item, itemChanges);

		}

		private void UpdateSchedule(Item scheduleItem = null) {
			LogHelper.Info("Schedule Update Started");
			lock (Locker) { // local everything while we do this.
				if (scheduleItem == null) {
					LogHelper.Info("Loading/reloading All Schedules");
					// stop all shedules if they're running.
					if (publishSchedules != null) {
						publishSchedules.ToList().ForEach(p => p.Value.Stop());
					}
					var publishSchedulesFolder = master.GetItem("/sitecore/system/Modules/Mindshift SC/Auto Publish/Publish Schedules");
					// note: checking enabled before conversion
					publishSchedules = publishSchedulesFolder.Axes.GetDescendants().Where(i => i.TemplateName == "Publish Schedule" && i["Enabled"] == "1").Select(i => new Publish_Schedule(i)).ToDictionary(s => s.SCItem.ID.ToString(), s => s);

					publishSchedules.ToList().ForEach(p => p.Value.Start());

					// TODO: possibly wait here? Shouldn't have to since it's static...

				} else { // we just updated one schedule, so just update that one shedule.
					var scheduleId = scheduleItem.ID.ToString();
					LogHelper.Info("Reloading Shedule: " + scheduleItem.Name + "(id:" + scheduleId + ")");
					if (publishSchedules.ContainsKey(scheduleId)) {
						publishSchedules[scheduleId].Stop();
						publishSchedules[scheduleId] = new Publish_Schedule(scheduleItem);
						publishSchedules[scheduleId].Start();
					}
				}
				LogHelper.Info("Schedule Update Ended");
			}
		}
	}
}