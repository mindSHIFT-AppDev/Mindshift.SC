using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Data.Fields;
using System.Threading;
using Sitecore.Configuration;
using Sitecore.Publishing;
using Sitecore.StringExtensions;
using Sitecore.Diagnostics.PerformanceCounters;
using Sitecore.Data.Managers;

namespace Mindshift.SC.AutoPublish {
	public partial class Publish_Schedule { // note: underscore because that's what it is in TemplateClasses.cs
		static Database master = Factory.GetDatabase("master");

		public delegate void Worker();
		Thread oThread;

		public void Start() {
			LogInfo("Thread Started");
			oThread = new Thread(new ThreadStart(Init));
			oThread.Start();
		}

		private void LogError(string message) {
			LogHelper.Error(message, this.SCItem.Paths.FullPath);
		}
		
		private void LogError(string message, Exception ex) {
			message += string.Format("\n\t### Message: {0}, StackTrace: {1}", ex.Message, ex.StackTrace); // TODO: do we need InnerException?
			LogHelper.Error(message, this.SCItem.Paths.FullPath, ex);
		}
		private void LogInfo(string message) {
			LogHelper.Info(message, this.SCItem.Paths.FullPath);
		}

		private void LogInfo(string message, Database database, Sitecore.Globalization.Language language, PublishStatistics statistics = null) {
			message += string.Format("\n\t### Database: {0}, Language: {1}", database.Name, language.Name);

			if (statistics != null) {
				message += string.Format("\n\t### Statistics: {0} Created, {1} Deleted, {2} Skipped, {3} Updated",
					statistics.Created,
					statistics.Deleted,
					statistics.Skipped,
					statistics.Updated
				);
			}
			LogHelper.Info(message, this.SCItem.Paths.FullPath);
		}

