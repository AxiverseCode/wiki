﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axiverse.Injection;
namespace Axiverse.Simulation
{
    public class Processor
    {
        public Type[] ComponentTypes { get; }

        public Dictionary<Guid, Entity> Entities { get; } = new Dictionary<Guid, Entity>();

        public Processor(params Type[] componentTypes)
        {
            ComponentTypes = componentTypes;
        }

        public bool ContainsKey(Guid identifier)
        {
            return Entities.ContainsKey(identifier);
        }

        public bool Matches(Entity entity)
        {
            return ComponentTypes.All(t => entity.Components.ContainsKey(t));
        }

        public void Add(Entity entity)
        {
            Entities.Add(entity.Identifier, entity);
            OnEntityAdded(entity);
        }

        public void Remove(Entity entity)
        {
            Entities.Remove(entity.Identifier);
            OnEntityRemoved(entity);
        }

        protected virtual void OnEntityAdded(Entity entity)
        {

        }

        protected virtual void OnEntityRemoved(Entity entity)
        {

        }
    }

    public class Processor<T1> : Processor 
        where T1 : Component
    {
        public Processor() : base(typeof(T1))
        {

        }

        protected virtual void OnEntityAdded(Entity entity, T1 component1)
        {

        }

        protected virtual void OnEntityRemoved(Entity entity, T1 component1)
        {

        }

        protected override void OnEntityAdded(Entity entity)
        {
            base.OnEntityAdded(entity);
            var component1 = entity.Components.Get<T1>();
            OnEntityAdded(entity, component1);
        }

        protected override void OnEntityRemoved(Entity entity)
        {
            base.OnEntityRemoved(entity);
            var component1 = entity.Components.Get<T1>();
            OnEntityRemoved(entity, component1);
        }
    }
}
