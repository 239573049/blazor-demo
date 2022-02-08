using Entitys.File;
using System.Net.Http.Json;

namespace BlazorApp.Client.Api
{

    public class FileAdminApi: Request
    {
        public static string Modules = "/api/File/";
        public static async Task<List<FilesDto>> GetFileData(string path)
        {
            var data = await Http.GetFromJsonAsync<List<FilesDto>>(Modules+"GetFileData?filePath=" + path);
            return data??new List<FilesDto>();
        }
        public static async Task<bool> DeleteFile(string path)
        {
            var data= await Http.DeleteAsync(path);
            return await data.Content.ReadFromJsonAsync<bool>();
        }
    }
}
