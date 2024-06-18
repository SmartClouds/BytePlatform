using Microsoft.AspNetCore.Identity;

namespace BytePlatform.Server.Models.Identity;

public partial class UserRoleEntity<TKey> : IdentityUserRole<TKey>, IEntity where TKey : IEquatable<TKey>
{
}
