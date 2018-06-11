using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreIntro.Controllers {

  public class ExamplesController : Controller {

    // Basic GET route
    [Route("")] public string Index() => "Hello World!";

    // Route params returning JSON (of any object type)
    // can pass in an existing or anonymous new object
    [Route("params/{param}")]
    public JsonResult RouteParams(string param) => Json(new { param = param });

  }
}