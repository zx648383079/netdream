using NetDream.Shared.Interfaces;
using System;

namespace NetDream.Shared.Models
{
    public class OperationResult(bool success = true, int failureReason = FailureReasons.None, string? message = null, Exception? error = null) : IOperationResult
    {
        public bool Succeeded => success;

        public int FailureReason => failureReason;

        public string Message => message ?? error?.Message ?? string.Empty;

        public Exception? Error => error;


        public static OperationResult Ok() => new(success: true);


        public static OperationResult Fail(int failureReason)
            => new(false, failureReason: failureReason);

        public static OperationResult Fail(int failureReason, string message)
            => new(false, failureReason: failureReason, message: message);

        public static OperationResult Fail(string message)
            => Fail(404, message);

        public static OperationResult Fail(int failureReason, Exception? error)
            => new(false, failureReason: failureReason, error: error);

        public static OperationResult<T> Ok<T>(T content)
            => OperationResult<T>.Ok(content);

        public static OperationResult<T> Fail<T>(string message)
            => OperationResult<T>.Fail(message);

        public static bool operator true(OperationResult result)
            => result.Succeeded;

        public static bool operator false(OperationResult result)
            => !result.Succeeded;

        public static implicit operator bool(OperationResult result)
            => result.Succeeded;
    }
}
