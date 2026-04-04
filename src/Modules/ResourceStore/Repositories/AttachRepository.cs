using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
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

        public void Add(ModuleTargetType type, int target, byte fileType, string file)
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

        public IEnumerable<IAttachment> Get(ModuleTargetType type, int target)
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

        public void Remove(ModuleTargetType type, int target)
        {
            throw new NotImplementedException();
        }

        public void Update(int id, byte fileType, string file)
        {
            throw new NotImplementedException();
        }
    }
}
