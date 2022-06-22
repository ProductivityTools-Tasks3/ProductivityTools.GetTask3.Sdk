using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ProductivityTools.MasterConfiguration;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;


namespace ProductivityTools.GetTask3.Sdk
{
    public class GetTaskHttpClient
    {
        //static string URL = "http://apigettask3.productivitytools.tech:8040/api/";// Consts.EndpointAddress;
        //static string URL = "http://localhost:5513/api/";// Consts.EndpointAddress;


        private readonly string URL;
        private readonly Action<String> Log;
        string WebApiKey;

        public GetTaskHttpClient(string url, string webapikey, Action<string> log)
        {
            this.URL = url;
            this.Log = log;
            this.WebApiKey= webapikey; 
        }

        private string token;
        private string Token
        {
            get
            {
                Console.WriteLine("GetToken");
                if (string.IsNullOrEmpty(token))
                {
                    SetNewAccessToken();
                }
                else
                {
                    var tokenhanlder = new JwtSecurityTokenHandler();
                    var jwtSecurityToke = tokenhanlder.ReadJwtToken(token);
                    if (jwtSecurityToke.ValidTo < DateTime.UtcNow)
                    {
                        SetNewAccessToken();
                    }
                }
                return token;
            }
        }

        private async Task<string> GetCustomToken()
        {
             var HttpClient = new HttpClient();
            Uri url = new Uri($"{this.URL}CustomToken/Get");

            HttpClient.DefaultRequestHeaders.Accept.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await HttpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var resultAsString = await response.Content.ReadAsStringAsync();
                // T result = JsonConvert.DeserializeObject<T>(resultAsString);
                return resultAsString;
            }
            throw new Exception(response.ReasonPhrase);
        }
        async Task<string> GetIdToken(string custom_token)
        {
            var HttpClient = new HttpClient();
            Uri url = new Uri($"https://identitytoolkit.googleapis.com/v1/accounts:signInWithCustomToken?key={this.WebApiKey}");

            HttpClient.DefaultRequestHeaders.Accept.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            object obj = new { token = custom_token, returnSecureToken = true };
            var dataAsString = JsonConvert.SerializeObject(obj);
            var content = new StringContent(dataAsString, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await HttpClient.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    var resultAsString = await response.Content.ReadAsStringAsync();
                    TokenResponse result = JsonConvert.DeserializeObject<TokenResponse>(resultAsString);
                    return result.idToken;
                }
                throw new Exception(response.ReasonPhrase);
            }
            catch (Exception)
            {

                throw;
            }
         
        }

        private void SetNewAccessToken()
        {
            string customToken = GetCustomToken().Result;
            string idToken = GetIdToken(customToken).Result;
            this.token = idToken;
        }

        //private void SetNewAccessToken()
        //{
        //    Log("token is empty need to call identity server");
        //    var client = new System.Net.Http.HttpClient();

        //    var disco = client.GetDiscoveryDocumentAsync("https://identityserver.productivitytools.tech:8010/").Result;
        //    Log($"Discovery server{disco}");
        //    if (disco.IsError)
        //    {
        //        Log(disco.Error);
        //    }

        //    Log("GetTask3Cmdlet secret");
        //    Log(ClientSecret);

        //    var tokenResponse = client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
        //    {
        //        Address = disco.TokenEndpoint,

        //        ClientId = "GetTask3Cmdlet",
        //        ClientSecret = ClientSecret,
        //        Scope = "GetTask3.API"
        //    }).Result;

        //    Log("Token response pw:");
        //    Log(tokenResponse.AccessToken);
        //    Log(tokenResponse.Error);

        //    if (tokenResponse.IsError)
        //    {
        //        Log(tokenResponse.Error);
        //    }

        //    Log(tokenResponse.Json.ToString());
        //    token = tokenResponse.AccessToken;
        //}



        public async Task<T> Post2<T>(string controller, string action, object obj)
        {
            Log($"Performing Post under address {URL}");
             var client = new System.Net.Http.HttpClient(new LoggingHandler(new HttpClientHandler(), Log));
            client.BaseAddress = new Uri(URL + controller + "/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.SetBearerToken(Token);

            HttpResponseMessage response = await client.PostAsJsonAsync(action, obj);
            if (response.IsSuccessStatusCode)
            {
                T result = await response.Content.ReadFromJsonAsync<T>();
                return result;
            }
            throw new Exception(response.ReasonPhrase);

        }
    }
}
