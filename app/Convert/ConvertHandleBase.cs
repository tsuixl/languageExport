using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Language.Reader;
using Language.Util;

namespace Language.Convert
{

    public enum LineType
    {
        None,
        No,
        End,
        End_Before
    }


    public enum SettingType
    {
        TYPE,
        FIELD,
        MAIN_KEY_INDEX
    }


    public abstract class ConvertHandleBase
    {
        private EPPlusReader _excelReader;
        private Dictionary<string, string> _setting;


        public int StartLine { get; private set; } = 4;
        public int TypeLine { get; private set; } = 1;
        public int FieldLine { get; private set; } = 2;
        public int Main_Key_Index { get; private set; } = 0;
        public string ExportType = "";

        protected Dictionary<SettingType, string[]> SettingDatas;

        private ExportBase _export;

        private HashSet<string> keys;

        public ConvertHandleBase()
        {
            keys = new HashSet<string>();
            _excelReader = new EPPlusReader();
            _setting = new Dictionary<string, string>();
            SettingDatas = new Dictionary<SettingType, string[]>();
        }

        public void Convert(List<ExcelFileInfo> files, AppArgs appArgs)
        {
            foreach (var f in files)
            {
                if (_excelReader.Load(f.FullPath))
                {
                    // 加载设置
                    if (_excelReader.SettingSheet == null)
                    {
                        Log.e($"{f},_SETTING_分页找不到!");
                        continue;
                    }

                    LoadSetting(_excelReader.SettingSheet);

                    OnLoadExcelStart(_excelReader);
                    foreach (ISheetReader sheetReader in _excelReader)
                    {
                        ResetSettingDatas (sheetReader);
                        OnLoadSheetStart(sheetReader);
                        LoadSheet(sheetReader);
                        OnLaodSheetOver();
                    }
                    OnLoadExcelOver();

                    var datas = GetDatas();
                    // Log.d (Newtonsoft.Json.JsonConvert.SerializeObject(datas, Newtonsoft.Json.Formatting.Indented));
                    _export.Save(appArgs.Out, f, datas);
                    
                }
            }
        }


        private void LoadSheet(ISheetReader sheetReader)
        {
            int line = StartLine;
            while (true)
            {
                var lineData = sheetReader.GetLineCell(line);
                line ++;
                if (lineData == null)
                    break;

                var type = GetLineType(lineData);
                if (type == LineType.End_Before)
                    break;

                if (type == LineType.No)
                    continue;

                if (keys.Contains(lineData[Main_Key_Index]))
                {
                    Log.e ($"{_excelReader.ExcelName}.{sheetReader.GetSheetName()} 主键重复 {lineData[0]}", true);
                    return;
                }
                keys.Add (lineData[Main_Key_Index]);
                OnLoadLine(lineData);

                if (type == LineType.End)
                    break;
            }
        }


        private void LoadSetting(ISheetReader settingSheet)
        {
            int line = 1;
            while (true)
            {
                line ++;
                var lineData = settingSheet.GetLineCell(line);

                if (lineData == null)
                    break;

                var type = GetLineType(lineData);
                if (type == LineType.End_Before)
                    break;

                if (type == LineType.No)
                    continue;

                SetSetting(lineData[1], lineData[2]);

                if (type == LineType.End)
                    break;
            }
        }


        protected abstract void OnLoadExcelStart(IExcelReader excelReader);

        protected abstract void OnLoadExcelOver();

        protected abstract void OnLoadSheetStart(ISheetReader sheetReader);

        protected abstract void OnLoadLine(string[] lineData);

        protected abstract void OnLaodSheetOver();


        protected abstract List<ConvertData> GetDatas ();


        private void SetSetting(string key, string value)
        {
            if (key == "START_LINE")
            {
                StartLine = TryParseInt (value, 4);
            }
            else if (key == "TYPE")
            {
                TypeLine = TryParseInt(value, 1);
            }
            else if (key == "FIELD")
            {
                FieldLine = TryParseInt(value, 2);
            }
            else if (key == "EXPORT_TYPE")
            {
                ExportType = value;
                Assembly assembly = Assembly.GetExecutingAssembly();
                _export = assembly.CreateInstance("Language.Convert." + value) as ExportBase;
            }
            else if (key == "MAIN_KEY_INDEX")
            {
                Main_Key_Index = TryParseInt(value, 0);
            }
        }


        private void ResetSettingDatas (ISheetReader sheetReader)
        {
            SettingDatas.Clear();
            SettingDatas.Add((SettingType)System.Enum.Parse(typeof(SettingType), "TYPE"), sheetReader.GetLineCell(TypeLine));
            SettingDatas.Add((SettingType)System.Enum.Parse(typeof(SettingType), "FIELD"), sheetReader.GetLineCell(FieldLine));
        }


        private int TryParseInt(string str, int defaultValue)
        {
            int temp = 0;
            if (int.TryParse(str, out temp))
                return temp;
            return defaultValue;
        }


        protected LineType GetLineType(string[] lineData)
        {
            if (lineData != null && lineData.Length > 0)
            {
                var type = lineData[0].ToLower();
                if (type.StartsWith("no"))
                    return LineType.No;
                if (type.StartsWith("end"))
                    return LineType.End;
                if (type.StartsWith("end_before"))
                    return LineType.End_Before;
            }
            return LineType.None;
        }

    }
}