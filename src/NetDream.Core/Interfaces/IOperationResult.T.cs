using System;

namespace NetDream.Core.Interfaces
{
    public interface IOperationResult<T>
    {
        public T? Result { get; }

        public bool IsSuccess { get; }

        public int FailureReason { get; }

        public string Message { get; }

        public Exception? Error { get; }

    }
}
