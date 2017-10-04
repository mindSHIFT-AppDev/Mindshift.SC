using System;
using log4net;
using Sitecore.Diagnostics;
using Sitecore.Publishing;

namespace Mindshift.SC.AutoPublish {
	public class LogHelper {
		// TODO: Move to Common and ask for which logger we want.
		private static readonly ILog Logger = LogManager.GetLogger("MindshiftSCAutoPublishLogger"); // TODO: move somewhere in Common!

		public static void Error(string message, string schedule = "Global") {
			Logger.Error("### Mindshift Auto Publish ###\n\t### Schedule: " + schedule + "\n\t### Message: " + message);
		}

		public static void Info(string message, string schedule = "Global") {
			Logger.Info("### Mindshift Auto Publish ###\n\t### Schedule: " + schedule + "\n\t### Message: " + message);
		}
		public static void Debug(string message, string schedule = "Global") {
			Logger.Debug("### Mindshift Auto Publish ###\n\t### Schedule: " + schedule + "\n\t### Message: " + message);
		}

		internal static void Error(string message, string schedule, Exception ex) {
			Logger.Error("### Mindshift Auto Publish ###\n\t### Schedule: " + schedule + "\n\t### Message: " + message, ex);
		}
	}
}
