using System.Net;
using BytePlatform.Shared.Resources;
using Microsoft.Extensions.Localization;

namespace BytePlatform.Shared.Exceptions;

public partial class ConflictException : RestException
{
    public ConflictException()
        : this(BytePlatformStrings.ExceptionError.ConflictException)
    {
    }

    public ConflictException(string message)
        : base(message)
    {
    }

    public ConflictException(string message, Exception? innerException)
        : base(message, innerException)
    {
    }

    public ConflictException(LocalizedString message)
        : base(message)
    {
    }

    public ConflictException(LocalizedString message, Exception? innerException)
        : base(message, innerException)
    {
    }

    public override HttpStatusCode StatusCode => HttpStatusCode.Conflict;
}
