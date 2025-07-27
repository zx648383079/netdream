namespace NetDream.Modules.Finance.Models
{
    public interface IBudget
    {
        public int Id { get; }
        public decimal Budget { get; }
        public byte Cycle { get; }

        public decimal Spent { set; }

        public int UpdatedAt { get; }
    }
}
