using Entitys.File;

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
        /// <summary>
        /// 获取文件列表
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        List<FilesDto> GetFileData(string filePath);
        /// <summary>
        /// 获取文件大小
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        long? GetFileLength(string filePath);
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        bool DeleteFile(string filePath);

    }
    public class FileService : IFileService
    {
        public bool DeleteFile(string filePath)
        {
            if (!File.Exists(filePath))return true;
            File.Delete(filePath);  
            return true;
        }

        public List<FilesDto> GetFileData(string filePath)
        {
            if (!Directory.Exists(filePath))
                return new List<FilesDto>();
            var files=new List<FilesDto>();
            var data=Directory.GetDirectories(filePath);
            foreach (var d in data)
            {
                files.Add(new FilesDto { IsFile=false,Path=d,Name=d.Replace(filePath,"")});
            }
            var getFileDatas=Directory.GetFiles(filePath);
            foreach (var d in getFileDatas)
            {
                var file = new FilesDto {Name=d.Replace(filePath,""),IsFile=true,Path=d};
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
