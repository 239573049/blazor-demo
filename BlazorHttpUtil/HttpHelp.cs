using System.Net.Http.Json;

namespace BlazorHttpUtil
{
    public interface IHttpHelp
    {
        Task<T?> GetAsync<T>(string url);
        Task<T?> PostAsync<T>(string url, object data);
        Task<T?> DeleteAsync<T>(string url);
        Task<T?> PutAsync<T>(string url, object data);


    }
    public class HttpHelp:IHttpHelp
    {
        public readonly HttpClient _http;
        public HttpHelp(HttpClient http)
        {
            _http = http;
        }
        /// <summary>
        /// 发起Get请求（异步）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<T?> GetAsync<T>(string url)
        {
            return await _http.GetFromJsonAsync<T>(url);
        }
        /// <summary>
        /// 发起post请求（异步）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<T?> PostAsync<T>(string url,object data)
        {
            var message = await _http.PostAsJsonAsync(url,data);
            return await message.Content.ReadFromJsonAsync<T>();
        }
        /// <summary>
        /// 发起Delete请求（异步）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<T?> DeleteAsync<T>(string url)
        {
            var message = await _http.DeleteAsync(url);
            return await message.Content.ReadFromJsonAsync<T>();
        }
        /// <summary>
        /// 发起Put请求（异步）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<T?> PutAsync<T>(string url,object data)
        {
            var message = await _http.PutAsJsonAsync(url, data);
            return await message.Content.ReadFromJsonAsync<T>();
        }
    }
}