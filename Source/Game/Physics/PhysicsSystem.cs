using Game.Entities;
using Game.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Physics
{
    public class PhysicsSystem
    {
        private readonly HashSet<PhysicsComponent> components = new HashSet<PhysicsComponent>();

        public PhysicsComponent CreateComponent(Entity entity)
        {
            var newComponent = new PhysicsComponent(entity);
            components.Add(newComponent);
            return newComponent;
        }

        public void DisposeComponent(PhysicsComponent component)
        {
            components.Remove(component);
        }

        public IEnumerable<PhysicsComponent> FindComponents(Rect area)
        {
            foreach (var component in components)
            {
                if (component.GetBoundingBox().Intersects(area))
                    yield return component;
            }
        }
    }
}
