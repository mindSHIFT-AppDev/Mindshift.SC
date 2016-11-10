using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASR.Reports.Logs;

namespace Mindshift.SC.AdoLogging.Models {
	public class LogItem {
		public long Id { get; private set; }
		public DateTime Date { get; private set; }
		public string Thread { get; private set; }
		public string Level { get; private set; }
		public string Logger { get; private set; }
		public string Message { get; protected set; }
		public string Exception { get; protected set; }
		public string MachineName { get; private set; }
		public string CurrentUser { get; private set; }
		public string Roles { get; private set; }
		public string SitecoreItemId { get; private set; }
		public string SitecoreItemName { get; private set; }
		public string Language { get; private set; }
		public string IpAddress { get; private set; }
		public string ForwardedIpAddress { get; private set; }
		public string HttpReferrer { get; private set; }
		public string HttpUrl { get; private set; }
		public string HttpMethod { get; private set; }
		public string FormVariables { get; private set; }
		public string HttpUserAgent { get; private set; }
		public string HttpQueryString { get; private set; }
		public string HttpCookies { get; private set; }




		public LogItem(long id, DateTime date, string thread, string level, string logger, string message, string exception, string machineName, string currentUser, string roles,
			string sitecoreItemId, string sitecoreItemName, string language, string ipAddress, string forwardedIpAddress, string httpReferrer, string httpUrl, string httpMethod,
			string formVariables, string httpUserAgent, string httpQueryString, string httpCookies) {
			Id = id;
			Date = date;
			Thread = thread;
			Level = level;
			Message = message;
			Exception = exception;
			Logger = logger;
			MachineName = machineName;
			CurrentUser = currentUser;
			Roles = roles;
			SitecoreItemId = sitecoreItemId;
			Language = language;
			IpAddress = ipAddress;
			ForwardedIpAddress = forwardedIpAddress;
			HttpReferrer = httpReferrer;
			HttpUrl = httpUrl;
			HttpMethod = httpMethod;
			FormVariables = formVariables;
			HttpUserAgent = httpUserAgent;
			HttpQueryString = httpQueryString;
			HttpCookies = httpCookies;
		}
	}
}
