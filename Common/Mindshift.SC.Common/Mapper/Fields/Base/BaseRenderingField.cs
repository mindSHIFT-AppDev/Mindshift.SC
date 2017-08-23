using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mindshift.SC.Common.Mapper.Fields.Base
{
    public abstract class BaseRenderingField
    {
        protected string ParamValue;

        public BaseRenderingField(string paramValue)
        {
            ParamValue = paramValue;
        }

        
    }
}
