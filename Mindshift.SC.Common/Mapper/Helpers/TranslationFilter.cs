using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Mindshift.SC.Common.Mapper.Helpers
{
public class TranslationFilter : MemoryStream
{
    private Stream filter = null;

    public TranslationFilter(HttpResponseBase httpResponseBase)
    {
        filter = httpResponseBase.Filter;
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        var response = UTF8Encoding.UTF8.GetString(buffer);

        // remove all newlines
        response = response.Replace(System.Environment.NewLine, "");

        /* remove just first empty line
          if (response.Substring(0, 2) == "\r\n")
        {
            response = response.Substring(2, response.Length - 2);
        } */

        filter.Write(UTF8Encoding.UTF8.GetBytes(response), offset, UTF8Encoding.UTF8.GetByteCount(response));
    }
}

public class ResponseFilter : ActionFilterAttribute
{
    public ResponseFilter()
    {
    }

    public override void OnResultExecuted(ResultExecutedContext filterContext)
    {
        base.OnResultExecuted(filterContext);
				if (filterContext.HttpContext.Response.Filter == null) return;
        filterContext.HttpContext.Response.Filter = new TranslationFilter(filterContext.HttpContext.Response);
    }
}
}
