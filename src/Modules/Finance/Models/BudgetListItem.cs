﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Finance.Models
{
    public class BudgetListItem : IBudget
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public float Budget { get; set; }
        public float Spent { get; set; }
        public byte Cycle { get; set; }
        public int UpdatedAt { get; set; }
        public int CreatedAt { get; set; }

        public float Remain => Budget - Spent;
    }
}
