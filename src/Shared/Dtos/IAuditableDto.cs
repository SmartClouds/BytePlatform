namespace BytePlatform.Shared.Dtos;

public interface IAuditableDto
{
    DateTimeOffset CreatedOn { get; set; }
    DateTimeOffset ModifiedOn { get; set; }
}
