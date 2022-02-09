using Entitys.File;
using Utils;

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
        /// <summary>
        /// 删除文件夹
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <returns></returns>
        bool DeleteDirectory(string directoryPath);
        /// <summary>
        /// 重命名文件夹
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        bool UpdateNameDirectory(string directoryPath,string name);
        /// <summary>
        /// 重命名文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        bool UpdateNameFile(string filePath,string name);
    }
    public class FileService : IFileService
    {
        public bool DeleteDirectory(string directoryPath)
        {
            try
            {
                if (!Directory.Exists(directoryPath)) throw new BusinessLogicException("文件夹不存在");
                Directory.Delete(directoryPath, true);
                return true;
            }
            catch (IOException)
            {
                throw new BusinessLogicException("删除失败");
            }
        }

        public bool DeleteFile(string filePath)
        {
            if (!File.Exists(filePath)) throw new BusinessLogicException("文件不存在");
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
                file.LengthName = FileUtil.GetSizeFormat(file.Length);
                files.Add(file);
            }
            return files;
        }

        public long? GetFileLength(string filePath)
        {
            try
            {
                using (var file = File.OpenRead(filePath))
                {
                    return file.Length;
                }
            }
            catch (IOException)
            {
                return 0;
            }
            
        }

        public bool IsExitFile(string fille)
        {
            return File.Exists(fille);
        }

        public bool UpdateNameDirectory(string directoryPath, string name)
        {
            var path = directoryPath.Split("\\");
            if (path[^1] == name) return true;
            if(path.Length  >1)
                path = path.Take(path.Length - 1).ToArray();
            if (!Directory.Exists(directoryPath)) throw new BusinessLogicException("文件夹不存在");
            name = string.Join("\\", path) + "\\" + name;
            if (Directory.Exists(name)) throw new BusinessLogicException("已经存在相同文件夹名称");
            try
                {
                Directory.Move(directoryPath, name);
            }
            catch (UnauthorizedAccessException)
            {
                throw new BusinessLogicException("访问错误");
            }
            catch (Exception)
            {
                throw new BusinessLogicException("重命名错误");
            }
            return true;
        }

        public bool UpdateNameFile(string filePath, string name)
        {
            var path=filePath.Split("\\");
            if (path[^1] == name) return true;
            if (path.Length > 1)
                path = path.Take(path.Length - 1).ToArray();
            if (!File.Exists(filePath)) throw new BusinessLogicException("文件不存在");
            name=string.Join("\\", path)+"\\" + name;
            if (Directory.Exists(name)) throw new BusinessLogicException("已经存在相同文件");
            try
            {
                File.Move(filePath, name);
            }
            catch (FileNotFoundException)
            {
                throw new BusinessLogicException("原地址不存在");
            }catch (NotSupportedException)
            {
                throw new BusinessLogicException("路径不合法");
            }
            catch (Exception)
            {
                throw new BusinessLogicException("重命名错误");
            }
            return true;
        }
    }
}
