using System;

namespace NetDream.Shared.Interfaces
{
    public interface IOperationResult
    {
        public bool Succeeded { get; }

        public int FailureReason { get; }

        public string Message { get; }

        public Exception? Error { get; }
    }
}
