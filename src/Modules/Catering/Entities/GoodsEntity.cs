using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Catering.Entities;
public class GoodsEntity : IIdEntity, ITimestampEntity
{
    public int Id { get; set; }
    public int StoreId { get; set; }
    public int CatId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public int RecipeId { get; set; }
    public string Description { get; set; } = string.Empty;
    public int UpdatedAt { get; set; }
    public int CreatedAt { get; set; }
    
}