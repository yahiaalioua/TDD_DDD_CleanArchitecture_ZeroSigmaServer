using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroSigma.Domain.Entities
{
    public record User
    {
        public Guid Id { get; set; }= Guid.NewGuid();
        public string Name { get; set; } = null!;
        public string Email { get; set; }= null!;
        public string Password { get; set; }=null!;
        public string AccessToken { get;set; }=null!;
        public string RefreshToken { get; set; } = null!;

    }
}
