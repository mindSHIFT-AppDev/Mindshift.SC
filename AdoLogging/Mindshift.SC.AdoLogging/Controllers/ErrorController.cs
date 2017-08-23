using Sitecore.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Mindshift.SC.AdoLogging.Controllers {
	public class ErrorController : SitecoreController {


		public ActionResult GenerateError() {
			Response.Clear();

			try {
				throw new Exception("Test Logging Exception");
			} catch( Exception ex) {
				Sitecore.Diagnostics.Log.Error("Error for log testing", ex, this);
			}

			Response.Write("<div>Error Test</div>");

			return null; // it will never get here!
		}
	}
}
