using System;

namespace WMS_WEBAPI.Models
{
    public class MobilePageGroup : BaseEntity
    {

        public string GroupName { get; set; } = string.Empty;

        public string GroupCode { get; set; } = string.Empty;

        public virtual ICollection<MobileUserGroupMatch> UserGroupMatches { get; set; } = new List<MobileUserGroupMatch>();
    }
}
