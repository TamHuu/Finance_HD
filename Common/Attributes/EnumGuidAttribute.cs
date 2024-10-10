using Finance_HD.Helpers;
using Microsoft.IdentityModel.Tokens;

namespace Finance_HD.Common.Attributes
{
    class EnumGuidAttribute : Attribute
    {
        public Guid Guid;

        public EnumGuidAttribute(string guid)
        {
            Guid = new Guid(guid);
        }
        public EnumGuidAttribute()
        {
            Guid = CommonGuids.defaultUID;
        }
    }
}
