using Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorApp.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;
        public FileController(
            IFileService fileServic
            )
        {
            _fileService = fileServic;
        }
        [HttpGet]
        public IActionResult IsExit(string filePath)
        {
            return new OkObjectResult(_fileService.IsExitFile(filePath));
        }
    }
}
