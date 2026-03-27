using NetDream.Shared.Interfaces;
using NetDream.Shared.Interfaces.Entities;
using System;
using System.Collections.Generic;

namespace NetDream.Modules.ResourceStore.Repositories
{
    public class AttachRepository(ResourceContext db) : IAttachRepository
    {
        public void Add(byte itemType, int itemId, byte fileType, string file)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IAttachment> Get(byte itemType, int itemId)
        {
            throw new NotImplementedException();
        }

        public IAttachment Get(int id)
        {
            throw new NotImplementedException();
        }

        public void Remove(byte itemType, int itemId)
        {
            throw new NotImplementedException();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(int id, byte fileType, string file)
        {
            throw new NotImplementedException();
        }
    }
}
