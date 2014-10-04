using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using XClip.Client.Properties;

namespace XClip.Client.Communications
{
    public class AuthorizationClient
    {
        public string AuthenticateUser(string user, string password)
        {
            var request = WebRequest.Create(Settings.Default.ServerUrl + "/Account/ClientLogin") as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.CookieContainer = new CookieContainer();

            var authCredentials = "UserName=" + user + "&Password=" + password;
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(authCredentials);
            request.ContentLength = bytes.Length;
            using (var requestStream = request.GetRequestStream())
            {
                requestStream.Write(bytes, 0, bytes.Length);
            }

            using (var response = request.GetResponse() as HttpWebResponse)
            {
                var authCookie = response.Cookies[".AspNet.ApplicationCookie"];
                return authCookie.Value;
            }

            return null;
        }
    }
}
