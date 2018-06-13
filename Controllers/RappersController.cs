using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Rappers;

namespace AspNetCoreIntro.Controllers {

  public class RappersController : Controller {
    public List<Artist> rappers;
    public List<Group> groups;
    public Dictionary<string, List<string>> routes;

    public RappersController() {
      rappers = JsonToFile<Artist>.ReadJson();
      groups = JsonToFile<Group>.ReadJson();
      routes = new Dictionary<string, List<string>>();
      routes["rapper"] = new List<string> {
        "/api",
        "/api/name/{name}",
        "/api/realname/{realname}",
        "/api/hometown/{hometown}"
      };
      routes["group"] = new List<string> {
        "/api/groups",
        "/api/groups/{groupname}",
        "/api/groups/showMembers={true/false}"
      };
    }

    [Route("rappers")]
    public IActionResult Index() {
      ViewBag.routes = routes;
      ViewBag.routesList = routes["rapper"].Concat(routes["group"]);
      ViewBag.visited = HttpContext.Session.FindOrCreateList("visited");
      ViewBag.cleared = TempData["cleared"];
      return View();
    }

    [Route("rappers/reset")]
    public IActionResult Reset() {
      HttpContext.Session.Clear();
      TempData["cleared"] = DateTime.Now;
      return RedirectToAction("Index");
    }

    [Route("rappers/api")]
    public JsonResult RapperList() {
      HttpContext.Session.AppendToList("visited", "/api");
      return Json(rappers);
    }

    [Route("rappers/api/name/{name}")]
    public JsonResult ByName(string name) {
      HttpContext.Session.AppendToList("visited", $"/api/name/{name}");
      return Json(rappers.Where(r => r.ArtistName.ContainsIgnoreCase(name)));
    }

    [Route("rappers/api/realName/{realname}")]
    public JsonResult ByRealName(string realname) {
      HttpContext.Session.AppendToList("visited", $"/api/realname/{realname}");
      return Json(rappers.Where(r => r.RealName.ContainsIgnoreCase(realname)));
    }

    [Route("rappers/api/hometown/{hometown}")]
    public JsonResult ByHometown(string hometown) {
      HttpContext.Session.AppendToList("visited", $"/api/hometown/{hometown}");
      return Json(rappers.Where(r => r.Hometown.ContainsIgnoreCase(hometown)));
    }

    [Route("rappers/api/groups")]
    public JsonResult GroupList() {
      HttpContext.Session.AppendToList("visited", $"/api/groups");
      return Json(groups);
    }

    [Route("rappers/api/groups/{groupname}")]
    public JsonResult ByGroupName(string groupname) {
      HttpContext.Session.AppendToList("visited", $"/api/groups/{groupname}");
      return Json(groups.Where(g => g.GroupName.ContainsIgnoreCase(groupname)));
    }

    [Route("rappers/api/groups/showMembers={show}")]
    public JsonResult AllGroups(string show) {
      if (show != "true") return GroupList();
      var rappersGroup = rappers.GroupBy(r => r.GroupId);
      foreach (var rG in rappersGroup) {
        var idx = groups.FindIndex(g => g.Id == rG.Key);
        if (idx > -1)
          foreach (var r in rG)
            groups[idx].Members.Add(r);
      }
      HttpContext.Session.AppendToList("visited", $"/api/groups/showMembers={show}");
      return Json(groups);
    }

  }
}