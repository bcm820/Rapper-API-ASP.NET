using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace AspNetCoreIntro {

  public static class SessionExtensions {

    // Serialize to JSON and store as a string in session
    public static void SetList(this ISession session, string key, List<string> value) {
      session.SetString(key, JsonConvert.SerializeObject(value));
    }

    public static List<string> GetList(this ISession session, string key) {
      string value = session.GetString(key);
      return value == null ? null : JsonConvert.DeserializeObject<List<string>>(value);
    }
  }

}