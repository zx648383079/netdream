﻿using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.SEO.Entities
{
    [TableName("base_file_log")]
    public class FileLogEntity
    {
        public int Id { get; set; }
        [Column("file_id")]
        public int FileId { get; set; }
        [Column("item_type")]
        public int ItemType { get; set; }
        [Column("item_id")]
        public int ItemId { get; set; }
        public string? Data { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
