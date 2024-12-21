
namespace NetDream.Modules.Shop.Entities
{
    
    public class GroupBuyLogEntity
    {
        
        public int Id { get; set; }
        
        public int ActId { get; set; }
        
        public int UserId { get; set; }
        
        public int OrderId { get; set; }
        
        public int OrderGoodsId { get; set; }
        public decimal Deposit { get; set; }
        
        public decimal FinalPayment { get; set; }
        public int Status { get; set; }
        
        public int PredeterminedAt { get; set; }
        
        public int FinalAt { get; set; }
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
