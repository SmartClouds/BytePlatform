using BytePlatform.Shared.Resources;
using Microsoft.Extensions.Localization;
using System.Net;

namespace BytePlatform.Shared.Exceptions;

public class ForbiddenException : RestException
{
    public ForbiddenException()
        : base(BytePlatformStrings.ExceptionError.ForbiddenException)
    {
    }

    public ForbiddenException(string message)
        : base(message)
    {
    }

    public ForbiddenException(string message, Exception? innerException)
        : base(message, innerException)
    {
    }

    public ForbiddenException(LocalizedString message)
        : base(message)
    {
    }

    public ForbiddenException(LocalizedString message, Exception? innerException)
        : base(message, innerException)
    {
    }

    public override HttpStatusCode StatusCode => HttpStatusCode.Forbidden;
}
