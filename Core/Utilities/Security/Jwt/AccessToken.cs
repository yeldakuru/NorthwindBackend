﻿using System;
using System.Collections.Generic;

using System.Text;


namespace Core.Utilities.Security.Jwt
{
    public class AccessToken //erişim anahtarı
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }

    }
}
