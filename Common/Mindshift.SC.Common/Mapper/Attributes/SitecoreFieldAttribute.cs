using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Mindshift.SC.Common.Mapper.Attributes
{
    public class SitecoreFieldAttribute : Attribute
    {
        private string _fieldID;

        public SitecoreFieldAttribute(string fieldID)
        {
            _fieldID = fieldID;
        }

        public ID FieldID
        {
            get
            {
                return new ID(_fieldID);
            }
        }
    }
}
