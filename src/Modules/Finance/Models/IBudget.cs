namespace NetDream.Modules.Finance.Models
{
    public interface IBudget
    {
        public int Id { get; }
        public float Budget { get; }
        public byte Cycle { get; }

        public float Spent { set; }

        public int UpdatedAt { get; }
    }
}
