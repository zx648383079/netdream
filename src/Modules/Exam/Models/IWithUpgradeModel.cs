using NetDream.Modules.Exam.Entities;

namespace NetDream.Modules.Exam.Models
{
    public interface IWithUpgradeModel
    {
        public int UpgradeId { get; }
        public UpgradeEntity? Upgrade { set; }
    }
}