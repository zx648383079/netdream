using NetDream.Modules.SEO.Entities;
using NetDream.Shared.Repositories.Models;
using NPoco;
using System.Collections.Generic;

namespace NetDream.Modules.SEO.Models
{
    public class AgreementModel: AgreementEntity
    {
        [Ignore]
        public IList<ILanguageFormatted> Languages { get; set; } = [];
    }
}
