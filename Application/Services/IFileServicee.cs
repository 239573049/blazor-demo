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


    }
    public class FiFileSipub : IFileService
    {
        public bool IsExitFile(string fille)
        {
            return File.Exists(fille);
        }
    }
}
