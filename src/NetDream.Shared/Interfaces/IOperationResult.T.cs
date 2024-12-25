using System.Diagnostics.CodeAnalysis;

namespace NetDream.Shared.Interfaces
{
    public interface IOperationResult<T> : IOperationResult
    {
        [MemberNotNullWhen(true, "Result")]
        public new bool Succeeded { get; }
        public T? Result { get; }

    }
}
