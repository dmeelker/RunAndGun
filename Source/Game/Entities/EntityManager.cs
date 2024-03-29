﻿using Game.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Entities
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

        public void Clear()
        {
            entities.Clear();
            disposableEntities.Clear();
            newEntities.Clear();
        }

        public void UpdateEntities(uint time, int ticksPassed)
        {
            foreach(var entity in entities)
            {
                entity.Update(time, ticksPassed);

                if (entity.Disposable)
                    disposableEntities.Add(entity);
            }

            foreach(var entity in disposableEntities)
            {
                entities.Remove(entity);
                entity.Disposed = true;
                entity.OnDisposed();
            }

            foreach(var newEntity in newEntities)
                entities.AddLast(newEntity);

            newEntities.Clear();

            disposableEntities.Clear();
        }

        public void RenderEntities(IntPtr rendererId, Point viewOffset)
        {
            foreach (var entity in entities)
            {
                entity.Render(rendererId, viewOffset);
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
