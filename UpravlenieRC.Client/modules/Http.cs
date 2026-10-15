using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace UpravlenieRC.Client.modules
{
    static internal class Http
    {
        public static HttpClient httpClient = new HttpClient();
        public static class Urls
        {
            private const string BaseUrl = "http://localhost:5152/api/";
            public const string AuthUrl = BaseUrl + "auth";
            public const string RegUrl = BaseUrl + "reg";

        }

    }
}
