using ZeroSigma.Domain.Models;
using ZeroSigma.Domain.ValueObjects.User;

namespace ZeroSigma.Domain.Entities
{
    public sealed class User:AggregateRoot<UserID>
    {
        private readonly List<UserAccess> _userAccess = new();
        public User(
            UserID id, FullName fullName, UserEmail email,
            UserPassword password) : base(id)
        {
            FullName = fullName;
            Email = email;
            Password = password;
        }

        public FullName FullName { get; set; }=null!;
        public UserEmail Email { get; set; }= null!;
        public UserPassword Password { get; set; }=null!;
        public IReadOnlyList<UserAccess> UserAccess=>_userAccess.ToList();

        public static User Create(FullName fullName,UserEmail email,UserPassword password)
        {
            return new(UserID.CreateUnique(), fullName, email, password);
        }
    }
}
