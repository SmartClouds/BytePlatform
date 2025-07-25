namespace BytePlatform.Server.Data.Models;

public interface IArchivableEntity
{
    bool IsArchived { get; set; }
}
