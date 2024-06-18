﻿namespace BytePlatform.Server.Models;

public interface IAuditableEntity
{
    DateTimeOffset CreatedOn { get; set; }
    DateTimeOffset ModifiedOn { get; set; }
}
