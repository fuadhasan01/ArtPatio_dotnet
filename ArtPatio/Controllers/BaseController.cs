using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ArtPatio.Controllers
{
    public class BaseController : Controller
    {
        public bool IsUserLoggedIn()
        {
            return HttpContext.Session.GetInt32("Id") != null;
        }

        // Keep public access modifier when overriding
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!IsUserLoggedIn())
            {
                // Redirect to login if the user is not logged in
                context.Result = new RedirectToActionResult("Login", "Account", null);
            }

            base.OnActionExecuting(context);
        }
    }
}
