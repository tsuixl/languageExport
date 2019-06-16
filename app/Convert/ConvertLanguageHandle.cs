using System.Text;
using Language.Util;
using System.Collections.Generic;

namespace Language.Convert
{
    public class ConvertLanguageHandle : ConvertHandleBase
    {
        private string _excelName;
        List<ConvertData> converDatas = new List<ConvertData>();

        protected override List<ConvertData> GetDatas()
        {
            return converDatas;
        }

        protected override void OnLaodSheetOver()
        {
 
        }

        protected override void OnLoadExcelOver()
        {
            Log.d($"LoadFinish {_excelName}\n", System.ConsoleColor.DarkGreen);
        }

        protected override void OnLoadExcelStart(IExcelReader excelReader)
        {
            _excelName = excelReader.ExcelName;
            Log.d($"Laod {_excelName}", System.ConsoleColor.DarkGreen);
        }

        protected override void OnLoadLine(string[] lineData)
        {
            for (int i = 0; i < converDatas.Count; i++)
            {
                var lineIndex = i + Main_Key_Index + 1;
                var converData = converDatas[i];
                if (converData != null)
                {
                    converData.AddLine();

                    // id
                    converData.AddLastCol(new ConvertFieldData()
                    {
                        Type = FieldTypeUtil.String2Type(SettingDatas[SettingType.TYPE][Main_Key_Index]),
                        FieldName = SettingDatas[SettingType.FIELD][Main_Key_Index],
                        Value = lineData[Main_Key_Index],
                        Comments = string.Empty
                    });

                    // 语言
                    converData.AddLastCol(new ConvertFieldData()
                    {
                        Type = FieldTypeUtil.String2Type(SettingDatas[SettingType.TYPE][lineIndex]),
                        FieldName = SettingDatas[SettingType.FIELD][lineIndex],
                        Value = lineData[lineIndex],
                        Comments = string.Empty
                    });
                }
            }

        }

        protected override void OnLoadSheetStart(ISheetReader sheetReader)
        {
            InitType();
        }

        private void InitType()
        {
            converDatas.Clear();
            var types = SettingDatas[SettingType.TYPE];
            var fields = SettingDatas[SettingType.FIELD];
            for (int i = 0; i < types.Length; i++)
            {
                if (i <= Main_Key_Index)
                    continue;
                var t = types[i];
                var f = fields[i];
                if (string.IsNullOrEmpty(t) || string.IsNullOrEmpty(f))
                {
                    converDatas.Add(null);
                    continue;
                }

                converDatas.Add(new ConvertData($"{f}"));
            }
        }

    }
}