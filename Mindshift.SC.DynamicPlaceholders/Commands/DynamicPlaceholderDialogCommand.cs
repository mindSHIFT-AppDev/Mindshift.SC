using Mindshift.SC.Common;
using Sitecore.Shell.Framework.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mindshift.SC.DynamicPlaceholders.Commands {
	public class DynamicPlaceholderDialogCommand : OpenDialogCommand {
		public override void Execute(CommandContext context) {

			OpenDialog("dynamicplaceholders", new Dictionary<string, string> { { "id", context.Items[0].ID.ToString() }, { "database", context.Items[0].Database.Name } });
			//context.Items[0].ID.ToString();

			// TODO: change path to "sitecore modules"
			//Sitecore.Context.ClientPage.ClientResponse.ShowModalDialog(string.Format("/sitecore/shell/Applications/Content Manager/Dialogs/Dynamic Placeholder/dynamic.html?id={0}&database={1}/#/layout/", context.Items[0].ID.ToString(), context.Items[0].Database.Name), "1000", "600", "Edit Dynamic Layouts", false, "1000", "600", true);

		}
	}
}

