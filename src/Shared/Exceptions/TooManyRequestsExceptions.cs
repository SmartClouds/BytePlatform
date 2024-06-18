using BytePlatform.Shared.Resources;
using Microsoft.Extensions.Localization;
using System.Net;

namespace BytePlatform.Shared.Exceptions;

public class TooManyRequestsExceptions : RestException
{
    public TooManyRequestsExceptions()
        : base(BytePlatformStrings.ExceptionError.TooManyRequestsExceptions)
    {
    }

    public TooManyRequestsExceptions(string message)
        : base(message)
    {
    }

    public TooManyRequestsExceptions(string message, Exception? innerException)
        : base(message, innerException)
    {
    }

    public TooManyRequestsExceptions(LocalizedString message)
        : base(message)
    {
    }

    public TooManyRequestsExceptions(LocalizedString message, Exception? innerException)
        : base(message, innerException)
    {
    }

    public override HttpStatusCode StatusCode => HttpStatusCode.TooManyRequests;
}
