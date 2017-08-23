using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Sitecore.Data.SqlServer;
using Sitecore.Services.Infrastructure.Web.Http;
using Mindshift.SC.Common.Controllers;
using Mindshift.SC.AdoLogging.Models;

namespace Ricoh.SC.Mapper.Logging
{
	public class RicohLogEntryDetailController : ControllerBase
	{
		private SqlServerDataApi dataApi = new SqlServerDataApi(
			Sitecore.Configuration.Settings.GetConnectionString("log4net")
		);

		[HttpGet]
		public LogItem GetLogEntryDetail([FromUri]LogEntryDetailRequest request)
		{



			// Note: the CTE function will get all ItemIDs under a root parent.
			// - the main query returns the __Created field, since it will always exist if a version exists
			string query = @"SELECT [ID],[Date],[Thread],[Level],[Logger],[Message],[Exception],[MachineName],[CurrentUser],[Roles],
				[SitecoreItemID],[SitecoreItemName],[Language],[IpAddress],[ForwardedIpAddress],[HttpReferrer],[HttpUrl],
				[HttpMethod],[FormVariables],[HttpUserAgent],[HttpQueryString],[HttpCookies]
				from dbo.[log] where id=@id";

			object[] parameters = new object[] { "id", request.Id };


			Sitecore.Data.DataProviders.Sql.DataProviderReader reader = dataApi.CreateReader(query, parameters); //,

			while (reader.Read())
			{
				return new LogItem(
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
			}
			return null;
		}
	}
}
