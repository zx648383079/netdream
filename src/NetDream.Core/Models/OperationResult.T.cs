using NetDream.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Core.Models
{
    public class OperationResult<T> : IOperationResult<T>
    {
        public T? Result { get; private set; }

        public bool IsSuccess { get; private set; }

        public int FailureReason { get; private set; }

        private string? _message;
        public string Message => _message ?? Error?.Message ?? string.Empty;

        public Exception? Error { get; private set; }

        public OperationResult(T result, string? message = null)
        {
            Result = result;
            IsSuccess = true;
            _message = message;
        }

        public OperationResult(int failureReason = FailureReasons.None, string? message = null, Exception? error = null)
        {
            IsSuccess = false;
            _message = message;
            FailureReason = failureReason;
            Error = error;
        }

        public OperationResult(bool success = true, T? result = default, int failureReason = FailureReasons.None, string? message = null, Exception? error = null)
        {
            Result = result;
            IsSuccess = result is not null || success;
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

        public static OperationResult<T> Fail(int failureReason, Exception? error)
            => new(false, failureReason: failureReason, error: error);

        public static implicit operator OperationResult<T>(T value)
            => Ok(value);

        public static implicit operator OperationResult<T>(OperationResult result)
            => new(result.IsSuccess, default, result.FailureReason, result.Message, result.Error);

        public static bool operator true(OperationResult<T> result)
            => result.IsSuccess;

        public static bool operator false(OperationResult<T> result)
            => !result.IsSuccess;

        public static implicit operator bool(OperationResult<T> result)
            => result.IsSuccess;
    }
}
