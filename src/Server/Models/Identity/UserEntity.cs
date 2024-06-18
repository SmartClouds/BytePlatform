using Microsoft.AspNetCore.Identity;

namespace BytePlatform.Server.Models.Identity;

public partial class UserEntity<TKey> : IdentityUser<TKey>, IEntity<TKey> where TKey : IEquatable<TKey>
{
}
