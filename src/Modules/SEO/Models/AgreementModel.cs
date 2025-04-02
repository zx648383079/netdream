using NetDream.Modules.SEO.Entities;
using NetDream.Shared.Helpers;
using NetDream.Shared.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace NetDream.Modules.SEO.Models
{
    public class AgreementModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public IList<AgreementGroupItem> Content { get; set; } = [];
        public int Status { get; set; }


        public IList<ILanguageFormatted> Languages { get; set; } = [];

        public DateTime UpdatedAt { get; set; }

        public DateTime CreatedAt { get; set; }

        public AgreementModel()
        {
            
        }

        public AgreementModel(AgreementEntity entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            Title = entity.Title;
            Language = entity.Language;
            Description = entity.Description;
            Status = entity.Status;
            UpdatedAt = TimeHelper.TimestampTo(entity.UpdatedAt);
            CreatedAt = TimeHelper.TimestampTo(entity.CreatedAt);
            Content = JsonSerializer.Deserialize<List<AgreementGroupItem>>(entity.Content, LinkRule.SerializeOptions) ?? [];
        }
    }

    public class AgreementGroupItem
    {
        public string Name { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;

        public IList<AgreementBodyItem> Children { get; set; } = [];
    }

    public class AgreementBodyItem
    {
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// 是否加粗
        /// </summary>
        public bool B {  get; set; }
    }
}
