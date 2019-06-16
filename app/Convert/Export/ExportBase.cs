using System.Collections.Generic;

namespace Language.Convert
{
    public abstract class ExportBase
    {
        public abstract void Save(string outPath, ExcelFileInfo fileInfo, List<ConvertData> datas);



        public string GetOutputPath(string outPath, ExcelFileInfo fileInfo)
        {
            var path = outPath + "/" + fileInfo.RelativeDirectoryPath;
            if (!path.EndsWith("/"))
                path += "/";
            path = path.Replace("\\", "/");
            return path;
        }

        public string GetOutputFilePath(string outPath, ExcelFileInfo fileInfo, ConvertData data)
        {
            var path = outPath + "/" + fileInfo.RelativeDirectoryPath;
            if (!path.EndsWith("/"))
                path += "/";
            path = path.Replace("\\", "/");
            return string.Format("{0}{1}_{2}.cs", path, fileInfo.FileNameNoSuffix, data.FileName);
        }
    }
}