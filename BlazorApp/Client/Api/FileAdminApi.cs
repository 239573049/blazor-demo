﻿using AntDesign;
using BlazorApp.Client.Configuration;
using BlazorHelper;
using Entitys.File;
using Entitys.Web;
using Newtonsoft.Json;

namespace BlazorApp.Client.Api;
public class FileAdminApi
{
    private readonly HttpHelper _httpHelp;
    private readonly MessageService _messageService;
    public FileAdminApi(HttpHelper httpHelp,
        MessageService messageService
        )
    {
        _httpHelp = httpHelp;
        _messageService= messageService;
    }
    public async Task<List<FilesDto>> GetFileData(string path)
    {
        var data = await _httpHelp.GetAsync<ModelStateResult<List<FilesDto>>>(ApiConfiguration.FileApi + "GetFileData?filePath=" + path);
        if (data != null && data.StatusCode == 200)
        {
            return data.Data!;
        }
        return new List<FilesDto>();
    }
    public async Task<ModelStateResult<bool>> DeleteFile(string path)
    {
        var data = await _httpHelp.DeleteAsync<ModelStateResult<bool>>(ApiConfiguration.FileApi + "DeleteFile?filePath=" + path);
        return data;
    }
    public async Task<ModelStateResult<bool>> DeleteDirectory(string path)
    {
        var data = await _httpHelp.DeleteAsync<ModelStateResult<bool>>(ApiConfiguration.FileApi  + "DeleteDirectory?directoryPath=" + path);
        return data;
    }
    public async Task<ModelStateResult<bool>> UpdateNameDirectory(string path, string newName)
    {
        var data = await _httpHelp.PutAsync<ModelStateResult<bool>>(ApiConfiguration.FileApi  + "UpdateNameDirectory?directoryPath=" + path + "&name=" + newName, "");
        return data;
    }
    public async Task<ModelStateResult<bool>> UpdateNameFile(string path, string newName)
    {
        var data = await _httpHelp.PutAsync< ModelStateResult<bool>>(ApiConfiguration.FileApi  + "UpdateNameFile?name=" + newName + "&filePath=" + path,"");
        return data;
    }
    public async Task<string?> GetFileContent(string filePath)
    {
        var data = await _httpHelp.GetAsync<ModelStateResult<string>>(ApiConfiguration.FileApi + "GetFileContent?filePath=" + filePath);
        if (data.StatusCode == 200)
        {
            return data.Data;
        }
        await _messageService.Warning(data.Message);
        return null;
    }
    public async Task<bool> UpdateFileContent(string filePath, string content)
    {
        var data = await _httpHelp.PostAsync<ModelStateResult<bool>>(ApiConfiguration.FileApi + "UpdateFileContent?filePath=" + filePath, content);
        if(data.StatusCode == 200)
        {
            await _messageService.Success("编辑成功");
            return true;
        }
        await _messageService.Warning(data.Message);
        return false;
    }
}
