using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace AspNetCoreIntro {

  // SessionExtensions adds additional static methods to the Http.ISession class.
  // For each method definition, "this ISession" is implicitly passed in when called.
  public static class SessionExtensions {

    // Serializes list of objects into JSON to store as a string.
    public static void SetList(this ISession session, string key, List<object> data) {
      session.SetString(key, JsonConvert.SerializeObject(data));
    }

    // Gets stored JSON string and deserializes into list of objects.
    public static List<object> GetList(this ISession session, string key) {
      string SessionList = session.GetString(key);
      return SessionList == null ? null : JsonConvert.DeserializeObject<List<object>>(SessionList);
    }

    // Checks session for matching key. If found, returns stored list.
    // If not found, stores and returns a new list of objects.
    public static List<object> FindOrCreateList(this ISession session, string key) {
      var SessionList = session.GetList(key); // note: no need to pass ISession in
      if (SessionList == null) {
        SessionList = new List<object>();
        session.SetList(key, SessionList);
      }
      return SessionList;
    }

    // Checks session for matching key, returning the existing list or a new list.
    // Appends new data to the list and returns the updated list.
    public static List<object> AppendToList(this ISession session, string key, object data) {
      List<object> SessionList = session.FindOrCreateList(key);
      SessionList.Add(data);
      session.SetList(key, SessionList);
      return SessionList;
    }

  }

}