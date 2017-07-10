using Mindshift.SC.Common.Mapper.Fields.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mindshift.SC.Common.Mapper.Fields.Rendering
{
    public class IntFld : BaseRenderingField
    {
        public IntFld(string paramValue)
            : base(paramValue)
        {

        }

        public int Number
        {
            get
            {
                if (ParamValue == null) return int.MinValue;

                if (ParamValue.Trim() == "") return int.MinValue;

                int intValue;
                if (Int32.TryParse(ParamValue, out intValue))
                {
                    return intValue;
                }

                return int.MinValue;
            }
        }
    }
}
