using Microsoft.AspNetCore.Identity;

namespace BytePlatform.Server.Models.Identity;

public partial class RoleClaimEntity<TKey> : IdentityRoleClaim<TKey>, IEntity where TKey : IEquatable<TKey>
{
    public new TKey Id { get; set; }
}
