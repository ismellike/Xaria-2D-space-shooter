﻿using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Xaria.Drops;
using Xaria.Projectiles;

namespace Xaria.Enemies
{
    /// <summary>
    /// The basic enemy class.
    /// </summary>
    /// <seealso cref="Xaria.Enemy" />
    class Basic : Enemy
    {
        /// <summary>
        /// The health
        /// </summary>
        const int HEALTH = 100;
        /// <summary>
        /// The next
        /// </summary>
        const int NEXT = 13000;
        /// <summary>
        /// The first
        /// </summary>
        const int FIRST = 3000;
        /// <summary>
        /// The laser DMG
        /// </summary>
        const int LASER_DMG = 20;
        /// <summary>
        /// The shield strength
        /// </summary>
        const int SHIELD_STRENGTH = 50;

        /// <summary>
        /// Initializes a new instance of the <see cref="Basic" /> class.
        /// </summary>
        /// <param name="position">The position.</param>
        public Basic(Vector2 position)
        {
            Health = HEALTH;
            Position = position;
            NextShoot = Level.random.Next(FIRST, NEXT);
            Texture = Game1.textureDictionary["basic"];
        }
        /// <summary>
        /// Shoots a projectile from the enemy.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        /// <param name="Projectiles">The projectiles.</param>
        public override void Shoot(GameTime gameTime, ref List<Projectile> Projectiles)
        {
            NextShoot -= gameTime.ElapsedGameTime.Milliseconds;
            if(NextShoot<= 0)
            {
                NextShoot = Level.random.Next(1000, NEXT);
                Projectiles.Add(new Laser(Position + new Vector2(Texture.Width / 2f, Texture.Height+ 5f), new Vector2(0, 20), LASER_DMG)); //moving down
            }
        }

        /// <summary>
        /// Called when [death].
        /// </summary>
        /// <param name="drops">The drops.</param>
        internal override void OnDeath(ref List<Drop> drops)
        {
            if(Level.random.Next(20) == 1) // 1/x chance of giving drop
            {
                 drops.Add(new Shield(Position + new Vector2(Texture.Width/2f, Texture.Height), SHIELD_STRENGTH));
            }
        }

        /// <summary>
        /// Updates the movement.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <param name="gameTime">The game time.</param>
        public override void UpdateMovement(Level level, GameTime gameTime = null)
        {
            if (level.movingRight)
            {
                Position.X += 1;
                if (Position.X + Texture.Width >= Game1.screenSize.X)
                {
                    level.movingRight = !level.movingRight;
                    level.MoveDown();
                }
            }
            else
            {
                Position.X -= 1;
                if (Position.X <= 0)
                {
                    level.movingRight = !level.movingRight;
                    level.MoveDown();
                }
            }
        }
    }
}