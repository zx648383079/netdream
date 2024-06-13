namespace NetDream.Core.Interfaces.Entities
{
    public interface ICreatedEntity
    {
        public int CreatedAt { get; set; }
    }
    public interface ITimestampEntity: ICreatedEntity
    {
        public int UpdatedAt { get; set; }
    }
}
