﻿using Microsoft.Xna.Framework;

namespace Xaria.Drops
{
    /// <summary>
    /// The life drop class
    /// </summary>
    /// <seealso cref="Xaria.Drops.Drop" />
    class Life : Drop
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="Life"/> class.
        /// </summary>
        /// <param name="position">The position.</param>
        public Life(Vector2 position)
        {
            Position = position;
            Texture = Game1.textureDictionary["life"];
        }

        /// <summary>
        /// Called when [receive].
        /// </summary>
        /// <param name="player">The player.</param>
        public override void OnReceive(ref Player player)
        {
            player.AddLife();
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        /// <remarks>
        /// To be added.
        /// </remarks>
        public override string ToString()
        {
            return "Extra Life";
        }
    }
}