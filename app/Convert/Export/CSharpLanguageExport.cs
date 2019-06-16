using Language.Util;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace Language.Convert
{
    public class CSharpLanguageExport : ExportBase
    {
        public override void Save(string outPath, ExcelFileInfo fileInfo, List<ConvertData> datas)
        {
            List<string> className = new List<string>();
            List<string> languageTypes = new List<string>();
            foreach (var data in datas)
            {
                var savePath = GetOutputFilePath(outPath, fileInfo, data);
               

                StringBuilder sb = new StringBuilder(1024);

                foreach (var lineData in data.Datas)
                {
                    var temp = CSharpLanguageExportTemplate.ContentTemp;
                    sb.AppendLine(string.Format(temp, lineData[0].Value, lineData[1].Value));
                }

                var name = $"{fileInfo.FileNameNoSuffix}_{data.FileName}";

                var classTemp = CSharpLanguageExportTemplate.ClassTemp;
                classTemp = classTemp.Replace("$CLASS_NAME$", name);
                classTemp = classTemp.Replace("$CONTENT$", sb.ToString());

                className.Add(name);
                languageTypes.Add(data.FileName);

                File.WriteAllText(savePath, classTemp);

                 Log.d($"[CSharp] save {savePath}");
            }

            // load
            var loadClassStr = CSharpLanguageExportTemplate.LoadClassTemp;
            var loadClassContentStr = CSharpLanguageExportTemplate.LoadClassContentTemp;
            StringBuilder LOAD_TYPE = new StringBuilder(64);
            StringBuilder LOAD_CONTENT = new StringBuilder(128);

            var outputPath = GetOutputPath(outPath, fileInfo);

            for (int i = 0; i < className.Count; i++)
            {
                var name = className[i];
                var type = languageTypes[i];

                LOAD_TYPE.AppendLine($"\t\t{type},");
                LOAD_CONTENT.AppendLine(string.Format(loadClassContentStr, type, name));
            }

            loadClassStr = loadClassStr.Replace("$LOAD_TYPE$", LOAD_TYPE.ToString());
            loadClassStr = loadClassStr.Replace("$LOAD_CONTENT$", LOAD_CONTENT.ToString());
            File.WriteAllText(outputPath + "LanguageLoad.cs", loadClassStr);
        }

    }


    public class CSharpLanguageExportTemplate
    {
        private static string _class;
        private static string _content;

        private static string _loadClass;
        private static string _loadClassContent;


        public static string LoadClassTemp
        {
            get
            {
                if (string.IsNullOrEmpty(_loadClass))
                {
                    _loadClass = File.ReadAllText("Template/CSharpLanguage/LoadClass.txt");
                }
                return _loadClass;
            }
        }


        public static string LoadClassContentTemp
        {
            get
            {
                if (string.IsNullOrEmpty(_loadClassContent))
                {
                    _loadClassContent = File.ReadAllText("Template/CSharpLanguage/LoadClass_Content.txt");
                }
                return _loadClassContent;
            }
        }


        public static string ClassTemp
        {
            get
            {
                if (string.IsNullOrEmpty(_class))
                {
                    _class = File.ReadAllText("Template/CSharpLanguage/Class.txt");
                }
                return _class;
            }
        }


        public static string ContentTemp
        {
            get
            {
                if (string.IsNullOrEmpty(_content))
                {
                    _content = File.ReadAllText("Template/CSharpLanguage/Content.txt");
                }
                return _content;
            }
        }

    }
}