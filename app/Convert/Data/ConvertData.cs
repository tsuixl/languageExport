using System.Collections.Generic;

namespace Language.Convert
{

    public struct ConvertFieldData
    {
        public FieldType Type;
        public string Comments;
        public string FieldName;
        public string Value;
    }

    public class ConvertData
    {
        public string FileName;
        public List<List<ConvertFieldData>> Datas;

        public ConvertData (string name)
        {
            FileName = name;
            Datas = new List<List<ConvertFieldData>>();
        }

        public void AddLine ()
        {
            Datas.Add(new List<ConvertFieldData>());
        }


        public void AddCol (int line, ConvertFieldData fieldData)
        {
            if (line >= 0 && line < Datas.Count)
            {
                Datas[line].Add(fieldData);
            }
        }


        public void AddLastCol (ConvertFieldData fieldData)
        {
            Datas[Datas.Count - 1].Add(fieldData);
        }

    }
}