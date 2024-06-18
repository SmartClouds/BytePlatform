using Microsoft.AspNetCore.Identity;

namespace BytePlatform.Server.Models.Identity;

public partial class UserTokenEntity<TKey> : IdentityUserToken<TKey>, IEntity where TKey : IEquatable<TKey>
{
}
