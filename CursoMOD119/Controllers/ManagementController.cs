using CursoMOD119.Lib;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CursoMOD119.Controllers
{
    [Authorize(Policy = AppConstants.APP_ADMIN_POLICY)]
    public class ManagementController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
