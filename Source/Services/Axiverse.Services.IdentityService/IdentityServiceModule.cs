﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axiverse.Injection;
using Axiverse.Modules;
using Grpc.Core;

namespace Axiverse.Services.IdentityService
{
    [Dependency(typeof(ServerModule))]
    public class IdentityServiceModule : Module
    {
        [Bind]
        Server server;

        protected override void Initialize()
        {
            server.Services.Add(Proto.IdentityService.BindService(new IdentityServiceImpl()));
        }
    }
}
