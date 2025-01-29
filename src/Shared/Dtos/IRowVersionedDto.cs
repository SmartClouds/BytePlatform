namespace BytePlatform.Shared.Dtos;

public interface IRowVersionedDto
{
    byte[] ConcurrencyStamp { get; set; }
}
