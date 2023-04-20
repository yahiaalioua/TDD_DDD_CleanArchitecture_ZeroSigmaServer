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
            //arrange
            string passwordToEncrypt = "passwordToEncrypt";
            EncryptionService encryptionService = new EncryptionService();
            //act
            string encryptedPassword=encryptionService.EncryptPassword(passwordToEncrypt);
            //assert
            Assert.NotEqual(passwordToEncrypt, encryptedPassword);
            Assert.NotNull(encryptionService);
            Assert.True(encryptedPassword.Length < 72);
            Assert.StartsWith("$2a$12$", encryptedPassword);
        }
        [Fact]
        public void ShouldReturnTrueWhenPasswordIsSameAsHash()
        {
            //arrange
            string passwordToEncrypt = "passwordToEncrypt";
            EncryptionService encryptionService = new EncryptionService();
            //act
            string encryptedPassword = encryptionService.EncryptPassword(passwordToEncrypt);
            //assert
            Assert.True(encryptionService.VerifyPassword(passwordToEncrypt,encryptedPassword));
        }
        [Fact]
        public void ShouldReturnFalseWhenPasswordIsNotSameAsHash()
        {
            //arrange
            string passwordToEncrypt = "passwordToEncrypt";
            EncryptionService encryptionService = new EncryptionService();
            //act
            string encryptedPassword = encryptionService.EncryptPassword(passwordToEncrypt);
            //assert
            Assert.False(encryptionService.VerifyPassword("WrongPassword", encryptedPassword));
        }
    }
}
