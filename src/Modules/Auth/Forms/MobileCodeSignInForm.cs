﻿using NetDream.Core.Interfaces.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Auth.Forms
{
    public class MobileCodeSignInForm: ISignInForm
    {
        public string Mobile { get; set; }

        public string Code { get; set; }
    }
}