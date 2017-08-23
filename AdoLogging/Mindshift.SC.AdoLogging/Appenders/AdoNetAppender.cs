using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using log4net.Appender;
using Sitecore.Configuration;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Sitecore.Security.Accounts;
using Sitecore.Web;

namespace Mindshift.SC.AdoLogging.Appenders {
	public class AdoNetAppender : log4net.Appender.ADONetAppender {
		public string connectionStringName { get; set; }

		public override void ActivateOptions() {
			this.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
			base.ActivateOptions();
		}

		private static Language tryParseLanguage(string name) {
			Sitecore.Diagnostics.Assert.ArgumentNotNull((object)name, "name");
			Language result;
			if (tryParseLanguage(name, out result))
				return result;
			throw new ArgumentException(string.Format("Could not parse the language '{0}'. Note that a custom language name must be on the form: isoLanguageCode-isoRegionCode-customName. The language codes are two-letter ISO 639-1, and the regions codes are are two-letter ISO 3166. Also, customName must not exceed 8 characters in length. Valid example: en-US-East. For the full list of requirements, see: http://msdn2.microsoft.com/en-US/library/system.globalization.cultureandregioninfobuilder.cultureandregioninfobuilder.aspx", (object)name));
		}

		private static bool tryParseLanguage(string name, out Language result) {
			Sitecore.Diagnostics.Assert.ArgumentNotNull((object)name, "name");
			result = (Language)null;
			if (name.Equals("__Standard Values", StringComparison.OrdinalIgnoreCase) || name.Equals("__language", StringComparison.OrdinalIgnoreCase) || !LanguageManager.IsValidLanguageName(name))
				return false;
			if (LanguageManager.LanguageRegistered(name) || LanguageManager.RegisterLanguage(name)) {
				return true;
			}
			return true;
		}



		private Language getLanguage() {
			Language language1 = Sitecore.Context.Items["sc_Language"] as Language;
			if (language1 != null)
				return language1;
			string name = string.Empty;
			Sitecore.Sites.SiteContext site = Sitecore.Context.Site;
			if (site != null) {
				name = WebUtil.GetCookieValue(site.GetCookieKey("lang"), site.Language);
				Assert.IsNotNull((object)name, "languageName");
			}
			if (name.Length == 0)
				name = Settings.DefaultLanguage;

			try {
				Language language2 = tryParseLanguage(name);
				if (Sitecore.Configuration.State.Sites.IsSiteResolved)
					Sitecore.Context.Items["sc_Language"] = (object)language2;
				return language2;
			} catch (Exception) { }
			return null;
		}

		protected override void Append(log4net.spi.LoggingEvent loggingEvent) {
			log4net.helpers.PropertiesCollection col = loggingEvent.Properties;
			if (!object.ReferenceEquals(Environment.MachineName, null)) {
				col["machine"] = Environment.MachineName;
			}

			HttpContext context = null;
			User currentUser = null;
			Item contextItem = null;
			Language language = null;

			try { context = HttpContext.Current; } catch { }
			try { currentUser = Sitecore.Context.User; } catch { }
			try { contextItem = Sitecore.Context.Item; } catch { }

			if (loggingEvent.LoggerName != "System.RuntimeType") {
				try { language = Sitecore.Context.Language; } catch { }
				if (language != null) {
					col["Language"] = language;
				}
			}

			try {
				if (currentUser != null) {
					col["CurrentUser"] = currentUser.DisplayName;

					var roles = "";
					foreach (var role in currentUser.Roles) {
						roles += role.DisplayName + ",";
					}
					roles = roles.Substring(0, roles.Length - 1); // remove last comma
					col["Roles"] = roles;
				}
			} catch { }

			if (contextItem != null) {
				col["SitecoreItemID"] = contextItem.ID.ToString();
				col["SitecoreItemName"] = contextItem.Name;
			}

			if (context != null) {
				try {
					string ipAddress = context.Request.ServerVariables["HTTP_CLIENT_IP"];
					if (String.IsNullOrEmpty(ipAddress)) {
						ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
					}
					if (String.IsNullOrEmpty(ipAddress)) {
						ipAddress = context.Request.ServerVariables["REMOTE_ADDR"];
					}
					if (!String.IsNullOrEmpty(ipAddress)) {
						col["IpAddress"] = ipAddress;
					}

					if (!String.IsNullOrEmpty(context.Request.ServerVariables["X-FORWARDED-FOR"])) {
						col["ForwardedIpAddress"] = context.Request.ServerVariables["X-FORWARDED-FOR"];
					}

					string referrer = context.Request.ServerVariables["HTTP_REFERER"];
					col["HttpReferrer"] = referrer;

					string currentUrl = context.Request.Url.AbsoluteUri;
					col["HttpUrl"] = currentUrl;

					string method = context.Request.HttpMethod;
					col["HttpMethod"] = method;

					if (method.ToLower() == "post") {
						var formVariables = "";
						if (context.Request.Form.Count > 0) {
							foreach (string key in context.Request.Form) {
								var value = context.Request.Form[key];
								formVariables += "|   " + key + " : " + value.ToString() + System.Environment.NewLine;
							}
						}
						col["FormVariables"] = formVariables;
					}

					HttpCookieCollection cookies = context.Request.Cookies;
					string userAgent = context.Request.ServerVariables["HTTP_USER_AGENT"];
					col["HttpUserAgent"] = userAgent;

					string queryString = context.Request.ServerVariables["QUERY_STRING"];
					if (!String.IsNullOrEmpty(queryString)) {
						col["HttpQueryString"] = queryString;
					}

					if (cookies != null && cookies.Count > 0) {
						var strCookies = "";
						for (int c = 0; c < cookies.Count; c++) {
							HttpCookie cookie = cookies.Get(c);
							strCookies += String.Format(CultureInfo.CurrentCulture, "|  HTTP Cookie: {0} = {1}{2}", cookie.Name, cookie.Value, Environment.NewLine);
						}
						col["HttpCookies"] = strCookies;
					}
				} catch { }
			}

			base.Append(loggingEvent);
		}
	}
}
