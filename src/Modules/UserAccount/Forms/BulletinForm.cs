using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.UserAccount.Forms
{
    public class BulletinForm
    {
        [Required]
        public int User { get; set; }

        public string Title { get; set; } = "消息";
        [Required]
        public string Content { get; set; } = string.Empty;
    }
}
