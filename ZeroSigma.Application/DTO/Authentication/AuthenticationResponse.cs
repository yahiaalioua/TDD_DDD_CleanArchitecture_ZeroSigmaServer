﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroSigma.Application.DTO.Authentication
{
    public record AuthenticationResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public String AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
    }
}
