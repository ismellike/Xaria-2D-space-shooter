﻿using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Xaria.Projectiles;
using Microsoft.Xna.Framework.Graphics;

namespace Xaria.Enemies
{
    class Boss4 : Enemy
    {
        public Boss4(Vector2 position)
        {
            Health = 5000;
            Position = position;
            Texture = Game1.textureDictionary["boss4"];//texture changes
        }


        internal override void Shoot(GameTime gameTime, ref List<Projectile> Projectiles)
        {
            NextShoot -= gameTime.ElapsedGameTime.Milliseconds;
            if (NextShoot <= 0)
            {
                NextShoot = Level.random.Next(1000, 10000);
            }
        }
    }
}