using Domain.Models;
using Domain.Response;

namespace Domain.Interfaces
{
    public interface IShootHandle
    {
        /// <summary>
        /// Determines the result of a shot based on the list of ships, the history of previous shots 
        /// and the current position to shoot at. Updates the shot history accordingly.
        /// </summary>
        /// <param name="ships">The ship list of the game.</param>
        /// <param name="history">The previous history of shots.</param>
        /// <param name="position">The position on the grid where the current shot is being made.</param>
        /// <returns>
        /// The <see cref="Result{ShootResult}"/> object which indicates the outcome of the shot.
        /// </returns>
        Task<Result<ShootResult>> ShootResult(List<Ship> ships, ShootResult history, ShootPosition position);

        /// <summary>
        /// Prepares the result based on the current ship and the position. Determines if the ship is hit or sunk, and updates the result accordingly.
        /// </summary>
        /// <param name="ship">The ship that is being checked for a hit or other action.</param>
        /// <param name="position">The position on the grid being targeted or checked.</param>
        /// <returns>
        /// The <see cref="Result"/> object indicating the outcome of the process.
        /// </returns>
        Task<Result> PrepareResult(Ship ship, ShootPosition position);

        /// <summary>
        /// Ensures that the shooted position will fit within the grid boundaries.
        /// </summary>
        /// <param name="position">The starting position on the grid.</param>
        /// <returns>
        /// The <see cref="bool"/> indicating whether the shooted position is valid or not.
        /// </returns>
        Task<bool> IsValidPosition(ShootPosition position);
    }
}
