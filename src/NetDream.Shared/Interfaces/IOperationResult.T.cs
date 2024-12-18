using System;
using System.Diagnostics.CodeAnalysis;

namespace NetDream.Shared.Interfaces
{
    public interface IOperationResult<T>
    {
        [MemberNotNullWhen(true, "Result")]
        public bool Succeeded { get; }
        public T? Result { get; }
        

        public int FailureReason { get; }

        public string Message { get; }

        public Exception? Error { get; }

    }
}
