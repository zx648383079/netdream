using NetDream.Shared.Interfaces;
using System;

namespace NetDream.Shared.Models
{
    public class OperationResult<T> : IOperationResult<T>
    {
        public T? Result { get; private set; }

        public bool Succeeded { get; private set; }

        public int FailureReason { get; private set; }

        private string? _message;
        public string Message => _message ?? Error?.Message ?? string.Empty;

        public Exception? Error { get; private set; }

        public OperationResult(T result, string? message = null)
        {
            Result = result;
            Succeeded = true;
            _message = message;
        }

        public OperationResult(int failureReason = FailureReasons.None, string? message = null, Exception? error = null)
        {
            Succeeded = false;
            _message = message;
            FailureReason = failureReason;
            Error = error;
        }

        public OperationResult(bool success = true, T? result = default, int failureReason = FailureReasons.None, string? message = null, Exception? error = null)
        {
            Result = result;
            Succeeded = result is not null || success;
            _message = message;
            FailureReason = failureReason;
            Error = error;
        }

        public static OperationResult<T> Ok(T content)
            => new(content);

        public static OperationResult<T> Fail(int failureReason)
            => new(false, failureReason: failureReason);

        public static OperationResult<T> Fail(int failureReason, string message)
            => new(false, failureReason: failureReason, message: message);

        public static OperationResult<T> Fail(string message)
            => Fail(404, message);

        public static OperationResult<T> Fail(int failureReason, Exception? error)
            => new(false, failureReason: failureReason, error: error);

        public static OperationResult<T> Fail(IOperationResult result)
        {
            return new(false, default, result.FailureReason, result.Message, result.Error);
        }

        public static implicit operator OperationResult<T>(T value)
            => Ok(value);

        public static implicit operator OperationResult<T>(OperationResult result)
            => new(result.Succeeded, default, result.FailureReason, result.Message, result.Error);

        public static bool operator true(OperationResult<T> result)
            => result.Succeeded;

        public static bool operator false(OperationResult<T> result)
            => !result.Succeeded;

        public static implicit operator bool(OperationResult<T> result)
            => result.Succeeded;
    }
}
