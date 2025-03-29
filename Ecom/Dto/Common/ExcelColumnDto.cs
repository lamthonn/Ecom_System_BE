namespace Ecom.Dto.Common
{
    public class ExcelColumnDto
    {
        public string DisplayName { get; set; } // Tên hiển thị trên Excel (tiêu đề cột)
        public string PropertyName { get; set; } // Tên thuộc tính trong entity
        public string? Format { get; set; } // Định dạng (ví dụ: "dd/MM/yyyy HH:mm:ss" cho ngày tháng)
        public string? BoolTrueValue { get; set; } 
        public string? BoolFalseValue { get; set; }
        public ExcelColumnDto(string displayName, string propertyName, string? format = null, string? boolTrueValue = null, string? boolFalseValue = null)
        {
            DisplayName = displayName;
            PropertyName = propertyName;
            Format = format;
            BoolTrueValue = boolTrueValue;
            BoolFalseValue = boolFalseValue;
        }
    }
}
