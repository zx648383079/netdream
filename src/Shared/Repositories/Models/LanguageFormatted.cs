using NetDream.Shared.Models;

namespace NetDream.Shared.Repositories.Models
{
    public class LanguageFormatted: OptionItem<string>, ILanguageFormatted
    {
        public int Id {  get; set; }

        public LanguageFormatted(string name, string value): base(name, value) 
        {
        
        }

        public LanguageFormatted(string name, string value, int id) : this(name, value)
        {
            Id = id;
        }
    }
}
