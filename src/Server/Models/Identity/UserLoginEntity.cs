using Microsoft.AspNetCore.Identity;

namespace BytePlatform.Server.Models.Identity;

public partial class UserLoginEntity<TKey> : IdentityUserLogin<TKey> where TKey : IEquatable<TKey>
{
}
