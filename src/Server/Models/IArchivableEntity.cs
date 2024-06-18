namespace BytePlatform.Server.Models;

public interface IArchivableEntity
{
    bool IsArchived { get; set; }
}
