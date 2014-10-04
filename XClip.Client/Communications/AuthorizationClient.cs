using System.Net;
using XClip.Client.Properties;

namespace XClip.Client.Communications
{
    public class AuthorizationClient : IAuthorizationClient
    {
        public static string AuthCookieName = ".AspNet.ApplicationCookie";

        public bool AuthenticateUser(string user, string password, out string token)
        {
            token = null;
            var request = WebRequest.Create(Settings.Default.ServerUrl + "/Account/ClientLogin") as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.CookieContainer = new CookieContainer();

            string authCredentials = string.Format("UserName={0}&Password={1}", user, password);
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(authCredentials);
            request.ContentLength = bytes.Length;
            using (var requestStream = request.GetRequestStream())
            {
                requestStream.Write(bytes, 0, bytes.Length);
            }
          
            using (var response = request.GetResponse() as HttpWebResponse)
            {
                var authCookie = response.Cookies[AuthCookieName];
                if (authCookie != null)
                {
                    token = authCookie.Value;
                }
            }

            return token != null;
        }
    }
}
