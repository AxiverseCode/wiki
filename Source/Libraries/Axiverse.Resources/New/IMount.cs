﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Resources.New
{
    public interface IMount
    {
        IMount Parent { get; }
        string RootPath { get; }
    }
}