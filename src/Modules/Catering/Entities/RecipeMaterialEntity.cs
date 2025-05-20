using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Catering.Entities;
public class RecipeMaterialEntity : IIdEntity
{
    public int Id { get; set; }
    public int RecipeId { get; set; }
    public int MaterialId { get; set; }
    public float Amount { get; set; }
    public byte Unit { get; set; }
    
}