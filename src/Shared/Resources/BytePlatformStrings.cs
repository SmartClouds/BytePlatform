namespace BytePlatform.Shared.Resources;
public static class BytePlatformStrings
{
    public static class ExceptionError
    {
        public const string ConflictException = $"{nameof(BytePlatformStrings)}.{nameof(ExceptionError)}.{nameof(ConflictException)}";
        public const string BadRequestException = $"{nameof(BytePlatformStrings)}.{nameof(ExceptionError)}.{nameof(BadRequestException)}";
        public const string ForbiddenException = $"{nameof(BytePlatformStrings)}.{nameof(ExceptionError)}.{nameof(ForbiddenException)}";
        public const string ResourceNotFoundException = $"{nameof(BytePlatformStrings)}.{nameof(ExceptionError)}.{nameof(ResourceNotFoundException)}";
        public const string ResourceValidationException = $"{nameof(BytePlatformStrings)}.{nameof(ExceptionError)}.{nameof(ResourceValidationException)}";
        public const string RestException = $"{nameof(BytePlatformStrings)}.{nameof(ExceptionError)}.{nameof(RestException)}";
        public const string ServerConnectionException = $"{nameof(BytePlatformStrings)}.{nameof(ExceptionError)}.{nameof(ServerConnectionException)}";
        public const string TooManyRequestsExceptions = $"{nameof(BytePlatformStrings)}.{nameof(ExceptionError)}.{nameof(TooManyRequestsExceptions)}";
        public const string UnauthorizedException = $"{nameof(BytePlatformStrings)}.{nameof(ExceptionError)}.{nameof(UnauthorizedException)}";
        public const string UnknownException = $"{nameof(BytePlatformStrings)}.{nameof(ExceptionError)}.{nameof(UnknownException)}";
    }

    public static class General
    {
        public const string Error = $"{nameof(BytePlatformStrings)}.{nameof(General)}.{nameof(Error)}";
        public const string ItemCouldNotBeFound = $"{nameof(BytePlatformStrings)}.{nameof(General)}.{nameof(ItemCouldNotBeFound)}";
        public const string PropertyAlreadyExists = $"{nameof(BytePlatformStrings)}.{nameof(General)}.{nameof(PropertyAlreadyExists)}";
        public const string UpdateConcurrencyException = $"{nameof(BytePlatformStrings)}.{nameof(General)}.{nameof(UpdateConcurrencyException)}";
    }

    public static class ValidationError
    {
        public const string EmailAddressAttribute = $"{nameof(BytePlatformStrings)}.{nameof(ValidationError)}.{nameof(EmailAddressAttribute)}";
        public const string RequiredAttribute = $"{nameof(BytePlatformStrings)}.{nameof(ValidationError)}.{nameof(RequiredAttribute)}";
        public const string MinLengthAttribute = $"{nameof(BytePlatformStrings)}.{nameof(ValidationError)}.{nameof(MinLengthAttribute)}";
        public const string CompareAttribute = $"{nameof(BytePlatformStrings)}.{nameof(ValidationError)}.{nameof(CompareAttribute)}";
    }

    public static class IdentityError
    {
        public const string ConcurrencyFailure = $"{nameof(BytePlatformStrings)}.{nameof(IdentityError)}.{nameof(ConcurrencyFailure)}";
        public const string DuplicateEmail = $"{nameof(BytePlatformStrings)}.{nameof(IdentityError)}.{nameof(DuplicateEmail)}";
        public const string DuplicateRoleName = $"{nameof(BytePlatformStrings)}.{nameof(IdentityError)}.{nameof(DuplicateRoleName)}";
        public const string DuplicateUserName = $"{nameof(BytePlatformStrings)}.{nameof(IdentityError)}.{nameof(DuplicateUserName)}";
        public const string InvalidEmail = $"{nameof(BytePlatformStrings)}.{nameof(IdentityError)}.{nameof(InvalidEmail)}";
        public const string InvalidRoleName = $"{nameof(BytePlatformStrings)}.{nameof(IdentityError)}.{nameof(InvalidRoleName)}";
        public const string InvalidToken = $"{nameof(BytePlatformStrings)}.{nameof(IdentityError)}.{nameof(InvalidToken)}";
        public const string InvalidUserName = $"{nameof(BytePlatformStrings)}.{nameof(IdentityError)}.{nameof(InvalidUserName)}";
        public const string LoginAlreadyAssociated = $"{nameof(BytePlatformStrings)}.{nameof(IdentityError)}.{nameof(LoginAlreadyAssociated)}";
        public const string PasswordMismatch = $"{nameof(BytePlatformStrings)}.{nameof(IdentityError)}.{nameof(PasswordMismatch)}";
        public const string PasswordRequiresDigit = $"{nameof(BytePlatformStrings)}.{nameof(IdentityError)}.{nameof(PasswordRequiresDigit)}";
        public const string PasswordRequiresLower = $"{nameof(BytePlatformStrings)}.{nameof(IdentityError)}.{nameof(PasswordRequiresLower)}";
        public const string PasswordRequiresNonAlphanumeric = $"{nameof(BytePlatformStrings)}.{nameof(IdentityError)}.{nameof(PasswordRequiresNonAlphanumeric)}";
        public const string PasswordRequiresUniqueChars = $"{nameof(BytePlatformStrings)}.{nameof(IdentityError)}.{nameof(PasswordRequiresUniqueChars)}";
        public const string PasswordRequiresUpper = $"{nameof(BytePlatformStrings)}.{nameof(IdentityError)}.{nameof(PasswordRequiresUpper)}";
        public const string PasswordTooShort = $"{nameof(BytePlatformStrings)}.{nameof(IdentityError)}.{nameof(PasswordTooShort)}";
        public const string RecoveryCodeRedemptionFailed = $"{nameof(BytePlatformStrings)}.{nameof(IdentityError)}.{nameof(RecoveryCodeRedemptionFailed)}";
        public const string UserAlreadyHasPassword = $"{nameof(BytePlatformStrings)}.{nameof(IdentityError)}.{nameof(UserAlreadyHasPassword)}";
        public const string UserAlreadyInRole = $"{nameof(BytePlatformStrings)}.{nameof(IdentityError)}.{nameof(UserAlreadyInRole)}";
        public const string UserLockoutNotEnabled = $"{nameof(BytePlatformStrings)}.{nameof(IdentityError)}.{nameof(UserLockoutNotEnabled)}";
        public const string UserNotInRole = $"{nameof(BytePlatformStrings)}.{nameof(IdentityError)}.{nameof(UserNotInRole)}";
        public const string DefaultError = $"{nameof(BytePlatformStrings)}.{nameof(IdentityError)}.{nameof(DefaultError)}";
    }
}
