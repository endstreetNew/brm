using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace Sassa.Brm.Common.Helpers
{
    public static class Security
    {
        public static List<string> GetRoles(this WindowsIdentity user)
        {
            List<string> result = new List<string>();
            var groups = user.Claims.Where(q => q.Type == ClaimTypes.GroupSid).Select(q => q.Value);
            foreach (var role in groups)
            {
                var group = new System.Security.Principal.SecurityIdentifier(role).Translate(typeof(System.Security.Principal.NTAccount)).ToString();
                if (!group.Contains("SASSA")) continue;

                result.Add(group.Substring(6));
            }
            return result;
        }
        public static string GetADEmail(this string username)
        {
            try
            {
                var di = new DirectoryEntry($"LDAP://DC=SASSA,DC=local");
                string[] loadProps = new string[] { "name", "mail" };
                DirectorySearcher ds = new DirectorySearcher(di, $"sAMAccountName={username}", loadProps);
                SearchResult sr = ds.FindOne();
                DirectoryEntry entry = sr.GetDirectoryEntry();
                return (string)entry.Properties["mail"].Value;
            }
            catch
            {
                //throw new Exception("Could not retrieve AD Email"); can't throw here because of bulk operations
                return null;
            }
        }
    }
}
