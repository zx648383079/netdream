using NetDream.Shared.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.OnlineDisk.Forms
{
    public class DiskFolderForm
    {
        [Required]
        public string Name { get; set; }

        public string Parent { get; set; }
    }

    public class DiskFileForm
    {
        [Required]
        public string Name { get; set; }

        public string Parent { get; set; }
        [Required]
        public string Md5 { get; set; }

        public IUploadFile File { get; set; }
    }

    public class DiskChunkForm
    {
        [Required]
        public string Name { get; set; }

        public IUploadFile File { get; set; }
    }

    public class DiskChunkFinishForm
    {
        [Required]
        public string Name { get; set; }

        public string Parent { get; set; }
        [Required]
        public string Md5 { get; set; }

        public string[] Files { get; set; }
    }

    public class DiskCheckForm
    {
        [Required]
        public string Name { get; set; }

        public string Parent { get; set; }
        [Required]
        public string Md5 { get; set; }
    }

    public class DiskRenameForm
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Id { get; set; }
    }

    public class DiskMoveForm
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string Parent { get; set; }
    }
}
