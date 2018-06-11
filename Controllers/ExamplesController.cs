using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreIntro.Controllers {

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
    [Route("example/api/{param}")]
    public JsonResult Params(string param) => Json(new { param = param });

  }
}