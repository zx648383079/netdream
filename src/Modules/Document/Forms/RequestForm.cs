using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.Document.Forms
{
    public class RequestForm
    {
        [Required]
        public string Url { get; set; }
        public string Method { get; set; }
        public string Type { get; set; }
        public string RawType { get; set; }
        public string Header { get; set; }
        public string Body { get; set; }
    }
}
