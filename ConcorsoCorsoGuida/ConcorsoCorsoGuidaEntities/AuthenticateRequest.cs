﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ConcorsoCorsoGuidaEntities
{
    public class AuthenticateRequest
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Token { get; set; }
    }
}
