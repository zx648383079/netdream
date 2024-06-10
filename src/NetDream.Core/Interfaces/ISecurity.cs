﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Core.Interfaces
{
    public interface ISecurity
    {
        public string Encrypt(string data);

        public string Decrypt(string data);
    }
}
