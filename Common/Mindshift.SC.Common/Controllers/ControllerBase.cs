using Sitecore.Mvc.Controllers;
using Sitecore.Services.Infrastructure.Web.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Mindshift.SC.Common.Controllers {
	public class ControllerBase : ServicesApiController {

		[HttpOptions]
		[Route("*")]
		public HttpResponseMessage Options() {
			return new HttpResponseMessage { StatusCode = HttpStatusCode.OK };
		}
	}
}