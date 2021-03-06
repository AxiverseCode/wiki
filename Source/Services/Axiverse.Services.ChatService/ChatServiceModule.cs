﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Grpc.Core;
using Axiverse.Modules;
using Axiverse.Injection;

namespace Axiverse.Services.ChatService
{
    [Dependency(typeof(ServerModule))]
    public class ChatServiceModule : Module
    {
        [Bind]
        Server server;

        protected override void Initialize()
        {
            server.Services.Add(Proto.ChatService.BindService(new ChatServiceImpl()));
        }
    }
}
