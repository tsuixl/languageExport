namespace Language.Convert
{
    public enum FieldType
    {
        None,
        Int,
        Float,
        String
    }

    
    public static class FieldTypeUtil 
    {
        public static FieldType String2Type (string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                str = str.ToLower();
                switch (str)
                {
                    case "int":
                    case "long":
                        return FieldType.Int;

                    case "float":
                    case "double":
                        return FieldType.Float;

                    case "string":
                        return FieldType.String;
                }
            }
            return FieldType.None;
        }
    }


}