using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroSigma.Application.Authentication.Services.Encryption
{
    public interface IEncryptionService
    {
        public string EncryptPassword(string password);
        public bool VerifyPassword(string password, string passwordHash);
    }
}
