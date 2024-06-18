using Microsoft.AspNetCore.Identity;

namespace BytePlatform.Server.Models.Identity;

public partial class UserClaimEntity<TKey> : IdentityUserClaim<TKey>, IEntity where TKey : IEquatable<TKey>
{
    public new TKey Id { get; set; }
}
