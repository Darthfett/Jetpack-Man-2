﻿using MonoGame.Extended.Maps.Tiled;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetpackMan
{
    class EntityManager
    {
        static List<IEntity> entities = new List<IEntity>();

        public EntityManager() { }

        public static void AddEntity(IEntity entity)
        {
            Debug.Assert(!entities.Contains(entity));
            entities.Add(entity);
        }

        public static void Update(TiledMap map)
        {
            entities.RemoveAll(item => item.IsDestroyed());

            foreach (var entity in entities)
            {
                entity.Update(map);
            }
        }
    }
}
