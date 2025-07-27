namespace NetDream.Modules.Finance.Models
{
    public class BudgetListItem : IBudget
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Budget { get; set; }
        public decimal Spent { get; set; }
        public byte Cycle { get; set; }
        public int UpdatedAt { get; set; }
        public int CreatedAt { get; set; }

        public decimal Remain => Budget - Spent;
    }
}
