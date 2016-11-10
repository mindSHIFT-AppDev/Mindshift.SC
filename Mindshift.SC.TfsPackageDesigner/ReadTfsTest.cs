using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Framework.Common;
using Microsoft.TeamFoundation.Framework.Client;

namespace Mindshift.SC.TfsPackageDesigner {
	public class ReadTfsTest {

		public static void ReadTfs() {
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
				Console.WriteLine("Collection: " + teamProjectCollection.Name);

				// Get a catalog of team projects for the collection
				ReadOnlyCollection<CatalogNode> projectNodes = collectionNode.QueryChildren(
						new[] { CatalogResourceTypes.TeamProject },
						false, CatalogQueryOptions.None);

				// List the team projects in the collection
				foreach (CatalogNode projectNode in projectNodes) {
					Console.WriteLine(" Team Project: " + projectNode.Resource.DisplayName);
				}
			}


		}

	}
}
