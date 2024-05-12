using System.Reflection;
using System.Web;
using Astrum.Infrastructure.Models;
using Astrum.SharedLib.Application.Contracts.Infrastructure;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Astrum.Infrastructure
{
    public static class HttpHelper
    {
        public static Uri CreateUri<T>(T model, string baseUrl)
        {
            var builder = new UriBuilder(baseUrl);
            var query = HttpUtility.ParseQueryString(builder.Query);

            var props = model.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);

            foreach (var prop in props)
            {
                var value = prop.GetValue(model, null);
                var isList = (prop.PropertyType.IsGenericType &&
                              (prop.PropertyType.GetGenericTypeDefinition() == typeof(List<>)));

                var name = prop.GetCustomAttribute<JsonPropertyAttribute>()?.PropertyName ?? prop.Name;
                if (value != null)
                {
                    if (isList)
                    {
                        try
                        {
                            var val = string.Join(",", value as List<int>); //TODO Костыль
                            query[name] = val;
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                    else
                    {
                        query[name] = value.ToString();
                    }
                }
            }

            builder.Query = query.ToString();

            return builder.Uri;
        }


        #region Get

        /// <summary>
        ///     Стандартный get-запрос
        /// </summary>
        /// <param name="url">Url</param>
        /// <param name="httpClientLogService">Logger</param>
        public static async Task<HttpResponseMessageExtended> GetAsync(string url,
            IHttpClientLogger httpClientLogService = null)
        {
            var builder = new UriBuilder(url);
            return await GetAsync(builder.Uri, httpClientLogService);
        }

        /// <summary>
        ///     Стандартный get-запрос
        /// </summary>
        /// <param name="url">Url</param>
        /// <param name="headersExt">Заголовки</param>
        /// <param name="httpClientLogService">Logger</param>
        public static async Task<HttpResponseMessageExtended> GetAsync(string url, Tuple<string, string> headersExt,
            IHttpClientLogger httpClientLogService = null)
        {
            var builder = new UriBuilder(url);
            return await GetAsync(builder.Uri, headersExt: headersExt, httpClientLogService: httpClientLogService);
        }

        /// <summary>
        ///     Параметризованный get запрос с преобразованием модели в параметры
        /// </summary>
        /// <typeparam name="T">class</typeparam>
        /// <param name="url">Url</param>
        /// <param name="paramObject">Объект для преобразования в параметры</param>
        /// <param name="headersExt">Заголовки</param>
        /// <param name="httpClientLogService">Logger</param>
        public static async Task<HttpResponseMessageExtended> GetAsync<T>(string url, T paramObject
            , Tuple<string, string> headersExt = null, IHttpClientLogger httpClientLogService = null) where T : class
        {
            var uri = CreateUri<T>(paramObject, url);
            return await GetAsync(uri, httpClientLogService, headersExt);
        }

        private static async Task<HttpResponseMessageExtended> GetAsync(Uri uri, IHttpClientLogger httpClientLogService,
            Tuple<string, string> headersExt = null)
        {
            HttpResponseMessageExtended result = null;

            try
            {
                using (var httpClient = new HttpClient())
                {
                    if (headersExt != null)
                    {
                        httpClient.DefaultRequestHeaders.Add(headersExt.Item1, headersExt.Item2);
                    }

                    var response = await httpClient.GetAsync(uri);

                    if (httpClientLogService != null)
                        await httpClientLogService.Log(response);

                    result = new HttpResponseMessageExtended(response);
                }
            }
            catch (Exception ex)
            {
                result = new HttpResponseMessageExtended(ex);
            }

            return result;
        }

        #endregion

        #region Post

        /// <summary>
        ///     Стандартный post-запрос
        /// </summary>
        /// <typeparam name="T">class</typeparam>
        /// <param name="url">Url</param>
        /// <param name="jsonObject">Модель для тела запроса</param>
        /// <param name="headersExt">Заголовки</param>
        /// <param name="contentType">Тип контента</param>
        /// <param name="httpClientLogService">Logger</param>
        public static async Task<HttpResponseMessageExtended> PostAsJsonAsync<T>(string url, T jsonObject
            , Tuple<string, string> headersExt = null, string contentType = "application/json",
            IHttpClientLogger httpClientLogService = null) where T : class
        {
            HttpResponseMessageExtended result = null;

            try
            {
                using (var httpClient = new HttpClient())
                {
                    if (headersExt != null)
                    {
                        httpClient.DefaultRequestHeaders.Add(headersExt.Item1, headersExt.Item2);
                    }

                    var response = await httpClient.PostAsJsonAsyncHandmade<T>(url, jsonObject, contentType);

                    if (httpClientLogService != null)
                        await httpClientLogService.Log(response);

                    result = new HttpResponseMessageExtended(response);
                }
            }
            catch (Exception ex)
            {
                result = new HttpResponseMessageExtended(ex);
            }

            return result;
        }

        /// <summary>
        ///     Post-запрос c преобразованием модели в параметры
        /// </summary>
        /// <typeparam name="T">class</typeparam>
        /// <param name="url">Url</param>
        /// <param name="paramObject">Модель для преобразования в параметры</param>
        /// <param name="headersExt">Заголовки</param>
        /// <param name="httpClientLogService">Logger</param>
        public static async Task<HttpResponseMessageExtended> PostAsJsonAsQueryAsync<T>(string url, T paramObject,
            Tuple<string, string> headersExt = null, IHttpClientLogger httpClientLogService = null) where T : class
        {
            var uri = CreateUri<T>(paramObject, url);

            HttpResponseMessageExtended result = null;

            try
            {
                using (var httpClient = new HttpClient())
                {
                    if (headersExt != null)
                    {
                        httpClient.DefaultRequestHeaders.Add(headersExt.Item1, headersExt.Item2);
                    }

                    var response = await httpClient.PostAsync(uri, null);

                    if (httpClientLogService != null)
                        await httpClientLogService.Log(response);

                    result = new HttpResponseMessageExtended(response);
                }
            }
            catch (Exception ex)
            {
                result = new HttpResponseMessageExtended(ex);
            }

            return result;
        }

        /// <summary>
        ///     FormUrlEncodedContent Post-запрос
        /// </summary>
        /// <param name="url">Url</param>
        /// <param name="values">Словарь данных для запаковки в content</param>
        /// <param name="headersExt">Заголовки</param>
        /// <param name="httpClientLogService">Logger</param>
        public static async Task<HttpResponseMessageExtended> PostAsFormUrlEncodedContentAsync(string url
            , IDictionary<string, string> values, Tuple<string, string> headersExt = null,
            IHttpClientLogger httpClientLogService = null)
        {
            HttpResponseMessageExtended result = null;

            try
            {
                using (var httpClient = new HttpClient())
                {
                    if (headersExt != null)
                    {
                        httpClient.DefaultRequestHeaders.Add(headersExt.Item1, headersExt.Item2);
                    }

                    var formContent = new FormUrlEncodedContent(values);
                    var response = await httpClient.PostAsync(url, formContent);

                    if (httpClientLogService != null)
                        await httpClientLogService.Log(response);

                    result = new HttpResponseMessageExtended(response);
                }
            }
            catch (Exception ex)
            {
                result = new HttpResponseMessageExtended(ex);
            }

            return result;
        }

        public static async Task<HttpResponseMessageExtended> PostFilesAsync(string url, List<IFormFile> files,
            string formFieldName = "file")
        {
            HttpResponseMessageExtended result = null;
            var multiContent = new MultipartFormDataContent();

            try
            {
                using var httpClient = new HttpClient();
                foreach (var file in files.Where(file => file.Length > 0))
                {
                    byte[] data;
                    using (var br = new BinaryReader(file.OpenReadStream()))
                    {
                        data = br.ReadBytes((int)file.OpenReadStream().Length);
                    }

                    var bytes = new ByteArrayContent(data);
                    multiContent.Add(bytes, $"{formFieldName}[]", file.FileName);
                }

                var response = await httpClient.PostAsync(url, multiContent);
                result = new HttpResponseMessageExtended(response);
            }
            catch (Exception ex)
            {
                result = new HttpResponseMessageExtended(ex);
            }

            return result;
        }

        #endregion
    }
}