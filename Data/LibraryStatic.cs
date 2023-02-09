using aspnetmvc_blog.Models;
using aspnetmvc_blog.Models.Views;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace aspnetmvc_blog.Data
{
    public static class LibraryStatic
    {
        public static string ComputeSha512(string randomString)
        {
            var message = Encoding.UTF8.GetBytes(randomString);
            using var alg = SHA512.Create();
            string hex = "";
            var hashValue = alg.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += string.Format("{0:x2}", x);
            }
            return hex;
        }

        public static string ToMD5Hash(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0)
            {
                return "";
            }
            using (var md5 = MD5.Create())
            {
                return string.Join("", md5.ComputeHash(bytes).Select(x => x.ToString("X2")));
            }
        }

        public static Guid? GetIdentitySid(ClaimsPrincipal principal)
        {
            var userIdentity = principal.Identity;
            if (userIdentity == null)
            {
                return null;
            }
            var claimIdentity = (ClaimsIdentity)userIdentity;
            if (claimIdentity == null)
            {
                return null;
            }
            var claim = claimIdentity.FindFirst(ClaimTypes.Sid);
            if (claim == null)
            {
                return null;
            }
            return new Guid(claim.Value);
        }

    }
}
