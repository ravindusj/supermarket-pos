using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace supermarket_pos
{
    public static class UserSession
    {
        public static string Username { get; private set; }
        public static string StaffName { get; private set; }
        public static string Role { get; private set; }
        public static bool IsLoggedIn { get; private set; }

        public static void SetSession(string username, string staffName, string role)
        {
            Username = username;
            StaffName = staffName;
            Role = role;
            IsLoggedIn = true;
        }

        public static void ClearSession()
        {
            Username = null;
            StaffName = null;
            Role = null;
            IsLoggedIn = false;
        }
    }

}
