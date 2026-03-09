using System.Diagnostics.CodeAnalysis;

namespace NetDream.Shared.Interfaces
{
    public interface IHttpContext
    {
        public bool TryGetHeader(string name, [NotNullWhen(true)] out string? value);
        public bool TryGet(string name, [NotNullWhen(true)] out string? value);
        public bool TryGetFile(string name, [NotNullWhen(true)] out IUploadFile? value);
        public bool TryGetFiles(string name, [NotNullWhen(true)] out IUploadFileCollection? value);
    }
}
