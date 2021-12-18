using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using ProductivityTools.MasterConfiguration;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ProductivityTools.GetTask3.Sdk
{
    public class GetTaskHttpClient
    {
        //static string URL = "http://apigettask3.productivitytools.tech:8040/api/";// Consts.EndpointAddress;
        //static string URL = "http://localhost:5513/api/";// Consts.EndpointAddress;


        private readonly string URL;
        private readonly Action<String> Log;
        IConfigurationRoot Configuration;

        public GetTaskHttpClient(string url, IConfigurationRoot configuration, Action<string> log)
        {
            this.URL = url;
            this.Log = log;
            this.Configuration = configuration;
            
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

        private void SetNewAccessToken()
        {
            //IConfigurationRoot configuration = null;
            //try
            //{
            //    configuration = new ConfigurationBuilder()
            //        .AddMasterConfiguration("ProductivityTools.GetTask3.Client.json")
            //        .Build();
            //}
            //catch (Exception ex)
            //{

            //    Console.WriteLine(ex.Message);
            //    throw;
            //}



            Log("token is empty need to call identity server");
            var client = new System.Net.Http.HttpClient();

            var disco = client.GetDiscoveryDocumentAsync("https://identityserver.productivitytools.tech:8010/").Result;
            Log($"Discovery server{disco}");
            if (disco.IsError)
            {
                Log(disco.Error);
            }

            Log("GetTask3Cmdlet secret");
            Log(Configuration["GetTask3Cmdlet"]);

            var tokenResponse = client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,

                ClientId = "GetTask3Cmdlet",
                ClientSecret = Configuration["GetTask3Cmdlet"],
                Scope = "GetTask3.API"
            }).Result;

            Log("Token response pw:");
            Log(tokenResponse.AccessToken);
            Log(tokenResponse.Error);

            if (tokenResponse.IsError)
            {
                Log(tokenResponse.Error);
            }

            Log(tokenResponse.Json.ToString());
            token = tokenResponse.AccessToken;
        }



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
                T result = await response.Content.ReadAsAsync<T>();
                return result;
            }
            throw new Exception(response.ReasonPhrase);

        }
    }
}
