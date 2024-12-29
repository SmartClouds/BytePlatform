namespace BytePlatform.Shared.Resources;
public static class BytePlatformStrings
{
    public static class ExceptionError
    {
        public const string ConflictException = $"{nameof(BytePlatformStrings)}_{nameof(ExceptionError)}_{nameof(ConflictException)}";
        public const string BadRequestException = $"{nameof(BytePlatformStrings)}_{nameof(ExceptionError)}_{nameof(BadRequestException)}";
        public const string ForbiddenException = $"{nameof(BytePlatformStrings)}_{nameof(ExceptionError)}_{nameof(ForbiddenException)}";
        public const string ResourceNotFoundException = $"{nameof(BytePlatformStrings)}_{nameof(ExceptionError)}_{nameof(ResourceNotFoundException)}";
        public const string ResourceValidationException = $"{nameof(BytePlatformStrings)}_{nameof(ExceptionError)}_{nameof(ResourceValidationException)}";
        public const string RestException = $"{nameof(BytePlatformStrings)}_{nameof(ExceptionError)}_{nameof(RestException)}";
        public const string ServerConnectionException = $"{nameof(BytePlatformStrings)}_{nameof(ExceptionError)}_{nameof(ServerConnectionException)}";
        public const string TooManyRequestsExceptions = $"{nameof(BytePlatformStrings)}_{nameof(ExceptionError)}_{nameof(TooManyRequestsExceptions)}";
        public const string UnauthorizedException = $"{nameof(BytePlatformStrings)}_{nameof(ExceptionError)}_{nameof(UnauthorizedException)}";
        public const string UnknownException = $"{nameof(BytePlatformStrings)}_{nameof(ExceptionError)}_{nameof(UnknownException)}";
    }

    public static class General
    {
        public const string Error = $"{nameof(BytePlatformStrings)}_{nameof(General)}_{nameof(Error)}";
        public const string ItemCouldNotBeFound = $"{nameof(BytePlatformStrings)}_{nameof(General)}_{nameof(ItemCouldNotBeFound)}";
        public const string PropertyAlreadyExists = $"{nameof(BytePlatformStrings)}_{nameof(General)}_{nameof(PropertyAlreadyExists)}";
        public const string UpdateConcurrencyException = $"{nameof(BytePlatformStrings)}_{nameof(General)}_{nameof(UpdateConcurrencyException)}";
    }

    public static class ValidationError
    {
        public const string EmailAddressAttribute = $"{nameof(BytePlatformStrings)}_{nameof(ValidationError)}_{nameof(EmailAddressAttribute)}";
        public const string RequiredAttribute = $"{nameof(BytePlatformStrings)}_{nameof(ValidationError)}_{nameof(RequiredAttribute)}";
        public const string MinLengthAttribute = $"{nameof(BytePlatformStrings)}_{nameof(ValidationError)}_{nameof(MinLengthAttribute)}";
        public const string CompareAttribute = $"{nameof(BytePlatformStrings)}_{nameof(ValidationError)}_{nameof(CompareAttribute)}";
    }

    public static class IdentityError
    {
        public const string ConcurrencyFailure = $"{nameof(BytePlatformStrings)}_{nameof(IdentityError)}_{nameof(ConcurrencyFailure)}";
        public const string DuplicateEmail = $"{nameof(BytePlatformStrings)}_{nameof(IdentityError)}_{nameof(DuplicateEmail)}";
        public const string DuplicateRoleName = $"{nameof(BytePlatformStrings)}_{nameof(IdentityError)}_{nameof(DuplicateRoleName)}";
        public const string DuplicateUserName = $"{nameof(BytePlatformStrings)}_{nameof(IdentityError)}_{nameof(DuplicateUserName)}";
        public const string InvalidEmail = $"{nameof(BytePlatformStrings)}_{nameof(IdentityError)}_{nameof(InvalidEmail)}";
        public const string InvalidRoleName = $"{nameof(BytePlatformStrings)}_{nameof(IdentityError)}_{nameof(InvalidRoleName)}";
        public const string InvalidToken = $"{nameof(BytePlatformStrings)}_{nameof(IdentityError)}_{nameof(InvalidToken)}";
        public const string InvalidUserName = $"{nameof(BytePlatformStrings)}_{nameof(IdentityError)}_{nameof(InvalidUserName)}";
        public const string LoginAlreadyAssociated = $"{nameof(BytePlatformStrings)}_{nameof(IdentityError)}_{nameof(LoginAlreadyAssociated)}";
        public const string PasswordMismatch = $"{nameof(BytePlatformStrings)}_{nameof(IdentityError)}_{nameof(PasswordMismatch)}";
        public const string PasswordRequiresDigit = $"{nameof(BytePlatformStrings)}_{nameof(IdentityError)}_{nameof(PasswordRequiresDigit)}";
        public const string PasswordRequiresLower = $"{nameof(BytePlatformStrings)}_{nameof(IdentityError)}_{nameof(PasswordRequiresLower)}";
        public const string PasswordRequiresNonAlphanumeric = $"{nameof(BytePlatformStrings)}_{nameof(IdentityError)}_{nameof(PasswordRequiresNonAlphanumeric)}";
        public const string PasswordRequiresUniqueChars = $"{nameof(BytePlatformStrings)}_{nameof(IdentityError)}_{nameof(PasswordRequiresUniqueChars)}";
        public const string PasswordRequiresUpper = $"{nameof(BytePlatformStrings)}_{nameof(IdentityError)}_{nameof(PasswordRequiresUpper)}";
        public const string PasswordTooShort = $"{nameof(BytePlatformStrings)}_{nameof(IdentityError)}_{nameof(PasswordTooShort)}";
        public const string RecoveryCodeRedemptionFailed = $"{nameof(BytePlatformStrings)}_{nameof(IdentityError)}_{nameof(RecoveryCodeRedemptionFailed)}";
        public const string UserAlreadyHasPassword = $"{nameof(BytePlatformStrings)}_{nameof(IdentityError)}_{nameof(UserAlreadyHasPassword)}";
        public const string UserAlreadyInRole = $"{nameof(BytePlatformStrings)}_{nameof(IdentityError)}_{nameof(UserAlreadyInRole)}";
        public const string UserLockoutNotEnabled = $"{nameof(BytePlatformStrings)}_{nameof(IdentityError)}_{nameof(UserLockoutNotEnabled)}";
        public const string UserNotInRole = $"{nameof(BytePlatformStrings)}_{nameof(IdentityError)}_{nameof(UserNotInRole)}";
        public const string DefaultError = $"{nameof(BytePlatformStrings)}_{nameof(IdentityError)}_{nameof(DefaultError)}";
    }
}
