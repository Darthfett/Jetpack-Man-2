using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Maps.Tiled;
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

            for(var i = 0; i < entities.Count; i++)
            {
                entities[i].Update(map);
            }

            Console.WriteLine("Count: {0}", entities.Count);
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach(var entity in entities)
            {
                entity.Draw(spriteBatch);
            }
        }
    }
}
