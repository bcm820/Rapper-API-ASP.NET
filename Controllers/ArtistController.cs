using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Artists;

using Microsoft.EntityFrameworkCore;
using AspNetCoreIntro.Models;

namespace AspNetCoreIntro.Controllers {

  public class ArtistController : Controller {

    // Mock API data lists
    // (to replace with fetched API data)
    public List<Artist> Artists;
    public List<Group> Groups;

    // Inject EF Core DB context
    private Context Db;
    // Db.Users.ToList();

    // Dictionary of route links to render on view
    public Dictionary<string, List<string>> Routes;

    public ArtistController(Context context) {
      Db = context;
      Artists = JsonToFile<Artist>.ReadJson();
      Groups = JsonToFile<Group>.ReadJson();
      Routes = new Dictionary<string, List<string>> {
        ["rapper"] = new List<string> {
            "/api",
            "/api/name/{name}",
            "/api/realname/{realname}",
            "/api/hometown/{hometown}"
        },
        ["group"] = new List<string> {
          "/api/groups",
          "/api/groups/{groupname}",
          "/api/groups/showMembers={true/false}"
        }
      };
    }

    [Route("")]
    public IActionResult Index() {
      ViewBag.Routes = Routes;
      ViewBag.RoutesList = Routes["rapper"].Concat(Routes["group"]);
      ViewBag.Visited = HttpContext.Session.FindOrCreateList("visited");
      ViewBag.Cleared = TempData["cleared"];
      return View();
    }

    [Route("reset")]
    public IActionResult Reset() {
      HttpContext.Session.Clear();
      TempData["cleared"] = DateTime.Now;
      return RedirectToAction("Index");
    }

    [Route("api")]
    public JsonResult RapperList() {
      HttpContext.Session.AppendToList("visited", "/api");
      return Json(Artists);
    }

    [Route("api/name/{name}")]
    public JsonResult ByName(string name) {
      HttpContext.Session.AppendToList("visited", $"/api/name/{name}");
      return Json(Artists.Where(r => r.ArtistName.ContainsIgnoreCase(name)));
    }

    [Route("api/realName/{realname}")]
    public JsonResult ByRealName(string realname) {
      HttpContext.Session.AppendToList("visited", $"/api/realname/{realname}");
      return Json(Artists.Where(r => r.RealName.ContainsIgnoreCase(realname)));
    }

    [Route("api/hometown/{hometown}")]
    public JsonResult ByHometown(string hometown) {
      HttpContext.Session.AppendToList("visited", $"/api/hometown/{hometown}");
      return Json(Artists.Where(r => r.Hometown.ContainsIgnoreCase(hometown)));
    }

    [Route("api/groups")]
    public JsonResult GroupList() {
      HttpContext.Session.AppendToList("visited", $"/api/groups");
      return Json(Groups);
    }

    [Route("api/groups/{groupname}")]
    public JsonResult ByGroupName(string groupname) {
      HttpContext.Session.AppendToList("visited", $"/api/groups/{groupname}");
      return Json(Groups.Where(g => g.GroupName.ContainsIgnoreCase(groupname)));
    }

    [Route("api/groups/showMembers={show}")]
    public JsonResult AllGroups(string show) {
      if (show != "true") return GroupList();
      var ArtistsGroup = Artists.GroupBy(r => r.GroupId);
      foreach (var rG in ArtistsGroup) {
        var idx = Groups.FindIndex(g => g.Id == rG.Key);
        if (idx > -1)
          foreach (var r in rG)
            Groups[idx].Members.Add(r);
      }
      HttpContext.Session.AppendToList("visited", $"/api/groups/showMembers={show}");
      return Json(Groups);
    }

  }
}