namespace BytePlatform.Tiny.Shared.Dtos;

public interface IRowVersionedDto
{
    byte[] ConcurrencyStamp { get; set; }
}
