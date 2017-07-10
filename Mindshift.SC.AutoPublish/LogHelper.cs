using System;
using Sitecore.Diagnostics;
using Sitecore.Publishing;

namespace Mindshift.SC.AutoPublish {
	public class LogHelper {
		public static void Error(string message, string schedule = "Global") {
			Log.Error("### Mindshift Auto Publish ###\n\t### Schedule: " + schedule + "\n\t### Message: " + message, new object());
		}

		public static void Info(string message, string schedule = "Global") {
			Log.Info("### Mindshift Auto Publish ###\n\t### Schedule: " + schedule + "\n\t### Message: " + message, new object());
		}

		internal static void Error(string message, string schedule, Exception ex) {
			Log.Error("### Mindshift Auto Publish ###\n\t### Schedule: " + schedule + "\n\t### Message: " + message, ex, new object());
		}
	}
}
