using System;
using System.Collections.Generic;
using System.Text;

namespace CLF.Common.Http
{
   public static class AppCookieDefaults
    {
        public static string Prefix => ".CLF";
        public static string SessionCookie => ".Session";
        public static string AntiforgeryCookie => ".Antiforgery";

    }
}
