using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto
{
    /// <summary>
    /// 文件模型
    /// </summary>
    public class FilesDto
    {
        /// <summary>
        /// 文件名称
        /// </summary>
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
        public string? Root { get; set; }
        /// <summary>
        /// 是否文件
        /// </summary>
        public bool IsFile { get; set; }
    }

}
