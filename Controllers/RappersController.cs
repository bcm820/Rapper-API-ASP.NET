using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Rappers;

namespace AspNetCoreIntro.Controllers {

  public class RappersController : Controller {
    public List<Artist> rappers;
    public List<Group> groups;
    public RappersController() {
      rappers = JsonToFile<Artist>.ReadJson();
      groups = JsonToFile<Group>.ReadJson();
    }

    [Route("rappers/")] public IActionResult Index() => View();
    [Route("rappers/api")] public JsonResult RapperList() => Json(rappers);

    [Route("rappers/api/name/{Name}")]
    public JsonResult ByName(string Name) =>
      Json(rappers.Where(r => r.ArtistName.Contains(Name)));

    [Route("rappers/api/realName/{RealName}")]
    public JsonResult ByRealName(string RealName) =>
      Json(rappers.Where(r => r.RealName.Contains(RealName)));

    [Route("rappers/api/hometown/{Hometown}")]
    public JsonResult ByHometown(string Hometown) =>
      Json(rappers.Where(r => r.Hometown.Contains(Hometown)));

    [Route("rappers/api/groups")]
    public JsonResult GroupList() => Json(groups);

    [Route("rappers/api/groups/{Name}")]
    public JsonResult ByGroupName(string Name) =>
      Json(groups.Where(g => g.GroupName.Contains(Name)));

    [Route("rappers/api/groups/showAll={show}")]
    public JsonResult AllGroups(string show) {
      if (show != "true") return GroupList();
      var rappersGroup = rappers.GroupBy(r => r.GroupId);
      foreach (var rG in rappersGroup) {
        var idx = groups.FindIndex(g => g.Id == rG.Key);
        if (idx > -1)
          foreach (var r in rG)
            groups[idx].Members.Add(r);
      }
      return Json(groups);
    }
  }
}