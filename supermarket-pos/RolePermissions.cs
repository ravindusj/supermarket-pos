using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace supermarket_pos
{
    public static class RolePermissions
    {
        private static readonly Dictionary<string, List<string>> RolePermissionMap = new Dictionary<string, List<string>>
    {
        {
            "cashier", new List<string>
            {
                "billing"
            }
        },
        {
            "store manager", new List<string>
            {
                "billing",
                "inventory",
                "staff",
                "customers",
                "report"
            }
        },
        {
            "stocker", new List<string>
            {
                "inventory",
                "billing"
            }
        },
        {
            "accountant", new List<string>
            {
                "inventory",
                "report"
            }
        }
    };

        public static bool HasPermission(string role, string feature)
        {
            if (string.IsNullOrEmpty(role) || !RolePermissionMap.ContainsKey(role.ToLower()))
                return false;

            return RolePermissionMap[role.ToLower()].Contains(feature.ToLower());
        }
    }
}
