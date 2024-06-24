using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Shared.Repositories.Models
{
    public interface ILanguageEntity: IIdEntity
    {
        public string Language { get; }
    }
}
