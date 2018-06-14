using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreIntro.Controllers {

  /* See RappersController for more examples. */
  public class ExamplesController : Controller {

    // Basic GET route (without serving view)
    [Route("")] public string Index() => "Hello World!";

    // Basic GET route to render view
    // If no View arg provided, defaults to "index";
    // Otherwise, specify filename without .cshtml extension
    // AspNetCore Mvc finds file in /Views/{ControllerName}/
    [Route("example")] public IActionResult Page() => View();

    // Route params (of any object type)
    // Example of serving JSON for APIs
    [Route("example/params/{param}")]
    public JsonResult Params(string param) => Json(new { param = param });

    // POST request with form-data in headers
    // input name -> string arguments into method
    // i.e. user enters "HELLO" into inputField -> { result: "HELLO" }
    [HttpPost]
    [Route("example/formPost")]
    public JsonResult PostRoute(string inputField) {
      return Json(new { result = inputField });
    }

    // Redirect to another method via RedirectToAction()
    // Object given as second argument map to params of that method
    [Route("example/redirectToOtherMethod")]
    public IActionResult RedirectToOtherMethod() {
      var args = new { Amt = 5, Food = "sandwiches" };
      return RedirectToAction("MethodToDirectTo", args);
    }
    [Route("example/anywhereItDontMatter")]
    public JsonResult MethodToRedirectTo(string food, int amt) =>
      Json($"I ate {amt} {food}."); // "I ate 5 sandwiches"

    // Redirect to another method in a diff controller...
    [Route("example/redirectToDiffControllerMethod")]
    public IActionResult RedirectToDiffControllerMethod() =>
      RedirectToAction("SomeMethodInADiffController", "Other");

    // Redirect to another route by route name...
    // Typically it'll be easier to redirect via methods though.
    [Route("example/redirectToAnotherRoute")]
    public IActionResult RedirectToAnotherRoute() =>
      RedirectToRoute("dir/nameOfRoute");
  }
}