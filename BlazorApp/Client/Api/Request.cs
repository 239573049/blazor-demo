using Microsoft.AspNetCore.Components;

namespace BlazorApp.Client.Api
{
    public class Request
    {
        public static Uri? BaseAddress;
        private static HttpClient? http;
        protected static HttpClient? Http{ get {
                if (http != null) return http;
                http = new HttpClient
                {
                    BaseAddress = BaseAddress
                };
                http.Timeout = new TimeSpan(60000);
                return http;
            } }
    }
}
