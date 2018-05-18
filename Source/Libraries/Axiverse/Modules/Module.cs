﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axiverse.Injection;

namespace Axiverse.Modules
{
    /// <summary>
    /// Represents a loadable module.
    /// </summary>
    public abstract class Module
    {
        public Injector Injector { get; set; }

        protected virtual void Initialize()
        {

        }

        protected void Bind<T>(T value) where T : class
        {
            Injector.Bind(Key.From(typeof(T)), value);
        }

        // bind - to inject
        
        // install - module possibly with parameters
    }
}
