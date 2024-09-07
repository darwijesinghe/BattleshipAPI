using Domain.Enums;

namespace Domain.Models
{
    /// <summary>
    /// Domain class for the shoot positions
    /// </summary>
    public class ShootPosition
    {
        public ShootPosition(int row, int column)
        {
            this.Row    = row;
            this.Column = column;
        }

        public ShootPosition(int row, int column, ShootStatus status)
        {
            this.Row         = row;
            this.Column      = column;
            this.ShootStatus = status;
        }

        /// <summary>
        /// Shooted row of the grid
        /// </summary>
        public int Row                  { get; set; }

        /// <summary>
        /// Shooted column of the grid
        /// </summary>
        public int Column               { get; set; }

        /// <summary>
        /// Status of the hit
        /// </summary>
        public ShootStatus ShootStatus  { get; set; }

        // override Equals method for comparison
        public override bool Equals(object obj)
        {
            if (obj is ShootPosition other)
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
