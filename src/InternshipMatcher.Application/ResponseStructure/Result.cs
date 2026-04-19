using FluentValidation;
using InternshipMatcher.Application.ApplicationEnums;

namespace InternshipMatcher.API.Common.ResponseStructure;


public class Result<T> where T : class
{
    public bool IsSuccess { get; private set; }
    public string Message { get; private set; } = string.Empty;
    public T? Data { get; private set; }
    public List<string> Errors { get; private set; } = new();
    public string Description { get; internal set; }

    private Result() { }

    // ✅ Success
    public static Result<T> Success(string message = "")
    {
        return new Result<T>
        {
            IsSuccess = true,
            Message = message,
            Errors = new List<string>()
        };
    }
    public static Result<T> Success(T data, string message = "")
    {
        return new Result<T>
        {
            IsSuccess = true,
            Data = data,
            Message = message,
            Errors = new List<string>()
        };
    }

    // ❌ Failure
    public static Result<T> Failure(string message, List<string>? errors = null)
    {
        return new Result<T>
        {
            IsSuccess = false,
            Message = message,
            Data = null,
            Errors = errors ?? new List<string>()
        };
    }

   
}