namespace BytePlatform.Server.Services.Helpers;

public class ServiceResult
{
    public ServiceResult()
    {
        Result = Result.NotAction;
        ValidationResult = new EntityValidationResult();
    }
    public EntityValidationResult ValidationResult { get; set; }

    public Result Result { get; set; }
    public bool Succeeded => Result == Result.Success;

    public void SetResult(Result result) => Result = result;
    public Result SetAndGetResult(Result result)
    {
        Result = result;
        return Result;
    }
}

public enum Result
{
    Success,
    Unsuccess,
    AccessDenied,
    NotAction
}
