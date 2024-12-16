using BytePlatform.Shared.Resources;

namespace BytePlatform.Shared.Exceptions;

public partial class UnknownException : Exception
{
    public UnknownException()
        : base(BytePlatformStrings.ExceptionError.UnknownException)
    {
    }

    public UnknownException(string message)
        : base(message)
    {
    }

    public UnknownException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
