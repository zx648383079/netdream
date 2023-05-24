﻿using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.SEO.Entities
{
    [TableName("seo_agreement")]
    public class AgreementEntity
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Title { get; set; }
        public string? Language { get; set; }
        public string? Description { get; set; }
        public string? Content { get; set; }
        public int Status { get; set; }
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
