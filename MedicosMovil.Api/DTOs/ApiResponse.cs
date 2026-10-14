namespace MedicosMovil.Api.DTOs;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = "Operación exitosa";
    public T? Data { get; set; }

    public static ApiResponse<T> Ok(T data, string message = "Operación exitosa")
        => new() { Success = true, Message = message, Data = data };

    public static ApiResponse<T> Error(string message)
        => new() { Success = false, Message = message };
}

public class ServiceResult<T>
{
    public T? Data { get; set; }
    public string? ErrorMessage { get; set; }
    public bool NotFound { get; set; }
    public bool IsSuccess => ErrorMessage is null && !NotFound;

    public static ServiceResult<T> Success(T data) => new() { Data = data };

    public static ServiceResult<T> Error(string message) => new() { ErrorMessage = message };

    public static ServiceResult<T> NotFount() => new() { NotFound = true };
}