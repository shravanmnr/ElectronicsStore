using System.Net;
using System.Web.Mvc;

namespace ElectronicsStore.Controllers
{
    public abstract class BaseController : Controller
    {
        // 200 OK — successful read or form re-display after validation error
        protected ActionResult OkView(object model = null)
        {
            Response.StatusCode = (int)HttpStatusCode.OK;
            return model == null ? View() : View(model);
        }

        // 404 Not Found — resource does not exist
        protected new ActionResult HttpNotFound(string statusDescription = null)
        {
            return new HttpStatusCodeResult(HttpStatusCode.NotFound, statusDescription ?? "Resource not found.");
        }

        // 400 Bad Request — missing or malformed input (e.g. null id)
        protected ActionResult BadRequest(string statusDescription = null)
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest, statusDescription ?? "Bad request.");
        }

        // 500 Internal Server Error — unexpected server-side failure
        protected ActionResult InternalServerError(string statusDescription = null)
        {
            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, statusDescription ?? "An unexpected error occurred.");
        }

        // Sets 500 on the response while still returning a view (for POST form re-display)
        protected ActionResult ServerErrorView(object model = null)
        {
            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            Response.TrySkipIisCustomErrors = true;
            return model == null ? View() : View(model);
        }
    }
}
