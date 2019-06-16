using System.IO;

namespace Language.Convert
{
    public class ExcelFileInfo
    {
        public string FullPath { get; protected set; }
        public string Suffix { get; protected set; }
        public string FileNameNoSuffix { get; set; }
        public string RelativePath { get; protected set; }
        public string RelativeDirectoryPath { get; protected set; }

        public ExcelFileInfo(string fullPath, string srcFolderHasDiagonal)
        {
            FullPath = fullPath;
            FileNameNoSuffix = Path.GetFileNameWithoutExtension(fullPath);
            Suffix = Path.GetExtension(fullPath);
            RelativePath = fullPath.Replace("\\", "/").Replace(srcFolderHasDiagonal, string.Empty);
            RelativeDirectoryPath = Path.GetDirectoryName(RelativePath);
        }
    }
}