using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroSigma.Infrastructure.Authentication.Encryption;

namespace ZeroSigma.Infrastructure.Authentication
{
    public class EncryptionTests
    {
        [Fact]
        public void ShouldEncryptPassword()
        {
            string passwordToEncrypt = "passwordToEncrypt";
            EncryptionService encryptionService = new EncryptionService();
            string encryptedPassword=encryptionService.EncryptPassword(passwordToEncrypt);
            Assert.NotEqual(passwordToEncrypt, encryptedPassword);
            Assert.NotNull(encryptionService);
            Assert.True(encryptedPassword.Length < 72);
            Assert.StartsWith("$2a$12$", encryptedPassword);


        }
    }
}
