using BlazorHttpUtil;
using Entitys.File;
using Entitys.Web;

namespace BlazorApp.Client.Api;
public class FileAdminApi
{
    private readonly IHttpHelp _httpHelp;

    public FileAdminApi(IHttpHelp httpHelp)
    {
        _httpHelp = httpHelp;
    }
    public static string Modules = "/api/File/";
    public async Task<List<FilesDto>> GetFileData(string path)
    {
        var data = await _httpHelp.GetAsync<ModelStateResult<List<FilesDto>>>(Modules + "GetFileData?filePath=" + path);
        if (data != null && data.StatusCode == 200)
        {
            return data.Data!;
        }
        return new List<FilesDto>();
    }
    public async Task<ModelStateResult<bool>> DeleteFile(string path)
    {
        var data = await _httpHelp.DeleteAsync<ModelStateResult<bool>>(Modules + "DeleteFile?filePath=" + path);
        return data;
    }
    public async Task<ModelStateResult<bool>> DeleteDirectory(string path)
    {
        var data = await _httpHelp.DeleteAsync<ModelStateResult<bool>>(Modules + "DeleteDirectory?directoryPath=" + path);
        return data;
    }
    public async Task<ModelStateResult<bool>> UpdateNameDirectory(string path, string newName)
    {
        var data = await _httpHelp.PutAsync<ModelStateResult<bool>>(Modules + "UpdateNameDirectory?directoryPath=" + path + "&name=" + newName, "");
        return data;
    }
    public async Task<ModelStateResult<bool>> UpdateNameFile(string path, string newName)
    {
        var data = await _httpHelp.PutAsync< ModelStateResult<bool>>(Modules + "UpdateNameFile?name=" + newName + "&filePath=" + path,"");
        return data;
    }
}
