using System.ComponentModel.DataAnnotations;
using BytePlatform.Server.Models;

namespace BytePlatform.Server.Services.Helpers;

[Serializable]
public class EntityValidationResult
{
    public IList<ValidationResult> Errors { get; private set; }

    public bool IsValid
    {
        get { return Errors.Count <= 0; }
    }

    public EntityValidationResult(IList<ValidationResult> violations = null)
    {
        Errors = violations ?? new List<ValidationResult>();
    }

    public void AddError(string error)
    {
        if (Errors == null)
        {
            Errors = new List<ValidationResult>();
        }
        Errors.Add(new ValidationResult(error));
    }

    public void AddErrors(IEnumerable<string> errors)
    {
        if (Errors == null)
        {
            Errors = new List<ValidationResult>();
        }

        foreach (var error in errors)
        {
            Errors.Add(new ValidationResult(error));
        }
    }

    public void AddErrors(IList<ValidationResult> errors)
    {
        if (Errors == null)
        {
            Errors = new List<ValidationResult>();
        }

        foreach (var error in errors)
        {
            Errors.Add(error);
        }
    }
}

public class DataAnnotation
{
    public static EntityValidationResult ValidateEntity<T>(T entity) where T : IEntity
    {
        return new EntityValidator<T>().Validate(entity);
    }

    public static bool IsValid<T>(T entity) where T : IEntity
    {
        return new EntityValidator<T>().Validate(entity).IsValid;
    }
}

public class EntityValidator<T> where T : IEntity
{
    public EntityValidationResult Validate(T entity)
    {
        var validationResults = new List<ValidationResult>();
        var vc = new ValidationContext(entity, null, null);
        var isValid = Validator.TryValidateObject(entity, vc, validationResults, true);

        return new EntityValidationResult(validationResults);
    }
}
