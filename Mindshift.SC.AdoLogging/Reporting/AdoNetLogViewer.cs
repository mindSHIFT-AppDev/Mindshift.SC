using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASR.Interface;
using Sitecore.Diagnostics;
using Mindshift.SC.AdoLogging.Models;

namespace Mindshift.SC.AdoLogging.Reporting {
	public class AdoNetLogViewer : ASR.Interface.BaseViewer {
		private readonly string ICON_WARN = "Applications/32x32/warning.png";
		private readonly string ICON_ERROR = "Applications/32x32/delete.png";
		private readonly string ICON_INFO = "Applications/32x32/information2.png";
		private readonly string ICON_AUDIT = "Applications/32x32/scroll_view.png";



		// TODO: what columns for me?
		public override string[] AvailableColumns {
			get { return new string[] { "Id", "Level", "Date", "Message", "User", "SitecoreItemId" }; }
		}

		public override void Display(DisplayElement dElement) {
			Debug.ArgumentNotNull(dElement, "element");

			var logElement = dElement.Element as LogItem;

			if (logElement == null) return;

			dElement.Icon = GetIcon(logElement);

			foreach (var column in Columns) {
				switch (column.Name) {
					case "id":
						dElement.AddColumn(column.Header, logElement.Id.ToString());
						break;
					case "level":
						dElement.AddColumn(column.Header, logElement.Level.ToString());
						break;
					case "date":
						dElement.AddColumn(column.Header, logElement.Date.ToString("yyyy-MM-dd HH:mm:ss"));
						break;
					case "message":
						dElement.AddColumn(column.Header, logElement.Message.Substring(0, Math.Min(logElement.Message.Length, 200)));
						break;
					case "user":
						dElement.AddColumn(column.Header, logElement.CurrentUser);
						break;
					case "sitecoreitemid":
						dElement.AddColumn(column.Header, logElement.SitecoreItemId);
						break;
				}
			}

			dElement.Value = logElement.Id.ToString();
		}

		private string GetIcon(LogItem logElement) {
			switch (logElement.Level) {
				case "AUDIT":
					return ICON_AUDIT;
				case "WARN":
					return ICON_WARN;
				case "INFO":
					return ICON_INFO;
				case "ERROR":
					return ICON_ERROR;
			}
			return string.Empty;
		}
	}
}
