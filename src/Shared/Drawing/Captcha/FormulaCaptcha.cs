﻿using NetDream.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Shared.Drawing.Captcha
{
    public class FormulaCaptcha(ICaptchaContext context) : ICaptcha
    {
        public bool IsOnlyImage => true;

        public ICaptchaOutput Generate()
        {
            throw new NotImplementedException();
        }

        public bool Verify(object value, object source)
        {
            throw new NotImplementedException();
        }
    }
}
