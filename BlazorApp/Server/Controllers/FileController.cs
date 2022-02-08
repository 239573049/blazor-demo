using Application.Services;
using Entitys.File;
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
        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult IsExit(string filePath)
        {
            return new OkObjectResult(_fileService.IsExitFile(filePath));
        }
        /// <summary>
        /// 获取文件列表
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        [HttpGet]
        public List<FilesDto> GetFileData(string filePath)
        {
            return _fileService.GetFileData(filePath);
        }
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        [HttpDelete]
        public bool DeleteFile(string filePath)
        {
            return _fileService.DeleteFile(filePath);
        }
        /// <summary>
        /// 删除文件夹
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <returns></returns>
        [HttpDelete]
        public bool DeleteDirectory(string directoryPath)
        {
            return _fileService.DeleteDirectory(directoryPath);
        }
    }
}
