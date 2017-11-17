﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;
using Xaria.Drops;
using Xaria.Enemies;

namespace Xaria
{
    /// <summary>
    /// The Level class
    /// </summary>
    public class Level
    {
        /// <summary>
        /// Gets the difficulty.
        /// </summary>
        /// <value>
        /// The difficulty.
        /// </value>
        public int Difficulty { get; private set; }
        /// <summary>
        /// The enemies
        /// </summary>
        internal List<List<Enemy>> Enemies = new List<List<Enemy>>();
        /// <summary>
        /// The spacing between enemies
        /// </summary>
        private readonly Vector2 spacing = new Vector2(50, 30);
        /// <summary>
        /// The projectiles of enemies
        /// </summary>
        internal List<Projectile> Projectiles = new List<Projectile>();

        internal List<Drop> Drops = new List<Drop>();
        /// <summary>
        /// Bool for enemy movements to right or left
        /// </summary>
        private bool movingRight = true;
        /// <summary>
        /// The enemies per row
        /// </summary>
        private const int ENEMIES_PER_ROW = 7;
        /// <summary>
        /// A random class for determining enemy shooting
        /// </summary>
        public static Random random = new Random();


        /// <summary>
        /// Initializes a new instance of the <see cref="Level" /> class.
        /// </summary>
        /// <param name="difficulty">The difficulty.</param>
        public Level(int difficulty)
        {
            Difficulty = difficulty;
            GenerateLevel(Difficulty);
        }

        /// <summary>
        /// Generates the level.
        /// </summary>
        /// <param name="difficulty">The difficulty.</param>
        private void GenerateLevel(int difficulty)
        {
            Enemies.Clear();
            if (difficulty % 5 == 0)
            {
                Enemies.Add(new List<Enemy>());
                if(difficulty/5 == 1)
                {
                    Enemies[0].Add(new Boss1(new Vector2(Game1.textureDictionary["Boss1"].Width, Game1.textureDictionary["Boss1"].Height)));
                }
                else if(difficulty/5 == 2)
                {
                    Enemies[0].Add(new Boss2(new Vector2(Game1.textureDictionary["Boss2"].Width, Game1.textureDictionary["Boss2"].Height)));
                }
                else if(difficulty/5 == 3)
                {
                    Enemies[0].Add(new Boss3(new Vector2(Game1.textureDictionary["Boss3"].Width, Game1.textureDictionary["Boss3"].Height)));
                }
                else if(difficulty/5 == 4)
                {
                    Enemies[0].Add(new Boss4(new Vector2(Game1.textureDictionary["Boss4"].Width, Game1.textureDictionary["Boss4"].Height)));
                }
            }
            else
            {
                for (int i = 1; i <= difficulty; i++) //use rows for difficulty
                {
                    Enemies.Add(new List<Enemy>());
                    for (int x = 1; x <= ENEMIES_PER_ROW; x++) //10 enemies per row
                        Enemies[i - 1].Add(new Basic(new Vector2((Game1.textureDictionary["basic"].Width + spacing.X) * x - spacing.X + Game1.textureDictionary["basic"].Width * (i % 2), (Game1.textureDictionary["basic"].Height + spacing.Y) * i + spacing.Y)));
                }
            }
        }

        /// <summary>
        /// Updates the Level's player, enemy projectiles, and enemies.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        /// <param name="touchCollection">The touch collection.</param>
        internal void Update(GameTime gameTime,TouchLocation[] touches)
        {
            Game1.player.Update(touches, ref Enemies);
            Game1.player.Shoot(gameTime);
            UpdateEnemies(gameTime);
            UpdateEnemyProjectiles();
            UpdateDrops();
            if (Game1.player.Health <= 0)
                GameOver();
        }

        private void UpdateDrops()
        {
            for (int dropIndex = Drops.Count - 1; dropIndex >= 0; dropIndex--)
            {
                Drops[dropIndex].Position += Drops[dropIndex].Velocity;
                if (Game1.player.Intersects(Drops[dropIndex]))
                {
                    Drops[dropIndex].OnReceive(ref Game1.player);
                    Drops.RemoveAt(dropIndex);
                }
            }
        }

