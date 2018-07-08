﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Axiverse.Simulation.Components;

namespace Axiverse.Simulation
{
    public class Universe
    {
        public Entity this[Guid identifier]
        {
            get => entities[identifier];
        }

        public int Count => entities.Count;

        public Universe()
        {
            Add(new PhysicsProcessor());
        }

        public void Post(Guid target)
        {

        }

        public void Step(float dt)
        {
            SimulationContext context = new SimulationContext()
            {
                DeltaTime = dt
            };

            foreach (var processor in processors.Values)
            {
                processor.Process(context);
            }
        }

        public void Add(Entity entity)
        {
            Requires.That<InvalidOperationException>(!entity.IsAttached);

            entity.Universe = this;
            entities.Add(entity.Identifier, entity);
            OnEntityAdded(entity);
        }

        public bool Remove(Entity entity)
        {
            var result = entities.Remove(entity.Identifier);
            if (result)
            {
                OnEntityRemoved(entity);
            }
            return result;
        }

        protected void OnEntityAdded(Entity entity)
        {
            entity.ComponentAdded += OnComponentAdded;
            foreach (var processor in processors.Values)
            {
                if (processor.Matches(entity))
                {
                    processor.Add(entity);
                }
            }

            EntityAdded?.Invoke(this, new EntityEventArgs(entity));
        }

        protected void OnEntityRemoved(Entity entity)
        {
            entity.ComponentRemoved += OnComponentRemoved;
            foreach (var processor in processors.Values)
            {
                if (processor.ContainsKey(entity.Identifier))
                {
                    processor.Remove(entity);
                }
            }

            EntityRemoved?.Invoke(this, new EntityEventArgs(entity));
        }

        private void OnComponentAdded(object sender, ComponentEventArgs e)
        {
            var entity = sender as Entity;

            foreach (var processor in processors.Values)
            {
                if (!processor.ContainsKey(entity.Identifier) && processor.Matches(entity))
                {
                    processor.Add(entity);
                }
            }
        }

        private void OnComponentRemoved(object sender, ComponentEventArgs e)
        {
            var entity = sender as Entity;

            foreach (var processor in processors.Values)
            {
                if (processor.ContainsKey(entity.Identifier) && !processor.Matches(entity))
                {
                    processor.Remove(entity);
                }
            }
        }

        public void Add(IProcessor processor)
        {
            Processors.Add(processor.Stage, processor);

            foreach (var entity in entities.Values)
            {
                if (processor.Matches(entity))
                {
                    processor.Add(entity);
                }
            }
        }

        public event EntityEventHandler EntityAdded;
        public event EntityEventHandler EntityRemoved;

        public SortedList<ProcessorStage, IProcessor> Processors => processors;
        
        private readonly SortedList<ProcessorStage, IProcessor> processors = new SortedList<ProcessorStage, IProcessor>(new ProcessorComparer());
        private readonly Dictionary<Guid, Entity> entities = new Dictionary<Guid, Entity>();

        private class ProcessorComparer : IComparer<ProcessorStage>
        {
            public int Compare(ProcessorStage x, ProcessorStage y)
            {
                int result = x.CompareTo(y);

                if (result == 0)
                    return 1;   // Handle equality as beeing greater
                else
                    return result;
            }
        }
    }
}
