﻿using NetDream.Modules.Book.Entities;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Book.Models
{
    public class BookModel: BookEntity
    {
        [Ignore]
        public CategoryEntity? Category { get; set; }
        [Ignore]
        public AuthorEntity? Author { get; set; }
    }
}