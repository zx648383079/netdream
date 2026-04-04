using NetDream.Shared.Interfaces;

namespace NetDream.Shared.Repositories.Models
{
    public interface ILanguageEntity: IIdEntity
    {
        public string Language { get; }
    }
}
