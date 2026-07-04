namespace E_Commerce.Application.Common;

public record Error(string Code, string Description, ErrorType ErrorType = ErrorType.Failure)
{
    public static Error Failure(string code = "General.Failure", string description = "General Failure has Occurred") => new(code, description, ErrorType.Failure);
    public static Error Validation(string code = "General.Validation", string description = "General Validation error has Occurred") => new(code, description, ErrorType.Validation);
    public static Error NotFound(string code = "General.NotFound", string description = "General NotFound error has Occurred") => new(code, description, ErrorType.NotFound);
    public static Error Conflict(string code = "General.Conflict", string description = "General Conflict error has Occurred") => new(code, description, ErrorType.Conflict);
    public static Error Unauthorized(string code = "General.Unauthorized", string description = "General Unauthorized error has Occurred") => new(code, description, ErrorType.Unauthorized);
    public static Error Forbidden(string code = "General.Forbidden", string description = "General Forbidden error has Occurred") => new(code, description, ErrorType.Forbidden);
    public static Error InvalidCredentials(string code = "General.InvalidCredentials", string description = "General InvalidCredentials error has Occurred") => new(code, description, ErrorType.InvalidCredentials);
}

public enum ErrorType
{
    Failure = 0,
    Validation = 1,
    NotFound = 2,
    Conflict = 3,
    Unauthorized = 4,
    Forbidden = 5,
    InvalidCredentials = 6
}