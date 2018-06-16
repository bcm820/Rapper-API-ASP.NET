using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace AspNetCoreIntro {

  // Handles all our request methods to external APIs.
  public class ApiProxier {

    private readonly IConfiguration Config;
    public ApiProxier(IConfiguration configuration) => Config = configuration;

    // Set request URI and optional bearer token from config
    public async Task<object> Get(string service, string endpoint) {
      var Client = new HttpClient();
      var Host = Config.GetValue<string>($"{service}:host");
      Client.BaseAddress = new Uri($"{Host}{endpoint}");
      var Token = Config.GetValue<string>($"{service}:token");
      if (Token != null) Client.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue("Bearer", Token);
      return await GetAsync(Client);
    }

    async Task<object> GetAsync(HttpClient client) {
      try { return await ParseResponse(client); } catch (HttpRequestException error) {
        return new { error = error };
      }
    }

    async Task<object> ParseResponse(HttpClient client) {
      var Response = await client.GetAsync("");
      Response.EnsureSuccessStatusCode();
      var ResponseString = await Response.Content.ReadAsStringAsync();
      return JsonConvert.DeserializeObject<object>(ResponseString);
    }

  }
}