using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NetDream.Modules.MessageService.Repositories
{
    public class MessageRepository(IDatabase db)
    {
        public void InsertIf(string name, string title, string content,
                                    int type = MessageProtocol.TYPE_TEXT)
        {
        }
    }
}
