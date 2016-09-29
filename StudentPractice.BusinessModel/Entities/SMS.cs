namespace StudentPractice.BusinessModel
{
    public partial class SMS
    {
        public string[] FieldValues
        {
            get { return FieldValuesInternal != null ? FieldValuesInternal.Split(new char[] { '#' }) : null; }
            set
            {
                FieldValuesInternal = null;
                if (value != null)
                    foreach (var str in value)
                        FieldValuesInternal += str + "#";
                if (FieldValuesInternal != null)
                    FieldValuesInternal = FieldValuesInternal.Remove(FieldValuesInternal.Length - 1, 1);
            }
        }
    }
}