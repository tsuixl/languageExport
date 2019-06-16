
using System.Collections;
using System.Collections.Generic;
namespace Language
{
    public interface IExcelReader : IEnumerator, IEnumerable, IEnumerator<ISheetReader>
    {

        int SheetCount {get;}

        string ExcelName {get;}

        string FullFilePath { get; }      

        bool Load (string filePath);  
    }
}