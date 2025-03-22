using NetDream.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Shared.Models
{
    public class Page<T> : IPage<T>
    {
        public T[] Items { get; set; } = Array.Empty<T>();

        public int ItemsPerPage { get; } = 20;
        public int CurrentPage { get; } = 1;
        public int TotalItems { get; }
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / ItemsPerPage);

        /// <summary>
        /// 本页数据的偏移
        /// </summary>
        public int ItemsOffset => (CurrentPage - 1) * ItemsPerPage;
        /// <summary>
        /// 判断本页是否有内容
        /// </summary>
        public bool IsEmpty => ItemsOffset >= TotalItems;

        public Page()
        {
            
        }

        public Page(IEnumerable<T> items)
        {
            Items = items.ToArray();
            TotalItems = Items.Length;
        }

        public Page(IPagination source)
            : this(source.TotalItems, source.CurrentPage, source.ItemsPerPage)
        {
            
        }

        public Page(int total, int page, int perPage)
        {
            TotalItems = total;
            ItemsPerPage = perPage;
            CurrentPage = Math.Max(page, 1);
        }

        public Page(int total, int page)
            : this(total, page, 20)
        {
            
        }

        public Page(int total, PaginationForm form)
            : this (total, form.Page, form.PerPage)
        {
        }
    }
}
