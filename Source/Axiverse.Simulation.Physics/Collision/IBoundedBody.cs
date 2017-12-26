﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Axiverse.Mathematics;

namespace Axiverse.Simulation.Physics.Collision
{
    public interface IBoundedBody
    {
        Bounds3 Bounds { get; }
        bool IsUnaffected { get; }
    }
}
