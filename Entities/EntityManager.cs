using SdlTest.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace SdlTest.Entities
{
    public class EntityManager
    {
        private readonly LinkedList<Entity> entities = new LinkedList<Entity>();
        private readonly List<Entity> disposableEntities = new List<Entity>();
        private readonly List<Entity> newEntities = new List<Entity>();

        public void Add(Entity entity)
        {
            newEntities.Add(entity);
        }

        public void UpdateEntities(int ticksPassed)
        {
            foreach(var entity in entities)
            {
                entity.Update(ticksPassed);

                if (entity.Disposable)
                    disposableEntities.Add(entity);
            }

            foreach(var entity in disposableEntities)
            {
                entities.Remove(entity);
                entity.Disposed = true;
            }

            foreach(var newEntity in newEntities)
                entities.AddLast(newEntity);

            newEntities.Clear();

            disposableEntities.Clear();
        }

        public void RenderEntities(IntPtr rendererId)
        {
            foreach (var entity in entities)
            {
                entity.Render(rendererId);
            }
        }

        public IEnumerable<Entity> FindEntities(Rect area)
        {
            foreach(var entity in entities)
            {
                if (!entity.Disposable && entity.GetBoundingBox().Intersects(area))
                    yield return entity;
            }
        }
    }
}
