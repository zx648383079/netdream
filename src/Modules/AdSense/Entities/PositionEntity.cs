using NetDream.Core.Interfaces.Entities;
using NPoco;
namespace Modules.AdSense.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class PositionEntity : IIdEntity, ITimestampEntity
    {
        internal const string ND_TABLE_NAME = "ad_position";
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        [Column("auto_size")]
        public byte AutoSize { get; set; }
        [Column("source_type")]
        public byte SourceType { get; set; }
        public string Width { get; set; } = string.Empty;
        public string Height { get; set; } = string.Empty;
        public string Template { get; set; } = string.Empty;
        public byte Status { get; set; }
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }

        public PositionEntity()
        {
            
        }

        public PositionEntity(string code, string name)
        {
            Code = code;
            Name = name;
        }
    }
}
