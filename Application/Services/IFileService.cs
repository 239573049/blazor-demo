using Entitys.File;
using System.IO.Compression;
using System.Security;
using System.Text;
using Utils;

namespace Application.Services;
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
    /// 删除全部
    /// </summary>
    /// <param name="paths"></param>
    /// <returns></returns>
    bool DeleteAll(List<DeleteAllDto> paths);
    /// <summary>
    /// 重命名文件夹
    /// </summary>
    /// <param name="directoryPath"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    bool UpdateNameDirectory(string directoryPath, string name);
    /// <summary>
    /// 重命名文件
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    bool UpdateNameFile(string filePath, string name);
    /// <summary>
    /// 获取文件内容
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    string? GetFileContent(string filePath);
    /// <summary>
    /// 编辑文件内容
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="content"></param>
    /// <returns></returns>
    bool UpdateFileContent(string filePath, string content);
    /// <summary>
    /// 压缩文件(ZIP)
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    string CompressDirectoryZIP(string directoryPath);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="directoryPath"></param>
    /// <returns></returns>
    string ExtractToDirectoryZip(string directoryPath);
    /// <summary>
    /// 创建文件夹
    /// </summary>
    /// <param name="path"></param>
    /// <param name="name"></param>
    string CreateDirectory(string path,string name);
    /// <summary>
    /// 上传文件
    /// </summary>
    /// <param name="stream"></param>
    /// <returns></returns>
    bool CreateFile(List<CreateFileDto> stream);
}
public class FileService : IFileService
{
    public string CompressDirectoryZIP(string directoryPath)
    {
        if (!Directory.Exists(directoryPath)) throw new BusinessLogicException("文件不存在或者已经被删除");
        var compressPath = directoryPath + ".zip";
        try
        {
            ZipFile.CreateFromDirectory(directoryPath, compressPath);
        }
        catch (UnauthorizedAccessException)
        {
            throw new BusinessLogicException("没有权限");
        }
        catch (IOException)
        {
            throw new BusinessLogicException("可能存在项目压缩名称文件");
        }
        return compressPath;
    }

    public string CreateDirectory(string path, string name)
    {
        path = Path.Combine(path, name);
        _= Directory.CreateDirectory(path);
        return path;
    }

    public bool CreateFile(List<CreateFileDto> stream)
    {
        foreach (var d in stream)
        {
            var file= File.Create(d.Path!);
            var bytes= new byte[d.stream.Length];
            d.stream.Read(bytes, 0, bytes.Length);
            file.Write(bytes,0, bytes.Length);
            file.Close();
            bytes = null;
        }
        return true;
    }

    public bool DeleteAll(List<DeleteAllDto> paths)
    {
        foreach (var d in paths)
        {
            if (!d.IsFile)
            {
                DeleteDirectory(d.Path);
            }
            else
            {
                DeleteFile(d.Path);
            }
        }
        return true;
    }

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

    public string ExtractToDirectoryZip(string directoryPath)
    {
        var newPath = directoryPath.TrimEnd('p', 'i', 'z', '.');
        if (!IsExitFile(directoryPath)) throw new BusinessLogicException("解压文件不存在");
        try
        {
            ZipFile.ExtractToDirectory(directoryPath, newPath);
        }
        catch (UnauthorizedAccessException)
        {
            throw new BusinessLogicException("没有权限");
        }
        catch (IOException)
        {
            throw new BusinessLogicException("可能存在项目压缩名称文件");
        }
        return newPath;
    }

    public  string? GetFileContent(string filePath)
    {
        if (!IsExitFile(filePath)) throw new BusinessLogicException("文件不存在");
        try
        {
            using var content = File.OpenRead(filePath);
            if (content.Length > 1024 * 1024 * 2)
            {
                throw new BusinessLogicException("文件大于2MB无法打开");
            }
            var bytes = new byte[content.Length];
            content.Read(bytes, 0, bytes.Length);
            return Encoding.UTF8.GetString(bytes);
        }
        catch (FileNotFoundException)
        {
            throw new BusinessLogicException("没有找到路径中指定的文件。");
        }catch (UnauthorizedAccessException)
        {
            throw new BusinessLogicException("没有权限");
        }
    }

    public List<FilesDto> GetFileData(string filePath)
    {
        try
        {

            if (!Directory.Exists(filePath))
                return new List<FilesDto>();
            var files = new List<FilesDto>();
            var data = Directory.GetDirectories(filePath);
            foreach (var d in data)
            {
                files.Add(new FilesDto { IsFile = false, Path = d, Name = d.Replace(filePath, "") });
            }
            var getFileDatas = Directory.GetFiles(filePath);
            foreach (var d in getFileDatas)
            {
                var file = new FilesDto { Name = d.Replace(filePath, ""), IsFile = true, Path = d };
                file.Length = GetFileLength(d);
                file.LengthName = FileUtil.GetSizeFormat(file.Length);
                files.Add(file);
            }
            return files;
        }
        catch (UnauthorizedAccessException)
        {
            throw new BusinessLogicException("没有权限");
        }
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

    public bool UpdateFileContent(string filePath, string content)
    {
        if (!IsExitFile(filePath)) throw new BusinessLogicException("文件不存在");
        try
        {
            File.WriteAllBytes(filePath, Encoding.UTF8.GetBytes(content));
        }
        catch (NotSupportedException)
        {
            throw new BusinessLogicException("路径格式无效。");
        }catch (SecurityException)
        {
            throw new BusinessLogicException("写入失败没有权限");
        }
        catch (ArgumentNullException)
        {
            throw new BusinessLogicException("路径为空或字节数组为空。");
        }
        catch (Exception ex)
        {
            throw new BusinessLogicException("写入错误:"+ex.Message);
        }
        return true;
    }

    public bool UpdateNameDirectory(string directoryPath, string name)
    {
        var path = directoryPath.Split("\\");
        if (path[^1] == name) return true;
        if (path.Length > 1)
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
            throw new BusinessLogicException("没有权限");
        }
        catch (Exception)
        {
            throw new BusinessLogicException("重命名错误");
        }
        return true;
    }

    public bool UpdateNameFile(string filePath, string name)
    {
        var path = filePath.Split("\\");
        if (path[^1] == name) return true;
        if (path.Length > 1)
            path = path.Take(path.Length - 1).ToArray();
        if (!File.Exists(filePath)) throw new BusinessLogicException("文件不存在");
        name = string.Join("\\", path) + "\\" + name;
        if (Directory.Exists(name)) throw new BusinessLogicException("已经存在相同文件");
        try
        {
            File.Move(filePath, name);
        }
        catch (FileNotFoundException)
        {
            throw new BusinessLogicException("原地址不存在");
        }
        catch (NotSupportedException)
        {
            throw new BusinessLogicException("路径不合法");
        }
        catch (UnauthorizedAccessException)
        {
            throw new BusinessLogicException("没有权限");
        }
        catch (Exception)
        {
            throw new BusinessLogicException("重命名错误");
        }
        return true;
    }
}