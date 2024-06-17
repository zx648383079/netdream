using NetDream.Core.Interfaces.Database;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Core.Providers
{
    public class ScoreProvider(IDatabase db, string prefix) : IMigrationProvider
    {
        public void Migration(IMigration migration)
        {
            throw new NotImplementedException();
        }
    }
}
