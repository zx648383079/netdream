using Microsoft.AspNetCore.Http;
using NetDream.Shared.Interfaces;
using System.IO;

namespace NetDream.Api.Base.Http
{
    public class FormUploadFile(IFormFile data) : IUploadFile
    {
        public string Name => data.Name;

        public long Size => data.Length;

        public string FileType => data.ContentType;

        public void CopyTo(Stream output)
        {
            data.CopyTo(output);
        }

        public Stream OpenRead()
        {
            return data.OpenReadStream();
        }
    }
}
