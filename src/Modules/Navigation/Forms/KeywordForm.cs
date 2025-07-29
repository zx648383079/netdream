using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.Navigation.Forms
{
    public class KeywordForm
    {
        public int Id { get; set; }
        [Required]
        public string Word { get; set; }

        public byte Type { get; set; }
    }
}
