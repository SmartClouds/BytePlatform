using BytePlatform.Shared.Resources;
using Microsoft.Extensions.Localization;
using System.Net;

namespace BytePlatform.Shared.Exceptions;

public class UnauthorizedException : RestException
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
