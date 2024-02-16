namespace ICP.Host.ApiTest.Models
{
    public class File
    {
        public string UploadName { get; set; }

        public string FileName { get; set; }

        public System.IO.Stream InputStream { get; set; }

        public string ContentType { get; set; }
    }
}