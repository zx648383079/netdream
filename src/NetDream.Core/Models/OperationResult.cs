using NetDream.Core.Interfaces;
using System;

namespace NetDream.Core.Models
{
    public class OperationResult(bool success = true, int failureReason = FailureReasons.None, string? message = null, Exception? error = null) : IOperationResult
    {
        public bool IsSuccess => success;

        public int FailureReason => failureReason;

        public string Message => message ?? error?.Message ?? string.Empty;

        public Exception? Error => error;


        public static OperationResult Ok()
        => new(success: true);

        public static OperationResult Fail(int failureReason)
            => new(false, failureReason: failureReason);

        public static OperationResult Fail(int failureReason, string message)
            => new(false, failureReason: failureReason, message: message);

        public static OperationResult Fail(int failureReason, Exception? error)
            => new(false, failureReason: failureReason, error: error);

        public static bool operator true(OperationResult result)
            => result.IsSuccess;

        public static bool operator false(OperationResult result)
            => !result.IsSuccess;

        public static implicit operator bool(OperationResult result)
            => result.IsSuccess;
    }
}
