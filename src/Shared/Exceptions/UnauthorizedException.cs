using System.Net;
using BytePlatform.Shared.Resources;
using Microsoft.Extensions.Localization;

namespace BytePlatform.Shared.Exceptions;

public partial class UnauthorizedException : RestException
{
    public UnauthorizedException()
        : base(BytePlatformStrings.ExceptionError.UnauthorizedException)
    {
    }

    public UnauthorizedException(string message)
        : base(message)
    {
    }

    public UnauthorizedException(string message, Exception? innerException)
        : base(message, innerException)
    {
    }

    public UnauthorizedException(LocalizedString message)
        : base(message)
    {
    }

    public UnauthorizedException(LocalizedString message, Exception? innerException)
        : base(message, innerException)
    {
    }

    public override HttpStatusCode StatusCode => HttpStatusCode.Unauthorized;
}
