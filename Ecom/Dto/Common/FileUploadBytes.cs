namespace backend_v3.Dto.Common
{
    public class FileUploadBytes
    {
        public byte[]? content { get; set; }
        public string? contentBase64 { get; set; }
        public string? fileName { get; set; }
        public string? contentType { get; set; }
    }
}
