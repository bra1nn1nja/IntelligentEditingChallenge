using Intelligent_Editing.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Intelligent_Editing.Facades;
using Microsoft.AspNetCore.Diagnostics;

namespace Intelligent_Editing.Controllers
{
    public class UploadController : Controller
    {
        private readonly ILogger<UploadController> _logger;
        private readonly IUploadFacade _facade;

        public UploadController(ILogger<UploadController> logger, IUploadFacade facade)
        {
            _logger = logger;
            _facade = facade;   
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadFile(IFormFile file)
        {
            return View(_facade.ProcessFile(file));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var exceptionHandlerPathFeature =
                HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            return View(new ErrorViewModel { Message = exceptionHandlerPathFeature.Error.Message });
        }
    }
}
