using System.IO;
using OfficeOpenXml;
using System.Collections.Generic;
using System.Collections;
using Language.Util;

namespace Language.Reader
{
    public class EPPlusReader : IExcelReader
    {
        private int _sheetIndex;

        private string _filePath;

        private ExcelPackage _excelPackage;

        private List<ExcelWorksheet> _sheetRef;

        private EPPlusSheetReader _settingSheet;
        private EPPlusSheetReader _sheetReader;



        #region public


        public EPPlusSheetReader SettingSheet => _settingSheet;

        public string FullFilePath => _filePath;

        public int SheetCount => _sheetRef.Count;

        public string ExcelName
        {
            get
            {
                if (!string.IsNullOrEmpty(_filePath))
                {
                    return Path.GetFileName(_filePath);
                }
                return string.Empty;
            }
        }

        #endregion


        public EPPlusReader()
        {
            _sheetIndex = -1;
            _sheetRef = new List<ExcelWorksheet>();
            _sheetReader = new EPPlusSheetReader();
            _settingSheet = new EPPlusSheetReader();
        }

        public bool Load(string filePath)
        {
            _filePath = filePath;

            if (string.IsNullOrEmpty(_filePath))
            {
                Log.e($"找不到该文件{filePath}");
                return false;
            }

            if (_excelPackage != null)
            {
                _excelPackage.Dispose();
                _excelPackage = null;
                _sheetRef.Clear();
            }

            try
            {
                _excelPackage = new ExcelPackage(new FileStream(_filePath,
                                                                FileMode.Open,
                                                                FileAccess.Read,
                                                                FileShare.ReadWrite));

                for (int i = 0; i < _excelPackage.Workbook.Worksheets.Count; i++)
                {
                    var sheet = _excelPackage.Workbook.Worksheets[i];
                    if (sheet.Name == "_SETTING_")
                    {
                        _settingSheet.ResetSheet(sheet);
                        continue;
                    }
                    if (sheet.Dimension != null)
                        _sheetRef.Add(sheet);
                }

                _sheetIndex = -1;
            }
            catch (System.Exception e)
            {
                Log.e($"加载失败 {_filePath}\n{e}");
                return false;
            }

            return true;
        }

        public bool MoveNext()
        {
            _sheetIndex++;
            return (_sheetIndex < _sheetRef.Count);
        }

        public void Reset()
        {
            _sheetIndex = -1;
        }

        public IEnumerator GetEnumerator()
        {
            return this;
        }

        public void Dispose()
        {

        }

        public object Current
        {
            get
            {
                try
                {
                    if (_sheetRef.Count <= 0)
                        return null;

                    var sheetData = _sheetRef[_sheetIndex];
                    if (sheetData == null)
                        throw new System.NullReferenceException(string.Format("[{0}] => ({1})有空的Sheet",
                                                                                FullFilePath,
                                                                                _sheetIndex));
                    _sheetReader.ResetSheet(sheetData);
                    return _sheetReader;
                }
                catch (System.IndexOutOfRangeException)
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        ISheetReader IEnumerator<ISheetReader>.Current
        {
            get
            {
                try
                {
                    if (_sheetRef.Count <= 0)
                        return null;

                    var sheetData = _sheetRef[_sheetIndex];
                    if (sheetData == null)
                        throw new System.NullReferenceException(string.Format("[{0}] => ({1})有空的Sheet",
                                                                                FullFilePath,
                                                                                _sheetIndex));
                    _sheetReader.ResetSheet(sheetData);
                    return _sheetReader;
                }
                catch (System.IndexOutOfRangeException)
                {
                    throw new System.InvalidOperationException();
                }
            }
        }
    }
}