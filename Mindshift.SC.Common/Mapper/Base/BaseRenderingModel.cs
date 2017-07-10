using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mindshift.SC.Common.Mapper.Base
{
    public abstract class BaseRenderingModel
    {
        protected NameValueCollection Parameters;

        public void SetParameters(NameValueCollection parameters)
        {
            Parameters = parameters;
        }

    }
}
