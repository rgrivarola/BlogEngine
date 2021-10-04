using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.Components;
using System.Text.Json;
using System.Text.Encodings;
using BlogEngine.Core;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace BlogEngine.GUI
{
    public class BackendService
    {
        private readonly IHttpClientFactory ClientFactory;
        private readonly NavigationManager NavigationManager;
        private readonly BlogEngineDBContext _Context;
        
        private readonly IHttpContextAccessor _httpContextAccessor;
        public BackendService(IHttpClientFactory ClientFactory, NavigationManager NavigationManager, BlogEngineDBContext Context, IHttpContextAccessor httpContextAccessor)
        {
            this.ClientFactory = ClientFactory;
            this.NavigationManager = NavigationManager;
            this._Context = Context;
            this._httpContextAccessor = httpContextAccessor;
            
        }

        //public async Task<T> GetFromURL<T>(string Url)
        //{
        //    T Return = Activator.CreateInstance<T>();

        //    if (!Url.ToLower().Trim().StartsWith("http"))
        //    {
        //        Url = NavigationManager.BaseUri + Url;
        //    }
            
        //    var request = new HttpRequestMessage(HttpMethod.Get,
        //        Url);
        //    request.Headers.Add("Accept", "application/json");
        //    request.Headers.Add("User-Agent", "BlogEngineWebApp");
        //    var client = ClientFactory.CreateClient();
            
            
        //    var response = await client.SendAsync(request);
        //    if (response.IsSuccessStatusCode)
        //    {
        //        var options = new JsonSerializerOptions
        //        {
        //            PropertyNameCaseInsensitive = true
        //        };

                
        //        using var responseStream = await response.Content.ReadAsStreamAsync();
        //            Return = await JsonSerializer.DeserializeAsync<T>(responseStream, options);
        //    }
        //    else
        //    {
        //        throw new Exception($"{response.StatusCode}: {await response.Content.ReadAsStringAsync()}");
        //    }
        //    return Return;
        //}

        public async Task PostURL<T>(string Url, T Body)
        {
            await PostPutDeleteURL(HttpMethod.Post, Url, Body);
        }
        public async Task PutURL<T>(string Url, T Body)
        {
            await PostPutDeleteURL(HttpMethod.Put, Url, Body);
        }

        public async Task PostPutDeleteURL<T>(HttpMethod HttpMethod, string Url, T Body)
        {
            
            await DummyAsync();
            
            if (!Url.ToLower().Trim().StartsWith("http"))
            {
                Url = NavigationManager.BaseUri + Url;
            }
            
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;
            request.Method = HttpMethod.Method.ToUpper();
            request.ContentType = "application/Json";

            if (Body != null)
            {
                string SerializedObject = JsonSerializer.Serialize<T>(Body);
                byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(SerializedObject);
                request.ContentLength = byteArray.Length;
                System.IO.Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
            }


                        
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"{response.StatusCode}: {response.StatusDescription}");
            }
            
        }

        public async Task DummyAsync()
        {

        }
        //private async Task PostPutDeleteURL<T>(HttpMethod HttpMethod, string Url, T Body)
        //{
        //    if (!Url.ToLower().Trim().StartsWith("http"))
        //    {
        //        Url = NavigationManager.BaseUri + Url;
        //    }

        //    var request = new HttpRequestMessage(HttpMethod,
        //        Url);
        //    request.Headers.Add("Accept", "application/json");
        //    request.Headers.Add("User-Agent", "BlogEngineWebApp");

        //    string SerializedObject = JsonSerializer.Serialize<T>(Body);

        //    request.Content = new StringContent(SerializedObject, encoding: System.Text.Encoding.UTF8, "application/json");

        //    var client = ClientFactory.CreateClient();
        //    var response = await client.SendAsync(request);
        //    if (!response.IsSuccessStatusCode)
        //    {
        //        throw new Exception($"{response.StatusCode}: {await response.Content.ReadAsStringAsync()}");
        //    }
            
        //}
        public void NavigateTo(string Url)
        {
            NavigationManager.NavigateTo(Url);
        }
        public User GetCurrentUser()
        {
            return GetCurrentClientUser(); //User.GetCurrentServerUser();
        }

        public async Task<T> GetFromURL<T>(string Url)
        {
            T Return = Activator.CreateInstance<T>();
            if (!Url.ToLower().Trim().StartsWith("http"))
            {
                Url = NavigationManager.BaseUri + Url;
            }
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"{response.StatusCode}: {response.StatusDescription}");
            }
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            using var responseStream = response.GetResponseStream();
                Return = await JsonSerializer.DeserializeAsync<T>(responseStream, options);
                        
            return Return;
        }


        public User GetCurrentClientUser()
        {
            var CurrentLogin = GetCurrentClientUserLogin();
            return _Context.Users.FirstOrDefault(u => u.Login.ToLower().Trim() == CurrentLogin.ToLower().Trim());

        }
        public string GetCurrentClientUserLogin()
        {


            if (_httpContextAccessor.HttpContext != null && _httpContextAccessor.HttpContext.User != null && _httpContextAccessor.HttpContext.User.Identity != null)
                return _httpContextAccessor.HttpContext.User.Identity.Name;
            else
                return "";
        }
        public bool IsUserLoggedIn()
        {
            return !String.IsNullOrWhiteSpace(GetCurrentClientUserLogin());
        }

        public void ValidateUserInRole(eRoles Role)
        {
            var CurrentUser = GetCurrentUser();
            if (CurrentUser == null)
            {
                throw new Exception($"User {GetCurrentClientUserLogin()} doesn´t exists");
            }
            else if (!CurrentUser.HasRole(Role))
                throw new Exception($"User {GetCurrentClientUserLogin()} doesn´t have {Role.ToString()} role, necessary to perform this operation");
        }
                        

    }
}
