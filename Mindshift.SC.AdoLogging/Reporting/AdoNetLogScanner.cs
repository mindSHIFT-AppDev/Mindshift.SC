using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Data.SqlServer;
using Mindshift.SC.AdoLogging.Models;

namespace Mindshift.SC.AdoLogging.Reporting {
	public class AdoNetLogScanner : ASR.Interface.BaseScanner {

		private SqlServerDataApi dataApi = new SqlServerDataApi(
			Sitecore.Configuration.Settings.GetConnectionString("log4net")
		);

		public string Level { get; set; }

		public string FromDate { get; set; }

		public string ToDate { get; set; }

		public override System.Collections.ICollection Scan() {
			string query = @"SELECT [ID],[Date],[Thread],[Level],[Logger],[Message],[Exception],[MachineName],[CurrentUser],[Roles],
				[SitecoreItemID],[SitecoreItemName],[Language],[IpAddress],[ForwardedIpAddress],[HttpReferrer],[HttpUrl],
				[HttpMethod],[FormVariables],[HttpUserAgent],[HttpQueryString],[HttpCookies]
				from dbo.[log]";


			string whereClause = "";
			if (!string.IsNullOrEmpty(Level)) {
				whereClause += string.Format(" and [Level] = '{0}'", Level);
			}

			if (!string.IsNullOrEmpty(FromDate)) {
				whereClause += string.Format(" and [Date] >= '{0} 00:00:00.000'", FromDate);
			}

			if (!string.IsNullOrEmpty(ToDate)) {
				whereClause += string.Format(" and [Date] <= '{0} 23:59:59.999'", ToDate);
			}

			if (whereClause.Length > 0) whereClause = " where " + whereClause.Substring(5, whereClause.Length - 5);

			Sitecore.Data.DataProviders.Sql.DataProviderReader reader = dataApi.CreateReader(query + whereClause + " order by [Date] desc"); //,

			List<LogItem> resultList = new List<LogItem>();
			while (reader.Read()) {
				var result = new LogItem(
					reader.InnerReader.GetInt64(0),
					reader.InnerReader.GetDateTime(1),
					reader.InnerReader.GetString(2),
					reader.InnerReader.GetString(3),
					reader.InnerReader.GetString(4),
					reader.InnerReader.GetString(5),
					reader.InnerReader.GetString(6),
					reader.InnerReader.GetString(7),
					reader.InnerReader.GetString(8),
					reader.InnerReader.GetString(9),
					reader.InnerReader.GetString(10),
					reader.InnerReader.GetString(11),
					reader.InnerReader.GetString(12),
					reader.InnerReader.GetString(13),
					reader.InnerReader.GetString(14),
					reader.InnerReader.GetString(15),
					reader.InnerReader.GetString(16),
					reader.InnerReader.GetString(17),
					reader.InnerReader.GetString(18),
					reader.InnerReader.GetString(19),
					reader.InnerReader.GetString(20),
					reader.InnerReader.GetString(21)
				);

				resultList.Add(result);
			}
			return resultList;


		}



	}
}
