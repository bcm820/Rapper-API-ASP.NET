using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace AspNetCoreIntro.Models {

  public class Group {
    public int Id;
    public string GroupName;
    public List<Artist> Members;
  }

  public class Artist {
    public string ArtistName;
    public string RealName;
    public int Age;
    public string Hometown;
    public int GroupId;
  }

  public class JsonToFile<T> {
    public static List<T> ReadJson() {
      string Filename = $"data/{typeof(T).Name}.json";
      using (StreamReader FileToConvert = File.OpenText(Filename)) {
        JsonSerializer Serializer = new JsonSerializer();
        return (List<T>)Serializer.Deserialize(FileToConvert, typeof(List<T>));
      }
    }
  }

}