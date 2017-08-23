using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Shell.Framework.Commands;
using Mindshift.SC.Common;

namespace Mindshift.SC.AdoLogging {
	public class AdoNetLogDetailsCommand : OpenDialogCommand {
		public override void Execute(CommandContext context) {
			OpenDialog("dynamicplaceholders", new Dictionary<string, string> { { "id", ((Sitecore.Collections.StringList)context.CustomData).FirstOrDefault() } });

			//var id = ((Sitecore.Collections.StringList)context.CustomData).FirstOrDefault();
			//Sitecore.Context.ClientPage.ClientResponse.ShowModalDialog(string.Format("/sitecore/shell/Applications/Content Manager/Dialogs/Log Entry/dialog.html?id={0}/#/logdetails/", id), "1000", "600", "Select Version", false, "1000", "600", true);
		}
	}
}
