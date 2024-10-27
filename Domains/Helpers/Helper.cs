using Domain.Enums;

namespace Domain.Helpers
{
    /// <summary>
    /// Class for the helper mothods
    /// </summary>
    public static class Helper
    {
        /// <summary>
        /// Random class instance
        /// </summary>
        private static Random random = new Random();

        /// <summary>
        /// Randomly picks the direction to place the ship (squars).
        /// </summary>
        /// <returns>
        /// A <see cref="ShipDirection"/> enum value.
        /// </returns>
        public static ShipDirection Direction()
        {
            // picks the ship placing direction randomly
            switch (random.Next(1, 4))
            {
                case 1 :
                    return ShipDirection.Up;
                case 2 :
                    return ShipDirection.Down;
                case 3 :
                    return ShipDirection.Left;
                case 4 :
                    return ShipDirection.Right;
                default:
                    return ShipDirection.NotSet;
            }
        }

        /// <summary>
        /// Randomly picks the number to decide the ship location.
        /// </summary>
        /// <returns>
        /// A randomly picked <see cref="int"/> number.
        /// </returns>
        public static int Location()
        {
            return random.Next(1, 10);
        }

        /// <summary>
        /// Checks whether the given <see cref="IEnumerable{T}"/> has any non-null elements.
        /// </summary>
        /// <param name="data">The collection of elements to check for non-null values.</param>
        /// <returns>
        /// <c>true</c> if the collection is not null and contains at least one non-null element; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasValue<T>(this IEnumerable<T> data)
        {
            return data != null && data.Any();
        }
    }
}
