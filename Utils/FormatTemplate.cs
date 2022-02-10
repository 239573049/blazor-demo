namespace Utils;

public class FormatTemplate
{
    /// <summary>
    /// 获取图片格式
    /// </summary>
    /// <returns></returns>
    public static string[] ImageType
    {
        get
        {
            return new string[] { ".png", ".xbm", ".tif", ".svgz", ".jpg", ".jpeg", ".ico", ".tiff", ".gif", ".svg", ".jfif", ".webp", ".bmp", "pjpeg", ".avif" };
        }
    }
    /// <summary>
    /// 获取可编辑文本格式
    /// </summary>
    /// <returns></returns>
    public static string[] TextType
    {
        get
        {
            return new string[] { ".txt", ".log", ".config", ".json", ".cs", ".sh", ".bat", ".vbs", ".ini", ".gitconfig", ".xml", ".html", ".css", ".js", ".md" };
        }
    }
}
