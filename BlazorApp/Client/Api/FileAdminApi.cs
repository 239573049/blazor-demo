using Entitys.File;
using System.Net.Http.Json;
using Entitys.Web;
namespace BlazorApp.Client.Api;
public class FileAdminApi
{
    private readonly HttpClient Http;
    public FileAdminApi(HttpClient http)
    {
        Http = http;
    }
    public static string Modules = "/api/File/";
    public async Task<List<FilesDto>> GetFileData(string path)
    {
        var data = await Http.GetFromJsonAsync<ModelStateResult<List<FilesDto>>>(Modules + "GetFileData?filePath=" + path);
        if (data != null && data.StatusCode == 200)
        {
            return data.Data!;
        }
        return new List<FilesDto>();
    }
    public async Task<bool> DeleteFile(string path)
    {
        var data = await Http.DeleteAsync(Modules + "DeleteFile?filePath=" + path);
        return (await data.Content.ReadFromJsonAsync<ModelStateResult<bool>>()).Data;
    }
    public async Task<bool> DeleteDirectory(string path)
    {
        var data = await Http.DeleteAsync(Modules + "DeleteDirectory?directoryPath=" + path);
        return (await data.Content.ReadFromJsonAsync<ModelStateResult<bool>>()).Data;
    }
    public async Task<bool> UpdateNameDirectory(string path, string newName)
    {
        var data = await Http.PutAsJsonAsync(Modules + "UpdateNameDirectory?directoryPath=" + path + "&name=" + newName, "");
        return (await data.Content.ReadFromJsonAsync<ModelStateResult<bool>>()).Data;
    }
}
