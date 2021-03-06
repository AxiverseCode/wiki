﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Resources
{
    /// <summary>
    /// Represents an cached object.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Cached<T>
    {
        /// <summary>
        /// Gets the path used to load the cached object.
        /// </summary>
        public Uri Uri { get; set; }

        /// <summary>
        /// Gets the number of references to the cached object.
        /// </summary>
        public int References { get; internal set; } = 1;

        /// <summary>
        /// Gets the value of the cached object.
        /// </summary>
        public T Value
        {
            get
            {
                return value;
            }
            set => this.value = value;
        }

        /// <summary>
        /// Constructs a cached object.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="value"></param>
        public Cached(Uri uri, T value)
        {
            Uri = uri;
            this.value = value;
        }

        private T value;
    }
}
