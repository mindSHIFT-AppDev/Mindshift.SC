using Mindshift.SC.Common.Mapper.Fields.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mindshift.SC.Common.Mapper.Fields.Rendering
{
    public class ChkboxFld : BaseRenderingField
    {
        public ChkboxFld(string paramValue) : base(paramValue)
        {

        }

        public bool Checked
        {
            get
            {
                if (ParamValue == "1")
                    return true;
                else
                    return false;
            }
        }
    }
}
