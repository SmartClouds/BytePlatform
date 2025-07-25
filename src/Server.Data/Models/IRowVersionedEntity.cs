namespace BytePlatform.Server.Data.Models;

public interface IRowVersionedEntity
{
    byte[] ConcurrencyStamp { get; set; }
}
