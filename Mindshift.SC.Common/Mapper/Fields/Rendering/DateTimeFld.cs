using Mindshift.SC.Common.Mapper.Fields.Base;
using Sitecore;
using Sitecore.Data.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mindshift.SC.Common.Mapper.Fields.Rendering
{
    public class DateTimeFld : BaseRenderingField
    {
        public DateTimeFld(string paramValue) : base(paramValue)
        {

        }

        public DateTime DateTime
        {
            get
            {
                if (ParamValue == null) return DateTime.MinValue;
                if (ParamValue == "") return DateTime.MinValue;
                if (ParamValue == "00010101T000000Z") return DateTime.MinValue;

                DateTime convertedDateTime = DateUtil.IsoDateToDateTime(ParamValue, DateTime.MinValue);
                return convertedDateTime;
            }
        } 
    }
}
