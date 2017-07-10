using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Data.Items;
using Sitecore.Mvc.Pipelines.MvcEvents.Exception;
using Sitecore.Security.Accounts;
using System.Globalization;
using System.Web;
using System.Net;

namespace Mindshift.SC.Common.Mapper.Helpers
{
	public static class ErrorHandlersHelper
	{
		public static string GetExceptionInfo(Item contextItem, string language, User currentUser, HttpContext context)
		{
			string extraInfo = Environment.NewLine;
			extraInfo += String.Format("|  Machine Name (Instance Name): {0}{1}", Environment.MachineName, Environment.NewLine);

            try
            {
                if (currentUser != null)
                {
                    extraInfo += String.Format(CultureInfo.CurrentCulture, "|  Current Sitecore User: {0}{1}", currentUser.DisplayName, Environment.NewLine);
                    extraInfo += "|  Current User's Roles: ";
                    foreach (var role in currentUser.Roles)
                    {
                        extraInfo += role.DisplayName + ",";
                    }
                    extraInfo = extraInfo.Substring(0, extraInfo.Length - 1); // remove last comma
                    extraInfo = String.Format(CultureInfo.CurrentCulture, "{0}{1}", extraInfo, Environment.NewLine);
                }
                else
                {
                    extraInfo += String.Format("|  Current User is NULL{0}", Environment.NewLine);
                }
            }
            catch
            {
                extraInfo += String.Format("Could not get User info", Environment.NewLine);
            }

			if (contextItem != null)
			{
				extraInfo += String.Format(CultureInfo.CurrentCulture, "|  Current Sitecore Item: {0} [{1}]{2}", contextItem.Name, contextItem.ID.ToString(), Environment.NewLine);
			}
			else
			{
				extraInfo += String.Format("|  Context Item is NULL{0}", Environment.NewLine);
			}

			if (language != null)
			{
				extraInfo += String.Format(CultureInfo.CurrentCulture, "|  Current Language: {0}{1}", language, Environment.NewLine);
			}
			else
			{
				extraInfo += String.Format("|  Language is NULL{0}", Environment.NewLine);
			}

			if (context != null)
			{
				string ipAddress = context.Request.ServerVariables["HTTP_CLIENT_IP"];
				if (String.IsNullOrEmpty(ipAddress))
				{
					ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
				}
				if (String.IsNullOrEmpty(ipAddress))
				{
					ipAddress = context.Request.ServerVariables["REMOTE_ADDR"];
				}
				if (!String.IsNullOrEmpty(ipAddress))
				{
					extraInfo += String.Format(CultureInfo.CurrentCulture, "|  HTTP IP Address: {0}{1}", ipAddress, Environment.NewLine);
				}

                extraInfo += "|  HTTP FORWARDED IP Address: ";

                if(!String.IsNullOrEmpty(context.Request.ServerVariables["X-FORWARDED-FOR"]))
                {
                    extraInfo += context.Request.ServerVariables["X-FORWARDED-FOR"];
                }

                extraInfo += Environment.NewLine;

				string referrer = context.Request.ServerVariables["HTTP_REFERER"];
				extraInfo += String.Format(CultureInfo.CurrentCulture, "|  HTTP Referrer: {0}{1}", referrer, Environment.NewLine);

				string currentUrl = context.Request.Url.AbsoluteUri;
				extraInfo += String.Format(CultureInfo.CurrentCulture, "|  HTTP URL: {0}{1}", currentUrl, Environment.NewLine);

				string method = context.Request.HttpMethod;
				extraInfo += String.Format(CultureInfo.CurrentCulture, "|  HTTP Method: {0}{1}", method, Environment.NewLine);

				if (method.ToLower() == "post")
				{
					if (context.Request.Form.Count > 0)
					{
						extraInfo += String.Format(CultureInfo.CurrentCulture, "|  FORM Variables {0}", Environment.NewLine);

						foreach (string key in context.Request.Form)
						{
							var value = context.Request.Form[key];
							extraInfo += "|   " + key + " : " + value.ToString() + System.Environment.NewLine;
						}
					}

				}

				HttpCookieCollection cookies = context.Request.Cookies;
				string userAgent = context.Request.ServerVariables["HTTP_USER_AGENT"];
				extraInfo += String.Format(CultureInfo.CurrentCulture, "|  HTTP User Agent: {0}{1}", userAgent, Environment.NewLine);

				string queryString = context.Request.ServerVariables["QUERY_STRING"];
				if (!String.IsNullOrEmpty(queryString))
				{
					extraInfo += String.Format(CultureInfo.CurrentCulture, "|  HTTP Query String: {0}{1}", queryString, Environment.NewLine);
				}

				if (cookies != null && cookies.Count > 0)
				{
					for (int c = 0; c < cookies.Count; c++)
					{
						HttpCookie cookie = cookies.Get(c);
						extraInfo += String.Format(CultureInfo.CurrentCulture, "|  HTTP Cookie: {0} = {1}{2}", cookie.Name, cookie.Value, Environment.NewLine);
					}
				}
			}
			else
			{
				extraInfo += String.Format("|  Context is NULL{0}", Environment.NewLine);
			}

			string errMsg = "-------------------------------------------";
			errMsg += extraInfo;
			errMsg += "-------------------------------------------";

			return errMsg;
		}

	}
}
