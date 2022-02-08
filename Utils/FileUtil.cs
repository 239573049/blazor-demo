using System.Drawing;

namespace Utils
{
    public class FileUtil
    {
        /// <summary>
        /// 获取文件转换类型
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static string GetSizeFormat(long? b)
        {
            const int GB = 1024 * 1024 * 1024;
            const int MB = 1024 * 1024;
            const int KB = 1024;
            if (b / GB >= 1)
            {
                return Math.Round(b??0 / (float)GB, 2) + "GB";
            }
            if (b / MB >= 1)
            {
                return Math.Round(b ?? 0 / (float)MB, 2) + "MB";
            }
            if (b / KB >= 1)
            {
                return Math.Round(b ?? 0 / (float)KB, 2) + "KB";
            }
            return b + "B";
        }
    }
}
