using System;
using System.Collections.Generic;
using System.Text;

namespace SdlTest.Entities
{
    public class EntityManager
    {
        private readonly LinkedList<Entity> entities = new LinkedList<Entity>();

        public void Add(Entity entity)
        {
            entities.AddLast(entity);
        }

        public void UpdateEntities(int ticksPassed)
        {
            foreach(var entity in entities)
            {
                entity.Update(ticksPassed);
            }
        }

        public void RenderEntities(IntPtr rendererId)
        {
            foreach (var entity in entities)
            {
                entity.Render(rendererId);
            }
        }
    }
}
