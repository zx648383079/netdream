using NetDream.Shared.Interfaces;
using System;

namespace NetDream.Modules.Document.Repositories
{
    public class MockRepository(DocumentContext db, IClientContext client)
    {

        internal object GetDefaultData(int id)
        {
            throw new NotImplementedException();
        }
    }
}