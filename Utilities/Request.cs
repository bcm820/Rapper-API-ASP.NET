using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Specialized;
using Newtonsoft.Json;

using Microsoft.Extensions.Options;

namespace AspNetCoreIntro {

  // Async API call method returns "Task" object representing asynchronous process.
  // Await keyword forces main thread to wait until the Task finishes executing.

  /* EXAMPLE TO TEST
  [Route("rappers/api/external/{id}")]
  public IActionResult GetRapperInfo(int RapperId) {
    string Url = $"https://rapperdb.io/api/byId/{RapperId}";
    var RapperInfo = MakeRequest(Url);
  }
  */

  public class ApiRequest {

    private readonly Dictionary<string, NameValueCollection> Config;

    public ApiRequest(IOptions<ApiConfig> config) {
      Config = config as Dictionary<string, NameValueCollection>;
    }

    public async Task<Dictionary<string, object>>
      MakeRequest(string service, string endpoint, bool requiresAuth) {
      var Client = new HttpClient();

      try {

        // Set request URI and optional bearer token from config
        if (requiresAuth) Client.DefaultRequestHeaders.Authorization =
          new AuthenticationHeaderValue("Bearer", Config[service]["token"]);
        var Response = await Client.GetAsync(Config[service]["host"] + endpoint);
        Response.EnsureSuccessStatusCode();

        string ResponseStr = await Response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<Dictionary<string, object>>(ResponseStr);

      } catch (HttpRequestException Error) {
        return new Dictionary<string, object> { ["error"] = Error };

      }

    }
  }
}