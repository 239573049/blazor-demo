using Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IFileService
    {
        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        /// <param name="fille"></param>
        /// <returns></returns>
        bool IsExitFile(string fille);
        List<FilesDto> GetFileData(string filePath);
        /// <summary>
        /// 获取文件大小
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        long? GetFileLength(string filePath); 

    }
    public class FiFileSipub : IFileService
    {
        public List<FilesDto> GetFileData(string filePath)
        {

            if (Directory.Exists(filePath))
                return new List<FilesDto>();
            var files=new List<FilesDto>();
            var data=Directory.GetDirectories(filePath);
            foreach (var d in data)
            {
                files.Add(new FilesDto { IsFile=false,Name=d});
            }
            var getFileDatas=Directory.GetFiles(filePath);
            foreach (var d in getFileDatas)
            {
                var file = new FilesDto {Name=d,IsFile=true };
                file.Length=GetFileLength(d);
                files.Add(file);
            }
            return files;
        }

        public long? GetFileLength(string filePath)
        {
            using(var file = File.OpenRead(filePath))
            {
                return file.Length;
            }
            
        }

        public bool IsExitFile(string fille)
        {
            return File.Exists(fille);
        }
       
    }
}
