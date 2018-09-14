using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;

namespace MVC.Controllers
{
    public static class ApiConsumer
    {
        private static HttpClient Client = new HttpClient();
        private static Uri UriTo(string relativePath) => new Uri(string.Format("http://localhost:55031/api/{0}", relativePath));

        public static T Get<T>(string relativePath) where T : class
        {
            T json = null;
            Client.GetAsync(UriTo(relativePath)).ContinueWith(response =>
              {
                  var result = response.Result;
                  if (result.StatusCode == HttpStatusCode.OK)
                  {
                      result.Content.ReadAsAsync<T>().ContinueWith(jsonRead => json = jsonRead.Result).Wait();
                  }
              }).Wait();
            return json;
        }

        public static HttpResponseMessage Post<T>(string relativePath, T body) where T : class
        {
            var post = Client.PostAsJsonAsync(UriTo(relativePath), body);
            post.Wait();
            return post.Result;
        }

        public static HttpResponseMessage Put<T>(string relativePath, T body) where T : class
        {
            var put = Client.PutAsJsonAsync(UriTo(relativePath), body);
            put.Wait();
            return put.Result;
        }
    }
}