        private void UpdateEnemies(GameTime gameTime)
        {
            if (Enemies.Count == 0)
                NextLevel();
            if(Difficulty%5 ==0 )
            {
                Enemy enemy = Enemies[0][0];
                if(Difficulty/5 == 1)
                {
                    enemy.Position = new Vector2(random.Next(0, ((int)Game1.screenSize.X - enemy.Texture.Width)), random.Next((500 + enemy.Texture.Height), (int)Game1.screenSize.Y));
                }
                else if(Difficulty/5 == 2)
                {
                    if (movingRight)
                    {
                        enemy.Position.X += 4;
                        if (enemy.Position.X + enemy.Texture.Width >= Game1.screenSize.X)
                        {
                            movingRight = !movingRight;
                        }
                    }
                    else
                    {
                        enemy.Position.X -= 4;
                        if (enemy.Position.X <= 0)
                        {
                            movingRight = !movingRight;
                        }
                    }
                }
                else if(Difficulty/5 == 3)
                {
                   /*     NextShoot -= gameTime.ElapsedGameTime.Milliseconds;
                    if (NextShoot <= 0)
                    {
                        NextShoot = Level.random.Next(1000, 10000);
                        Projectiles.Add(new Laser(Position + new Vector2(Texture.Width / 2f - 1f, Texture.Height + 5f), new Vector2(0, 20), 20)); //moving up
                    }*/
                }
                else if(Difficulty/5 == 4)
                {

                }
            }
            else
            {
                for (int rowIndex = Enemies.Count - 1; rowIndex >= 0; rowIndex--) //move right to left then move down
                {
                    if (Enemies[rowIndex].Count == 0)
                    {
                        Enemies.RemoveAt(rowIndex);
                        continue;
                    }
                    for (int enemyIndex = Enemies[rowIndex].Count - 1; enemyIndex >= 0; enemyIndex--)
                    {
                        Enemy enemy = Enemies[rowIndex][enemyIndex];
                        if (enemy.Health <= 0)
                        {
                            Enemies[rowIndex][enemyIndex].OnDeath();
                            Enemies[rowIndex].RemoveAt(enemyIndex);
                            continue;
                        }

                        enemy.Shoot(gameTime, ref Projectiles);
                        if (movingRight)
                        {
                            enemy.Position.X += 1;
                            if (enemy.Position.X + enemy.Texture.Width >= Game1.screenSize.X)
                            {
                                movingRight = !movingRight;
                                MoveDown();
                            }
                        }
                        else
                        {
                            enemy.Position.X -= 1;
                            if (enemy.Position.X <= 0)
                            {
                                movingRight = !movingRight;
                                MoveDown();
                            }
                        }
                    }
                }
            }
        }

        private void UpdateEnemyProjectiles()
        {
            for (int projectileIndex = Projectiles.Count - 1; projectileIndex >= 0; projectileIndex--)
            {
                Projectiles[projectileIndex].Position += Projectiles[projectileIndex].Velocity;
                if (Game1.player.Intersects(Projectiles[projectileIndex]))
                {
                    Projectiles[projectileIndex].OnCollision(ref Game1.player);
                    Projectiles.RemoveAt(projectileIndex);
                }
            }
        }

        /// <summary>
        /// Goes to the next level.
        /// </summary>
        private void NextLevel()
        {
            Difficulty++;
            GenerateLevel(Difficulty);
        }

        /// <summary>
        /// Draws the Level's enemies and enemy projectiles.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch.</param>
        internal void Draw(ref SpriteBatch spriteBatch)
        {
            foreach (List<Enemy> row in Enemies)
                foreach (Enemy enemy in row)
                {
                    enemy.Draw(ref spriteBatch);
                }
            foreach (Projectile projectile in Projectiles)
            {
                projectile.Draw(ref spriteBatch, Color.Red);
            }
            foreach(Drop drop in Drops)
            {
                drop.Draw(ref spriteBatch);
            }
        }

        /// <summary>
        /// Moves the enemies down.
        /// </summary>
        private void MoveDown()
        {
            if (Difficulty % 5 == 0)
            {

            }
            else
            {
                foreach (List<Enemy> row in Enemies)
                    foreach (Enemy enemy in row)
                    {
                        enemy.Position.Y += (enemy.Texture.Height + spacing.Y);
                        if (enemy.Position.Y >= Game1.screenSize.Y - Game1.textureDictionary["ship"].Height - 5)
                        {
                            GameOver();
                        }
                    }
            }
        }

        /// <summary>
        /// Ends the game
        /// </summary>
        private void GameOver()
        {
            Game1.state = GameState.End;
        }

        internal void AddDrop(Drop drop)
        {
            Drops.Add(drop);
        }
    }
}