﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axiverse.Resources;
using Axiverse.Interface.Assets.Models;

namespace Axiverse.Interface.Assets
{
    public class AssetsModule : Modules.Module
    {
        protected override void Initialize()
        {
            var cache = Injector.Resolve<Cache>();
            //cache.Register(new ObjLoader());
        }
    }
}
