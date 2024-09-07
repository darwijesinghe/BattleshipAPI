using Domain.Enums;
using Domain.Models;
using Domain.Response;
using Domain.Helpers;
using Domain.Interfaces;

namespace Domain.Handling
{
    /// <summary>
    /// Domain class to handle the ship specific cases
    /// </summary>
    public class ShipHandle : IShipHandle
    {
        /// <summary>
        /// Default row count
        /// </summary>
        private const int _rows    = 10;

        /// <summary>
        /// Default column count
        /// </summary>
        private const int _columns = 10;

        public ShipHandle()
        {
            _ships = new List<Ship>();
        }

        /// <summary>
        /// Holds newly created ships for temporary
        /// </summary>
        private List<Ship> _ships { get; set; }

        /// <summary>
        /// Retrieves a list of ships based on the provided ship types
        /// </summary>
        /// <param name="shipTypes">A list of ship types to be used in creating</param>
        /// <returns>
        /// The <see cref="Result{List{Ship}}"/> which includes the list of ships based on the provided types
        /// </returns>
        public async Task<Result<List<Ship>>> ShipList(List<ShipType> shipTypes)
        {
            try
            {
                // going through the ship types
                foreach (var type in shipTypes)
                {
                    // instance for hold the palced ship list
                    var result = new Result<Ship>();

                    // execute until get the successful result.
                    // locations are generated randomly.
                    // random result might be out of 10 * 10 grid with the direction.
                    // therefore need to execute the method until we get the valid positions.

                    do
                    {
                        result = await this.PlaceShip(type);
                        if (result.Success)
                            _ships.Add(result.Data);
                    }
                    while (!result.Success);
                }

                // returns result
                return new Result<List<Ship>> { Success = true, Data = _ships };
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Places a ship on the grid. The method will determine a valid position and direction for the ship on the grid, ensuring no overlap or boundary issues
        /// </summary>
        /// <param name="shipType">The type of the ship to be placed</param>
        /// <returns>
        /// The <see cref="Result{T}"/> object that includes the placed <see cref="Ship"/> if successful; otherwise, error message if placement fails
        /// </returns>
        public async Task<Result<Ship>> PlaceShip(ShipType shipType)
        {
            try
            {
                // creates a new ship
                var newShip = this.BuildShip(shipType);
                if (string.IsNullOrEmpty(newShip.ShipName))
                    return new Result<Ship> { Message = "Ship not found." };

                // creates a location to place
                var direction = Helper.Direction();
                var position  = new ShipPosition(Helper.Location(), Helper.Location(), direction);

                // validates the ship area
                if (!await IsValidPosition(position, newShip.ShipSize))
                    return new Result<Ship> { Message = "Not enough space found." };

                // holds the ship positions result
                var result = new Result<List<ShipPosition>>();

                // places the ship according to the position and direction
                switch (direction)
                {
                    case ShipDirection.Up   :
                        result = await PlaceUp(position, newShip.ShipSize);
                        break;

                    case ShipDirection.Down :
                        result = await PlaceDown(position, newShip.ShipSize);
                        break;

                    case ShipDirection.Right:
                        result = await PlaceRight(position, newShip.ShipSize);
                        break;

                    case ShipDirection.Left :
                        result = await PlaceLeft(position, newShip.ShipSize);
                        break;

                    default:
                        return new Result<Ship> { Message = "Direction not found." };
                }

                // validates the result
                if (!result.Data.HasValue())
                    return new Result<Ship> { Message = result.Message };

                // newly placed ship
                newShip.ShipPositions = result.Data;

                // returns result
                return new Result<Ship> { Success = true, Data = newShip };
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Builds and returns a ship based on the specified ship type.
        /// This method generates a ship object with properties specific to the provided ship type.
        /// </summary>
        /// <param name="type">The type of ship to be created</param>
        /// <returns>
        /// A <see cref="Ship"/> object that is constructed based on the provided ship type
        /// </returns>
        public Ship BuildShip(ShipType type)
        {
            try
            {
                // creates the ship based on the ship type
                switch (type)
                {
                    case ShipType.Battleship     :
                        return new Ship(ShipType.Battleship, ShipSize.Battleship);
                    case ShipType.Destroyer      :
                        return new Ship(ShipType.Destroyer, ShipSize.Destroyer);
                    case ShipType.DestroyerBackup:
                        return new Ship(ShipType.DestroyerBackup, ShipSize.DestroyerBackup);
                    default:
                        return new Ship();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Places the ship vertically upwards from the given starting position
        /// </summary>
        /// <param name="position">The starting position on the grid where the placement begins</param>
        /// <param name="count">The number of positions to place vertically upwards from the starting position</param>
        /// <returns>
        /// The <see cref="Result{List{ShipPosition}}"/> that includes the list of positions
        /// </returns>
        public async Task<Result<List<ShipPosition>>> PlaceUp(ShipPosition position, int count)
        {
            try
            {
                // instance to hold the position temporary
                var shipPosition = new List<ShipPosition>();

                // iterate over a range of rows in the grid, starting from a specific row and moving upwards based on a given count

                for (int row = position.Row; row > (position.Row - count); row--)
                {
                    // creates a new position
                    var newPosition = new ShipPosition(row, position.Column, position.Direction);

                    // checks the grid boundires
                    if (!await IsValidPosition(newPosition, count))
                        return new Result<List<ShipPosition>> { Message = "Not enough space found." };

                    // if overlapped
                    if (await IsOverlapped(newPosition))
                        return new Result<List<ShipPosition>> { Message = "Overlap detected." };

                    // adds positions to the list
                    shipPosition.Add(newPosition);
                }

                // returns result
                return new Result<List<ShipPosition>> { Message = "OK", Success = true, Data = shipPosition };
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Places the ship vertically downwards from the given starting position
        /// </summary>
        /// <param name="position">The starting position on the grid where the placement begins</param>
        /// <param name="count">The number of positions to place vertically downwards from the starting position</param>
        /// <returns>
        /// The <see cref="Result{List{ShipPosition}}"/> that includes the list of positions
        /// </returns>
        public async Task<Result<List<ShipPosition>>> PlaceDown(ShipPosition position, int count)
        {
            try
            {
                // instance to hold the position temporary
                var shipPosition = new List<ShipPosition>();

                // iterate over a range of rows in the grid, starting from a specific row and moving downwards based on a given count

                for (int row = position.Row; row < (position.Row + count); row++)
                {
                    // creates a new position
                    var newPosition = new ShipPosition(row, position.Column, position.Direction);

                    // checks the grid boundires
                    if (!await IsValidPosition(newPosition, count))
                        return new Result<List<ShipPosition>> { Message = "Not enough space found." };

                    // if overlapped
                    if (await IsOverlapped(newPosition))
                        return new Result<List<ShipPosition>> { Message = "Overlap detected." };

                    // adds positions to the list
                    shipPosition.Add(newPosition);
                }

                // returns result
                return new Result<List<ShipPosition>> { Message = "OK", Success = true, Data = shipPosition };
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Places the ship horizontally rightward from the given starting position
        /// </summary>
        /// <param name="position">The starting position on the grid where the placement begins</param>
        /// <param name="count">The number of positions to place horizontally rightward from the starting position</param>
        /// <returns>
        /// The <see cref="Result{List{ShipPosition}}"/> that includes the list of positions
        /// </returns>
        public async Task<Result<List<ShipPosition>>> PlaceRight(ShipPosition position, int count)
        {
            try
            {
                // instance to hold the position temporary
                var shipPosition = new List<ShipPosition>();

                // iterate over a range of columns in the grid, starting from a specific column and moving rightward based on a given count

                for (int col = position.Column; col < (position.Column + count); col++)
                {
                    // creates a new position
                    var newPosition = new ShipPosition(position.Row, col, position.Direction);

                    // checks the grid boundires
                    if (!await IsValidPosition(newPosition, count))
                        return new Result<List<ShipPosition>> { Message = "Not enough space found." };

                    // if overlapped
                    if (await IsOverlapped(newPosition))
                        return new Result<List<ShipPosition>> { Message = "Overlap detected." };

                    // adds positions to the list
                    shipPosition.Add(newPosition);
                }

                // returns result
                return new Result<List<ShipPosition>> { Message = "OK", Success = true, Data = shipPosition };
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Places the ship horizontally leftward from the given starting position
        /// </summary>
        /// <param name="position">The starting position on the grid where the placement begins</param>
        /// <param name="count">The number of positions to place horizontally leftward from the starting position</param>
        /// <returns>
        /// The <see cref="Result{List{ShipPosition}}"/> that includes the list of positions
        /// </returns>
        public async Task<Result<List<ShipPosition>>> PlaceLeft(ShipPosition position, int count)
        {
            try
            {
                // instance to hold the position temporary
                var shipPosition = new List<ShipPosition>();

                // iterate over a range of columns in the grid, starting from a specific column and moving leftward based on a given count

                for (int col = position.Column; col > (position.Column + count); col--)
                {
                    // creates a new position
                    var newPosition = new ShipPosition(position.Row, col, position.Direction);

                    // checks the grid boundires
                    if (!await IsValidPosition(newPosition, count))
                        return new Result<List<ShipPosition>> { Message = "Not enough space found." };

                    // if overlapped
                    if (_ships.Any(a => a.ShipPositions.Contains(newPosition)))
                        return new Result<List<ShipPosition>> { Message = "Overlap detected." };

                    // adds positions to the list
                    shipPosition.Add(newPosition);
                }

                // returns result
                return new Result<List<ShipPosition>> { Message = "OK", Success = true, Data = shipPosition };
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Ensures that the ship will fit within the grid boundaries based on the provided position and length
        /// </summary>
        /// <param name="position">The starting position on the grid</param>
        /// <param name="length">The length of the ship to be placed</param>
        /// <returns>
        /// The <see cref="bool"/> indicating whether the position is valid for placing the ship
        /// </returns>
        public Task<bool> IsValidPosition(ShipPosition position, int length)
        {
            try
            {
                // validates postions based on the direction of the ship and checks if the ship can fit within the grid boundaries

                switch (position?.Direction)
                {
                    case ShipDirection.Right:
                        return Task.FromResult(position.Column + length <= _columns);

                    case ShipDirection.Left :
                        return Task.FromResult(position.Column - length + 1 >= 0);

                    case ShipDirection.Up   :
                        return Task.FromResult(position.Row - length + 1 >= 0);

                    case ShipDirection.Down :
                        return Task.FromResult(position.Row + length <= _rows);

                    default                 :
                        return Task.FromResult(false);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Checks whether the given position overlaps with any existing ships on the grid
        /// </summary>
        /// <param name="position">The <see cref="ShipPosition"/> to check for overlap</param>
        /// <returns>
        /// The <see cref="bool"/> indicating whether the overlapped or not
        /// </returns>
        public Task<bool> IsOverlapped(ShipPosition position)
        {
            try
            {
                // checks the overlapping
                if (_ships.Any(ship => ship.ShipPositions.Contains(position)))
                    return Task.FromResult(true);

                return Task.FromResult(false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
