using Mindshift.SC.Common.Mapper.Fields.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mindshift.SC.Common.Mapper.Fields.Rendering
{
    public class StringFld : BaseRenderingField
    {
        public StringFld(string paramValue) : base(paramValue)
        {

        }

        public string Text
        {
            get
            {
                return ParamValue;
            }
        }
    }
}
