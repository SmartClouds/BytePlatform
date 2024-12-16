using BytePlatform.Shared.Resources;

namespace BytePlatform.Shared.Exceptions;
public partial class ServerConnectionException : KnownException
{
    public ServerConnectionException()
        : base(BytePlatformStrings.ExceptionError.ServerConnectionException)
    {
    }

    public ServerConnectionException(string message)
        : base(message)
    {
    }

    public ServerConnectionException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
