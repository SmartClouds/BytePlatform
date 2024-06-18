using BytePlatform.Shared.Resources;
using Microsoft.Extensions.Localization;
using System.Net;

namespace BytePlatform.Shared.Exceptions;

public class ResourceNotFoundException : RestException
{
    public ResourceNotFoundException()
        : base(BytePlatformStrings.ExceptionError.ResourceNotFoundException)
    {
    }

    public ResourceNotFoundException(string message)
        : base(message)
    {
    }

    public ResourceNotFoundException(string message, Exception? innerException)
        : base(message, innerException)
    {
    }

    public ResourceNotFoundException(LocalizedString message)
        : base(message)
    {
    }

    public ResourceNotFoundException(LocalizedString message, Exception? innerException)
        : base(message, innerException)
    {
    }

    public override HttpStatusCode StatusCode => HttpStatusCode.NotFound;
}
