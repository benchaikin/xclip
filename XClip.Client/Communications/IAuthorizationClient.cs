using System;

namespace XClip.Client.Communications
{
    public interface IAuthorizationClient
    {
        bool AuthenticateUser(string user, string password, out string token);
    }
}
