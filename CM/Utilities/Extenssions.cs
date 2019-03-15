using System;
using System.IO;
using System.Net;
using System.Linq;

namespace CM.Utilities
{
    public static class Extenssions
    {
        /// <summary>
        /// Search if the given string is contained in any string parameter
        /// </summary>
        /// <param name="subStr">String to be searched in the parameters</param>
        /// <param name="str">Strings to search in</param>
        /// <returns>True id the substring was found in any parameter, False otherwise</returns>
        public static bool IsContainedIn(this string subStr, params string[] strs)
        {
            foreach (var str in strs)
            {
                if (str != null && str.IndexOf(subStr, StringComparison.OrdinalIgnoreCase) > -1)
                    return true;
            }
            return false;
        }

        public static byte[] GetBytes(this FileInfo info)
        {
            return File.ReadAllBytes(info.FullName);
        }

        public static byte[] GetBytes(this FileInfo info, string userName, string password)
        {
            using (new NetworkConnection(info.FullName, new NetworkCredential(userName, password)))
            {
                return info.GetBytes();
            }
        }

        public static bool IsAnyNullOrWhiteSpace(params string[] strs)
        {
            return strs.Any(s => string.IsNullOrWhiteSpace(s));
        }
    }
}
