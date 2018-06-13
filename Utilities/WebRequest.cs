using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace AspNetCoreIntro {

  // Async API call method returns "Task" object representing its asynchronous process.
  // The await keyword forces the main thread to wait until the Task is finished executing.
  // In our controllers, a lambda callback will handle the return.

  /* EXAMPLE:
  [Route("rappers/api/external/{id}")]
  public IActionResult GetRapperInfo(int RapperId) {
    string Url = $"https://rapperdb.io/api/byId/{RapperId}";
    var RapperInfo = new Dictionary<string, object>();
    WebRequest.GetAsync(Url, JsonRes => RapperInfo = JsonRes).Wait();
  }
  */

  public class WebRequest {

    // Receives an id and an Action returning a dictionary of objects.
    // Or if API returns an array, parameter type can simply be Action.
    public static async Task GetAsync(string url, Action<Dictionary<string, object>> Callback) {

      // Create temporary HttpClient connection.
      using (var Client = new HttpClient()) {

        try {
          Client.BaseAddress = new Uri(url);
          HttpResponseMessage Response = await Client.GetAsync(""); // Make the API call
          Response.EnsureSuccessStatusCode(); // Throw error if not successful
          string ResponseStr = await Response.Content.ReadAsStringAsync(); // Read response as a string

          // Parse the result into JSON and convert to a dictionary
          // DeserializeObject will only parse the top level object
          // Depending on the API we may need to dig deeper and continue deserializing
          var JsonResponse = JsonConvert.DeserializeObject<Dictionary<string, object>>(ResponseStr);

          // Execute our callback, passing it the response we got.
          Callback(JsonResponse);

        } catch (HttpRequestException error) {
          Console.WriteLine($"Request exception: {error.Message}");

        }
      }
    }
  }
}