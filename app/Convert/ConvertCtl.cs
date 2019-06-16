using System.IO;
using System.Collections.Generic;
using Language.Util;

namespace Language.Convert
{
    public class ConvertCtl
    {
        public string SrcFolderPath { get; private set; }
        public string DesFolderPath { get; private set; }
        public string SingleFilePath { get; private set; }

        protected AppArgs Args { get; private set; }

        private ConvertHandleBase _converHandle;

        public ConvertCtl(AppArgs appArgs, ConvertHandleBase converHandle)
        {
            Args = appArgs;
            _converHandle = converHandle;
        }


        public void Start()
        {
            if (Args == null)
                return;

            InitPath();

            var files = GetFiles();
            if (files.Count == 0)
            {
                SystemUtil.Kill("Excel 数量为0");
            }
            else
            {
                _converHandle.Convert(files, Args);
            }
        }


        private void InitPath()
        {
            SrcFolderPath = Args.Src.Replace("\\", "/");
            DesFolderPath = Args.Out.Replace("\\", "/");
            if (!string.IsNullOrEmpty(Args.SrcFile))
                SingleFilePath = Args.SrcFile.Replace("\\", "/");

            if (!SrcFolderPath.EndsWith("/"))
                SrcFolderPath += "/";
            if (!DesFolderPath.EndsWith("/"))
                DesFolderPath += "/";

             if (!Directory.Exists(SrcFolderPath))
                Log.e($"{SrcFolderPath} 找不到该目录!", true);
            

            if (Args.ClearOutput)
            {
                if (Directory.Exists(DesFolderPath))
                {
                    Directory.Delete (DesFolderPath, true);
                }
                Directory.CreateDirectory(DesFolderPath);
            }
            else
            {
                if (!Directory.Exists(DesFolderPath))
                    Directory.CreateDirectory(DesFolderPath);
            }

        }


        private List<ExcelFileInfo> GetFiles()
        {
            List<ExcelFileInfo> result = new List<ExcelFileInfo>();
            if (!string.IsNullOrEmpty(SingleFilePath))
            {
                if (File.Exists(SingleFilePath))
                    result.Add(new ExcelFileInfo(SingleFilePath, SrcFolderPath));
                else
                    Log.e($"{SingleFilePath} 找不到该文件!", true);
            }
            else
            {
                var files = Directory.GetFiles(SrcFolderPath, "*.*", SearchOption.AllDirectories);
                foreach (var f in files)
                {
                    if (f.Contains("~$"))
                        continue;
                    var suffix = Path.GetExtension(f).ToLower();
                    if (suffix.Contains("xlsx"))
                    {
                        result.Add(new ExcelFileInfo(f, SrcFolderPath));
                    }
                }
            }
            return result;
        }

    }
}