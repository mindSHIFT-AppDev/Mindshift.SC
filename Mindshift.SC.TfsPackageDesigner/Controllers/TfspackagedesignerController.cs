using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Items;
using Sitecore.Links;
using System.Reflection;
using System.Xml;
using Sitecore.Layouts;
using System.Text;
using Sitecore.Data;
using System.Collections.ObjectModel;
using Sitecore.Data.Managers;
using System.Web.Http;
using System.Collections;
using Mindshift.SC.Common.Controllers;

using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Framework.Common;
using Microsoft.TeamFoundation.Framework.Client;


namespace Mindshift.SC.TfsPackageDesigner.Controllers {

	//[RoutePrefix("mindshiftAPI/{controller}")]
	[RoutePrefix("mindshiftAPI/TfsPackageDesigner")]
	public class TfspackagedesignerController : ControllerBase {

		[HttpGet]
		//mindshiftAPI/{controller}/{action}/{itemid}/{database}
		[Route("GetTfsTest")]
		//[ResponseFilter]
		public string GetTfsTest() { //[FromUri]DynamicGetRequest request
			StringBuilder sbRet = new StringBuilder();

			// Connect to Team Foundation Server
			//     Server is the name of the server that is running the application tier for Team Foundation.
			//     Port is the port that Team Foundation uses. The default port is 8080.
			//     VDir is the virtual path to the Team Foundation application. The default path is tfs.
			Uri tfsUri = new Uri("http://invi01-tfs2010:8080/tfs");

			TfsConfigurationServer configurationServer = TfsConfigurationServerFactory.GetConfigurationServer(tfsUri);

			// Get the catalog of team project collections
			ReadOnlyCollection<CatalogNode> collectionNodes = configurationServer.CatalogNode.QueryChildren(new[] { CatalogResourceTypes.ProjectCollection }, false, CatalogQueryOptions.None);

			// List the team project collections
			foreach (CatalogNode collectionNode in collectionNodes) {
				// Use the InstanceId property to get the team project collection
				Guid collectionId = new Guid(collectionNode.Resource.Properties["InstanceId"]);
				TfsTeamProjectCollection teamProjectCollection = configurationServer.GetTeamProjectCollection(collectionId);

				// Print the name of the team project collection
				sbRet.AppendLine("Collection: " + teamProjectCollection.Name);

				// Get a catalog of team projects for the collection
				ReadOnlyCollection<CatalogNode> projectNodes = collectionNode.QueryChildren(new[] { CatalogResourceTypes.TeamProject }, false, CatalogQueryOptions.None);
				// List the team projects in the collection
				foreach (CatalogNode projectNode in projectNodes) {
					sbRet.AppendLine(" Team Project: " + projectNode.Resource.DisplayName);
				}
			}
			return sbRet.ToString();

		}

		//[HttpPost]
		//[Route("mindshiftAPI/DynamicPlaceholders/SaveRenderings/{itemid}/{database}")]
		//public DynamicResponse SaveRenderings(DynamicSaveRequest request) {


	}
}