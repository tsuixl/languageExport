using System.Collections.Generic;
using System.IO;

public class AppArgs
{
    public bool Jenkins { get; private set; }

    public string Src { get; private set; }

    public string Out { get; private set; }

    public string SrcFile { get; private set; }

    public bool ClearOutput { get; private set; } = true;


    public AppArgs(string[] args)
    {
        if (args != null)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            var idx = 0;
            while (idx < args.Length)
            {
                if (idx < args.Length - 1)
                    dic.Add(args[idx], args[idx + 1]);
                idx += 2;
            }

            foreach (var item in dic)
            {
                Set(item.Key, item.Value);
            }
        }
    }


    private void Set(string key, string value)
    {

        if (key.Contains("jenkins"))
        {
            Jenkins = value.ToLower() == "true";
        }
        else if (key.Contains("src"))
        {
            Src = GetPath(value);
        }
        else if (key.Contains("out"))
        {
            Out = GetPath(value);
        }
        // else if (key.Contains("src_file"))
        // {
        //     SrcFile = GetPath(value);
        // }
        else if (key.Contains("clear_out"))
        {
            ClearOutput = value.ToLower() == "true";
        }
    }


    public static string GetPath(string path)
    {
        if (path.StartsWith("../"))
        {
            return string.Format("{0}/{1}", Directory.GetCurrentDirectory(), path);
        }
        else if (path.StartsWith("/.."))
        {
            return string.Format("{0}{1}", Directory.GetCurrentDirectory(), path);
        }
        return path;
    }

    public static string ArgTips()
    {
        return
        "参数设置:\n" +
        "jenkins            在Jenkins中显示有颜色的日志.\n" +
        "src                Excel目录.\n" +
        "out                输出目录.\n" +
        // "src_file           导出单个文件,src不在处理.\n" +
        "clear_out          清空输出目录.\n"
        ;
    }


}