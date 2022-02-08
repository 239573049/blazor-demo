using System.ComponentModel;

namespace Entitys.File
{
    /// <summary>
    /// 文件模型
    /// </summary>
    public class FilesDto
    {
        /// <summary>
        /// 文件名称
        /// </summary>
        [DisplayName("文件名")]
        public string? Name { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public long? Length { get; set; }
        /// <summary>
        /// 完整路径
        /// </summary>
        public string? Path { get; set; }
        /// <summary>
        /// 权限信息
        /// </summary>
        [DisplayName("权限")]
        public string? Root { get; set; }
        /// <summary>
        /// 是否文件
        /// </summary>
        public bool IsFile { get; set; }
        [DisplayName("文件大小")]
        public string? LengthName { get; set; }
    }

}
