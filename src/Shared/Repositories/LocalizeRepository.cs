using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Shared.Repositories
{
    public class LocalizeRepository(IClientContext environment) : ILocalizeRepository
    {
        readonly Dictionary<string, string> LANGUAGE_MAP = new()
        {
            {"zh", "中"},
            {"en", "EN"},
        };

        public string Default => LANGUAGE_MAP.Keys.First();

        public string Language => environment.Language;

        public string[] Keys => [.. LANGUAGE_MAP.Keys];

        public IOptionItem<string>[] Items => LANGUAGE_MAP.Select(i => new LanguageFormatted(i.Value, i.Key)).ToArray();

    }
}
