using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;

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

  public class WebRequest {

    public static async Task<Dictionary<string, object>> MakeRequest(string url) {
      var client = new HttpClient();

      try {
        var Response = await client.GetAsync(url);
        Response.EnsureSuccessStatusCode();
        string ResponseStr = await Response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<Dictionary<string, object>>(ResponseStr);

      } catch (HttpRequestException error) {
        return new Dictionary<string, object>() { { "error", error } };
      }

    }
  }
}