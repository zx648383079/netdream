﻿using NetDream.Modules.Blog.Entities;
using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Blog.Models
{
    public class BlogModel: BlogEntity, IWithUserModel, IWithCategoryModel
    {
        public IListLabelItem? Term { get; set; }
        public IUser? User { get; set; }
        public bool IsLocalization { get; set; }
    }
}
