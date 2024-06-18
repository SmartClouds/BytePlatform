using Microsoft.AspNetCore.Identity;

namespace BytePlatform.Server.Models.Identity;

public partial class RoleEntity<TKey> : IdentityRole<TKey>, IEntity where TKey : IEquatable<TKey>
{
}
