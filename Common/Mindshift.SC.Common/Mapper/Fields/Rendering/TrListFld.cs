using Mindshift.SC.Common.Mapper.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mindshift.SC.Common.Mapper.Fields.Rendering
{
    public class TrListFld<T> : ListFld<T> where T : BaseModel, new()
    {
        public TrListFld(string paramValue)
            : base(paramValue)
        {

        }

    }
}
