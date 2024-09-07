using Domain.Enums;
using Domain.Helpers;
using Domain.Interfaces;
using Domain.Models;
using Domain.Response;

namespace Domain.Handling
{
    /// <summary>
    /// Domain class for all the shoot specific cases
    /// </summary>
    public class ShootHandle : IShootHandle
    {
        // services
        private readonly IShipHandle _shipHandle;

        /// <summary>
        /// Default row count
        /// </summary>
        private const int _rows    = 10;

        /// <summary>
        /// Default column count
        /// </summary>
        private const int _columns = 10;

        public ShootHandle(IShipHandle shipHandle)
        {
            _shipHandle  = shipHandle;
            _shootResult = new ShootResult();
        }

        // holds the shoot result data temporary
        private ShootResult _shootResult { get; set; }

        /// <summary>
        /// Determines the result of a shot based on the list of ships, the history of previous shots, 
        /// and the current position to shoot at. Updates the shot history accordingly.
        /// </summary>
        /// <param name="ships">The ship list of the game</param>
        /// <param name="history">The previous history of shots</param>
        /// <param name="position">The position on the grid where the current shot is being made</param>
        /// <returns>
        /// The <see cref="Result{ShootResult}"/> object which indicates the outcome of the shot
        /// </returns>
        public async Task<Result<ShootResult>> ShootResult(List<Ship> ships, ShootResult history, ShootPosition position)
        {
            try
            {
                // history from cache
                _shootResult          = history;
                _shootResult.ShipInfo = ships;

                // ship data validation
                if (!_shootResult.ShipInfo.HasValue())
                    return new Result<ShootResult> { Message = "No ships found.", Data = _shootResult };

                // going through the ship data
                foreach (var ship in _shootResult.ShipInfo)
                {
                    // validates the hit area
                    if (!await IsValidPosition(position))
                    {
                        _shootResult.ShootStatus = ShootStatus.Invalid;
                        return new Result<ShootResult> { Message = "Invalid position.", Data = _shootResult };
                    }

                    // validates same hit
                    if (_shootResult.ShootHistory.Contains(position))
                    {
                        _shootResult.ShootStatus = ShootStatus.Same;
                        return new Result<ShootResult> { Message = "Same hit found.", Data = _shootResult };
                    }
                        

                    // if current ship sunk be cool, take the next ship
                    if (_shootResult.ShipInfo.Find(x => x.ShipName.Equals(ship.ShipName))?.IsSunk == true)
                        continue;

                    // gets shoot result and break the loop
                    var result = await PrepareResult(ship, position);
                    if (result.Success)
                        break;
                }

                // all ships sunk. game over
                if (_shootResult.ShipInfo.All(x => x.IsSunk == true))
                {
                    _shootResult.ShootStatus = ShootStatus.Won;
                    return new Result<ShootResult> { Message = "Won", Success = true, Data = _shootResult };
                }

                // returns result
                return new Result<ShootResult> { Success = true, Data = _shootResult };
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Prepares the result based on the current ship and the position. Determines if the ship is hit or sunk, and updates the result accordingly.
        /// </summary>
        /// <param name="ship">The ship that is being checked for a hit or other action</param>
        /// <param name="position">The position on the grid being targeted or checked</param>
        /// <returns>
        /// The <see cref="Result"/> object indicating the outcome of the process
        /// </returns>
        public Task<Result> PrepareResult(Ship ship, ShootPosition position)
        {
            try
            {
                // success shot 
                if (ship.ShipPositions.Contains(new ShipPosition(position.Row, position.Column)))
                {
                    // current ship health
                    var health = _shootResult.ShipInfo.Find(x => x.ShipName.Equals(ship.ShipName))?.Health;
                    if (health <= 1)
                    {
                        // shot result
                        _shootResult.ShootStatus  = ShootStatus.Sunk;
                        _shootResult.DamagedShip  = ship.ShipName;

                        // updates the ship health
                        var thisShip = _shootResult.ShipInfo.Find(x => x.ShipName.Equals(ship.ShipName));
                        if(thisShip is not null)
                        {
                            thisShip.Health = 0;
                            thisShip.IsSunk = true;
                        }

                        // shoot history
                        _shootResult.ShootHistory.Add(new ShootPosition(position.Row, position.Column, ShootStatus.Sunk));
                    }
                    // shot was hit
                    else
                    {
                        // shot result
                        _shootResult.ShootStatus   = ShootStatus.Hit;
                        _shootResult.DamagedShip   = ship.ShipName;

                        // updates the ship health
                        var thisShip = _shootResult.ShipInfo.Find(x => x.ShipName.Equals(ship.ShipName));
                        if(thisShip is not null)
                        {
                            thisShip.Health = thisShip.Health - 1;
                            thisShip.IsSunk = false;
                        }

                        // shoot history
                        _shootResult.ShootHistory.Add(new ShootPosition(position.Row, position.Column, ShootStatus.Hit));
                    }
                }
                
                // faild shot
                else
                {
                    // shot was missed. update the shooting history
                    _shootResult.ShootStatus = ShootStatus.Miss;
                    _shootResult.ShootHistory.Add(new ShootPosition(position.Row, position.Column, ShootStatus.Miss));
                }

                // returns result
                return Task.FromResult(new Result { Success = true });
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Ensures that the shooted position will fit within the grid boundaries
        /// </summary>
        /// <param name="position">The starting position on the grid</param>
        /// <returns>
        /// The <see cref="bool"/> indicating whether the shooted position is valid or not
        /// </returns>
        public Task<bool> IsValidPosition(ShootPosition position)
        {
            try
            {
                // checks if the shooted position is within the grid boundaries
                return Task.FromResult(position.Row >= 1 && position.Row <= _rows && position.Column >= 1 && position.Column <= _columns);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
