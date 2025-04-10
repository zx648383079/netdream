﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace NetDream.Web.Areas.Chat.WebSockets
{
    public class WebSocketItem
    {

        public WebSocket Socket { get; private set; }

        public Dictionary<string, object> Items { get; private set; } = [];

        public WebSocketItem(WebSocket socket)
        {
            Socket = socket;
        }
    }
}
