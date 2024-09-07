namespace Domain.Enums
{
    /// <summary>
    /// Shoot status enum class
    /// </summary>
    public enum ShootStatus
    {
        /// <summary>
        /// Invalid hit. May be out of boundry
        /// </summary>
        Invalid,

        /// <summary>
        /// Same hit like previously shooted
        /// </summary>
        Same,

        /// <summary>
        /// Missed fire
        /// </summary>
        Miss,

        /// <summary>
        /// Hit to the ship
        /// </summary>
        Hit,

        /// <summary>
        /// Ship has been sunk by this hit
        /// </summary>
        Sunk,

        /// <summary>
        /// All down
        /// </summary>
        Won
    }
}
