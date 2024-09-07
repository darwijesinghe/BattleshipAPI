using Domain.Enums;
using Domain.Models;
using Domain.Response;
namespace Domain.Interfaces
{
    public interface IShipHandle
    {
        /// <summary>
        /// Retrieves a list of ships based on the provided ship types
        /// </summary>
        /// <param name="shipTypes">A list of ship types to be used in creating</param>
        /// <returns>
        /// The <see cref="Result{List{Ship}}"/> which includes the list of ships based on the provided types
        /// </returns>
        Task<Result<List<Ship>>> ShipList(List<ShipType> shipTypes);

        /// <summary>
        /// Builds and returns a ship based on the specified ship type.
        /// This method generates a ship object with properties specific to the provided ship type.
        /// </summary>
        /// <param name="type">The type of ship to be created</param>
        /// <returns>
        /// A <see cref="Ship"/> object that is constructed based on the provided ship type
        /// </returns>
        Ship BuildShip(ShipType type);

        /// <summary>
        /// Places a ship on the grid. The method will determine a valid position and direction for the ship on the grid, ensuring no overlap or boundary issues
        /// </summary>
        /// <param name="shipType">The type of the ship to be placed</param>
        /// <returns>
        /// The <see cref="Result{T}"/> object that includes the placed <see cref="Ship"/> if successful; otherwise, error message if placement fails
        /// </returns>
        Task<Result<Ship>> PlaceShip(ShipType shipType);

        /// <summary>
        /// Places the ship vertically upwards from the given starting position
        /// </summary>
        /// <param name="position">The starting position on the grid where the placement begins</param>
        /// <param name="count">The number of positions to place vertically upwards from the starting position</param>
        /// <returns>
        /// The <see cref="Result{List{ShipPosition}}"/> that includes the list of positions
        /// </return
        Task<Result<List<ShipPosition>>> PlaceUp(ShipPosition position, int count);

        /// <summary>
        /// Places the ship vertically downwards from the given starting position
        /// </summary>
        /// <param name="position">The starting position on the grid where the placement begins</param>
        /// <param name="count">The number of positions to place vertically downwards from the starting position</param>
        /// <returns>
        /// The <see cref="Result{List{ShipPosition}}"/> that includes the list of positions
        /// </returns>
        Task<Result<List<ShipPosition>>> PlaceDown(ShipPosition position, int count);

        /// <summary>
        /// Places the ship horizontally rightward from the given starting position
        /// </summary>
        /// <param name="position">The starting position on the grid where the placement begins</param>
        /// <param name="count">The number of positions to place horizontally rightward from the starting position</param>
        /// <returns>
        /// The <see cref="Result{List{ShipPosition}}"/> that includes the list of positions
        /// </returns>
        Task<Result<List<ShipPosition>>> PlaceRight(ShipPosition position, int count);

        /// <summary>
        /// Places the ship horizontally leftward from the given starting position
        /// </summary>
        /// <param name="position">The starting position on the grid where the placement begins</param>
        /// <param name="count">The number of positions to place horizontally leftward from the starting position</param>
        /// <returns>
        /// The <see cref="Result{List{ShipPosition}}"/> that includes the list of positions
        /// </returns>
        Task<Result<List<ShipPosition>>> PlaceLeft(ShipPosition position, int count);

        /// <summary>
        /// Ensures that the ship will fit within the grid boundaries based on the provided position and length
        /// </summary>
        /// <param name="position">The starting position on the grid</param>
        /// <param name="length">The length of the ship to be placed</param>
        /// <returns>
        /// The <see cref="bool"/> indicating whether the position is valid for placing the ship
        /// </returns>
        Task<bool> IsValidPosition(ShipPosition position, int length);

        /// <summary>
        /// Checks whether the given position overlaps with any existing ships on the grid
        /// </summary>
        /// <param name="position">The <see cref="ShipPosition"/> to check for overlap</param>
        /// <returns>
        /// The <see cref="bool"/> indicating whether the overlapped or not
        /// </returns>
        Task<bool> IsOverlapped(ShipPosition position);
    }
}
