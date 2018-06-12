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

    public static List<string> FindOrStartSession(HttpContext ctx) {
      var strList = ctx.Session.GetList("strList");
      if (strList == null) {
        strList = new List<string>();
        ctx.Session.SetList("strList", strList);
      }
      return strList;
    }

    public static List<string> UpdateSession(HttpContext ctx, string str) {
      List<string> strList = FindOrStartSession(ctx);
      strList.Add(str);
      ctx.Session.SetList("strList", strList);
      return strList;
    }

    [Route("rappers")]
    public IActionResult Index() {
      ViewBag.routes = routes;
      ViewBag.routesList = routes["rapper"].Concat(routes["group"]);
      ViewBag.visited = FindOrStartSession(HttpContext);
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
      UpdateSession(HttpContext, $"/api");
      return Json(rappers);
    }

    [Route("rappers/api/name/{name}")]
    public JsonResult ByName(string name) {
      UpdateSession(HttpContext, $"/api/name/{name}");
      return Json(rappers.Where(r => r.ArtistName.Contains(name)));
    }

    [Route("rappers/api/realName/{realname}")]
    public JsonResult ByRealName(string realname) {
      UpdateSession(HttpContext, $"/api/realname/{realname}");
      return Json(rappers.Where(r => r.RealName.Contains(realname)));
    }

    [Route("rappers/api/hometown/{hometown}")]
    public JsonResult ByHometown(string hometown) {
      UpdateSession(HttpContext, $"/api/hometown/{hometown}");
      return Json(rappers.Where(r => r.Hometown.Contains(hometown)));
    }

    [Route("rappers/api/groups")]
    public JsonResult GroupList() {
      UpdateSession(HttpContext, $"/api/groups");
      return Json(groups);
    }

    [Route("rappers/api/groups/{groupname}")]
    public JsonResult ByGroupName(string groupname) {
      UpdateSession(HttpContext, $"/api/groups/{groupname}");
      return Json(groups.Where(g => g.GroupName.Contains(groupname)));
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
      UpdateSession(HttpContext, $"/api/groups/showMembers={show}");
      return Json(groups);
    }

  }
}