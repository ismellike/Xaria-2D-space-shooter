﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Xaria.Projectiles
{
    /// <summary>
    /// The rocket class.
    /// </summary>
    /// <seealso cref="Xaria.Projectile" />
    class Rocket : Projectile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Laser" /> class.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="velocity">The velocity.</param>
        /// <param name="damage">The damage.</param>
        public Rocket(Vector2 position, Vector2 velocity, int damage)
        {
            Position = position;
            Velocity = velocity;
            Texture = Game1.textureDictionary["rocket"];
            Damage = damage;
        }

<<<<<<< HEAD
=======
        internal override void OnCollision(ref List<List<Enemy>> Enemies, int y, int x)
        {
            for(int i = y -1; i <= y+1; i++)
            {
                if (i >= Enemies.Count || i < 0)
                    continue;
                for(int j = x -1; j <= x +1; j++)
                {
                    if (j >= Enemies[i].Count || j < 0)
                        continue;
                    Enemies[i][j].Health -= Damage;
                }
            }
            //create explosion effect
        }

>>>>>>> 82cb159b0c63d65d68836bf53f086f87f6a6ee2b
        /* @Pre: rocket projectile overlaps player sprite
         * @Post: players health is reduced by the amount of damage the rocket does
         * @Return: None
         */
        internal override void OnCollision(ref Player player)
        {
            player.Damage(Damage);
        }

        public override void Draw(ref SpriteBatch spriteBatch)
        {
        }
    }
}