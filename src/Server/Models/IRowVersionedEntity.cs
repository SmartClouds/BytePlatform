namespace BytePlatform.Server.Models;

public interface IRowVersionedEntity
{
    byte[] ConcurrencyStamp { get; set; }
}
