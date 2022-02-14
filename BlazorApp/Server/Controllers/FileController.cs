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
        /// <summary>
        /// 删除所有文件
        /// </summary>
        /// <param name="paths"></param>
        /// <returns></returns>
        [HttpPost]
        public bool DeleteAll(List<DeleteAllDto> paths)
        {
            return _fileService.DeleteAll(paths);
        }
        /// <summary>
        /// 编辑文件夹名称
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPut]
        public bool UpdateNameDirectory(string directoryPath, string name)
        {
            return _fileService.UpdateNameDirectory(directoryPath, name);
        }
        /// <summary>
        /// 编辑文件名称
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPut]
        public bool UpdateNameFile(string filePath, string name)
        {
            return _fileService.UpdateNameFile(filePath,name);
        }
        /// <summary>
        /// 获取文件内容
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        [HttpGet]
        public string? GetFileContent(string filePath)
        {
            return  _fileService.GetFileContent(filePath);
        }
        /// <summary>
        /// 编辑文件内容
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        [HttpPost]
        public bool UpdateFileContent(string filePath,[FromBody]string content)
        {
            return _fileService.UpdateFileContent(filePath, content);
        }
        /// <summary>
        /// 压缩文件夹（zip）
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <returns></returns>
        [HttpGet]
        public string CompressDirectoryZIP(string directoryPath)
        {
            return _fileService.CompressDirectoryZIP(directoryPath);
        }
        /// <summary>
        /// 解压zip
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <returns></returns>
        [HttpGet]
        public string ExtractToDirectoryZip(string directoryPath)
        {
            return _fileService.ExtractToDirectoryZip(directoryPath);
        }
    }
}
