using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.Navigation.Forms
{
    public class CollectForm
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Link { get; set; }

        public int GroupId { get; set; }

        public byte Position { get; set; }
    }

    public class CollectBatchForm
    {
        public CollectGroupForm[] Data { get; set; }
    }

    public class CollectGroupForm
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public byte Position { get; set; }

        public CollectForm[] Items { get; set; }
    }
}
