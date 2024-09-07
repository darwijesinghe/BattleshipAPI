using Domain.Enums;

namespace Services.Helpers
{
    /// <summary>
    /// Class for the helper mothods.
    /// </summary>
    public class Helper
    {
        /// <summary>
        /// Random class instance.
        /// </summary>
        private static Random random = new Random();

        /// <summary>
        /// Randomly picks the direction to place the ship (squars)
        /// </summary>
        /// <returns>
        /// A <see cref="ShipDirection"/> enum value
        /// </returns>
        public static ShipDirection Direction()
        {
            switch (random.Next(1, 4))
            {
                case 1:
                    return ShipDirection.Up;
                case 2:
                    return ShipDirection.Down;
                case 3:
                    return ShipDirection.Left;
                case 4:
                    return ShipDirection.Right;
                default:
                    return ShipDirection.NotSet;
            }
        }

        /// <summary>
        /// Randomly picks the number to decide the ship location
        /// </summary>
        /// <returns>
        /// A randomly picked number
        /// </returns>
        public static int Location()
        {
            return random.Next(1, 10);
        }
    }
}