		public void Init() {
			try {
				while (true) {
					LogInfo("Initializing Thread");
					// LogMsgToFile("### Schedular loG Started -Init process ");
					var now = DateTime.Now.ToUniversalTime();

					// sleep if the start date didn't happen yet.
					if (!Schedule_Start_Date.DateTime.Equals(DateTime.MinValue) && Schedule_Start_Date.DateTime > now) {
						LogInfo("Schedule paused due to future start date.");
						Thread.Sleep(Schedule_Start_Date.DateTime - now); // wait until this schedule is valid.
						continue; // then start over
					}

					// stop if the end date happened.
					if (!Schedule_End_Date.DateTime.Equals(DateTime.MinValue) && Schedule_End_Date.DateTime < now) {
						LogInfo("Schedule stopped due to past end date.");
						Stop();
						return;
					}

					var languages = (Languages.Ids.Count > 0) ? Languages.RawItems.Select(i => Sitecore.Globalization.Language.Parse(i["Iso"])) : LanguageManager.GetLanguages(master); // TODO: is Iso correct for this?

					if (Publishing_Targets.Ids.Count == 0) {
						LogError("At least one Publishing Target required.");
						Stop();
						return;
					}

					if (Root_Path.RawItem == null) {
						LogError("Root item blank or not found.");
						Stop();
						return;
					}


					// TODO: parse what kind of schedule this is!
					// Q: can we somehow use the TDS items?
					var frequency = Frequency.RawItem.Name;

					DateTime timeToRun;

					switch (frequency) {
						case "Daily":
							if (Time_Of_The_Day.DateTime.Equals(DateTime.MinValue)) {
								LogError("Daily Frequency requires Time of the Day to be populated.");
								Stop();
								return;
							}
							timeToRun = new DateTime(now.Year, now.Month, now.Day, Time_Of_The_Day.DateTime.Hour, Time_Of_The_Day.DateTime.Minute, 0, 0); // run when the time comes.
							if (timeToRun < now) timeToRun = timeToRun.AddDays(1); // It's too late, you missed it. Come back tomorrow.
							break;
						case "Hourly":
							timeToRun = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0, 0).AddHours(1); // run at the top of the next hour.
							break;
						case "Monthly":
							if (Time_Of_The_Day.DateTime.Equals(DateTime.MinValue) || Day_Of_The_Month.Integer == 0) {
								LogError("Daily Frequency requires Time of the Day and Day of the Month to be populated.");
								Stop();
								return;
							}
							timeToRun = new DateTime(now.Year, now.Month, Day_Of_The_Month.Integer, Time_Of_The_Day.DateTime.Hour, Time_Of_The_Day.DateTime.Minute, 0, 0);
							if (timeToRun < now) timeToRun = timeToRun.AddMonths(1); // It already past this month, wait until next month.
							break;
						case "Once":
							if (Specific_Date_And_Time.DateTime.Equals(DateTime.MinValue)) {
								LogError("Daily Frequency requires a Specific Date and Time to be populated.");
								Stop();
								return;
							}
							timeToRun = Specific_Date_And_Time.DateTime; // run when the time comes.
							if (timeToRun < now) {
								LogInfo("Schedule stopped due to a Frequency of Once and being in the past.");
								// TODO: should we disable it now?
								Stop();
							}
							break;
						case "Weekly":
							if (Time_Of_The_Day.DateTime.Equals(DateTime.MinValue) || Day_Of_The_Week.RawItem == null) {
								LogError("Daily Frequency requires Time of the Day and Day of the Week to be populated.");
								Stop();
								return;
							}
							timeToRun = new DateTime(now.Year, now.Month, now.Day, Time_Of_The_Day.DateTime.Hour, Time_Of_The_Day.DateTime.Minute, 0, 0);
							var dayOfTheWeek = (DayOfWeek)int.Parse(Day_Of_The_Week.RawItem["Value"]);
							timeToRun = timeToRun.AddDays(dayOfTheWeek - now.DayOfWeek); // add the amount of days that will bring you to where you need to be (within this week).
							if (timeToRun < now) timeToRun = timeToRun.AddDays(7); // next week, most likely because we went back in the previous step!
							break;
						default: // will NEVER happen...
							LogError("Daily Frequency required.");
							//timeToRun = DateTime.MinValue;
							Stop();
							return;
					}


					// time to next run is todays date + run time
					//				DateTime nextTimeToRun = new DateTime(now.Year, now.Month, now.Day, timeToRun.Hour, timeToRun.Minute, timeToRun.Second, 0);

					// how long we have to wait
					var timeToWait = timeToRun - now;
					LogInfo("Waiting: " + timeToWait);
					Thread.Sleep(timeToWait); // hush little thready...

					var targetDatabases = Publishing_Targets.RawItems.Select(d => Factory.GetDatabase(d["Target database"]));

					foreach (var targetDatabase in targetDatabases) {
						foreach (var language in languages) {
							LogInfo("Publish started", targetDatabase, language);
							var options = new PublishOptions(master, targetDatabase, PublishMode.Smart, language, DateTime.Now) { Deep = Include_Children };
							Item rootItem = master.GetItem(Root_Path.RawItem.ID, language); // get the item in the perticular langage (is this necessary?)
							if (rootItem == null) {
								// TODO: how do we bring this check back?
								LogInfo(string.Format("Root item not found in current language: {0} for target database: {1}.", language, targetDatabase.Name));
								return;
							}
							options.RootItem = rootItem;
							Publisher publisher = new Publisher(options);
							bool willBeQueued = publisher.WillBeQueued;

							// TODO: lock publishing while this is happening... possibly check queue?
							LogInfo("Executing publisher started");
							var result = publisher.PublishWithResult();
							LogInfo("Publish complete", targetDatabase, language, result.Statistics);
							//TaskCounters.Publishings.Increment(); // TODO: what is this and why is it?
						}
					}
				}


			} catch (ThreadAbortException) {
				// LogInfo("Thread aborted."); // do nothing here, I most likely wanted to abort the thread. Thought about logging anyway, but it would be to verbose
			} catch(Exception ex) {
				LogError("Publishing Thread had a fatal error.", ex);
			}
		}


		public void Stop() {
			if (oThread != null) oThread.Abort();
			LogInfo("Thread Stopped");
		}

		//public void LogMsgToFile(string msg) {
		//	System.IO.StreamWriter sw = System.IO.File.AppendText(@"D:\SVN\Mindshift\branches\dsingh\20160815-AutoPublish\DI.SC\DI.SC.Web\Data\logs\MyLogFile.txt");
		//	try {
		//		string logLine = System.String.Format(
		//				"{0:G}: {1}.", System.DateTime.Now, msg);
		//		sw.WriteLine(logLine);
		//	}
		//	finally {
		//		sw.Close();
		//	}
		//}

	}
}