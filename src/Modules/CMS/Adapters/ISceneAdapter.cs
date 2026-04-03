using System;
using System.Collections.Generic;
using System.Text;

namespace NetDream.Modules.CMS.Adapters
{
    public interface ISceneAdapter
    {

        public string MainTableName { get; }
        public string ExtendTableName { get; }
        public string CommentTableName { get; }

        public void Boot();

        public void Initialize();

        public void Destroy();
    }
}
