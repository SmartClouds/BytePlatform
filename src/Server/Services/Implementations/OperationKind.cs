namespace BytePlatform.Server.Services.Implementations;

[Flags]
public enum OperationKind
{
    Get = 1,
    Add = 2,
    Edit = 4,
    Remove = 8
}
