using Domain.Enums;

namespace Domain.Models
{
    /// <summary>
    /// Domain class for the ship
    /// </summary>
    public class Ship
    {
        public Ship()
        {
            ShipPositions = new List<ShipPosition>();
        }

        public Ship(ShipType type, ShipSize squares)
        {
            ShipType      = type;
            ShipSize      = (int)squares;
            ShipPositions = new List<ShipPosition>();
            Health        = (int)squares;
        }

        /// <summary>
        /// Ship type
        /// </summary>
        public ShipType ShipType                { get; set; }
        
        /// <summary>
        /// Ship name
        /// </summary>
        public string ShipName                  { get { return ShipType.ToString(); } }

        /// <summary>
        /// Ship position. ex: Battleship (RIGHT: 5:5, 5:6, 5:7)
        /// </summary>
        public List<ShipPosition> ShipPositions { get; set; }
       
        /// <summary>
        /// Size (squares) of this ship
        /// </summary>
        public int ShipSize                     { get; set; }

        /// <summary>
        /// Ship health
        /// </summary>
        public int Health                       { get; set; }

        /// <summary>
        /// Indicates whether the ship was sunk or not
        /// </summary>
        public bool IsSunk                      { get; set; } = false;

    }
}
