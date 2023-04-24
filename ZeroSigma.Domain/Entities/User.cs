using ZeroSigma.Domain.Models;
using ZeroSigma.Domain.User.ValueObjects;

namespace ZeroSigma.Domain.Entities
{
    public sealed class User:Entity<UserID>
    {
        public User(
            UserID id, FullName fullName, string email,
            string password, string accessToken,
            string refreshToken) : base(id)
        {
            FullName = fullName;
            Email = email;
            Password = password;
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }

        public FullName FullName { get; set; }=null!;
        public string Email { get; set; }= null!;
        public string Password { get; set; }=null!;
        public string ?AccessToken { get;set; }=null!;
        public string RefreshToken { get; set; } = null!;

        public static User Create(FullName fullName,string email,string password,string accessToken,string refreshToken)
        {
            return new(UserID.CreateUnique(), fullName, email, password, accessToken, refreshToken);
        }
    }
}
