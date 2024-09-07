using Domain.Enums;

namespace Domain.Models
{
    /// <summary>
    /// Domain class for the ship positions
    /// </summary>
    public class ShipPosition
    {
        public ShipPosition() { }

        public ShipPosition(int row, int column)
        {
            this.Row    = row;
            this.Column = column;
        }

        public ShipPosition(int row, int column, ShipDirection direction)
        {
            this.Row       = row;
            this.Column    = column;
            this.Direction = direction;
        }

        /// <summary>
        /// Rows of the grid
        /// </summary>
        public int Row                  { get; set; }

        /// <summary>
        /// Columns of the grid
        /// </summary>
        public int Column               { get; set; }

        /// <summary>
        /// Position direction
        /// </summary>
        public ShipDirection Direction  { get; set; }

        // override Equals method for comparison
        public override bool Equals(object obj)
        {
            if (obj is ShipPosition other)
            {
                return Row == other.Row && Column == other.Column;
            }

            return false;
        }

        // override GetHashCode method to support Equals
        public override int GetHashCode()
        {
            return (Row, Column).GetHashCode();
        }
    }
}
