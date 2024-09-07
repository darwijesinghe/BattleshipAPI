using Domain.Enums;
using System.Text.Json.Serialization;

namespace Domain.Models
{
    /// <summary>
    /// Domian class for the shoot result
    /// </summary>
    public class ShootResult
    {
        public ShootResult()
        {
            ShootHistory = new List<ShootPosition>();
            ShipInfo     = new List<Ship>();
            DamagedShip  = string.Empty;
        }

        /// <summary>
        /// Type of shoot
        /// </summary>
        public ShootStatus ShootStatus          { get; set; }

        /// <summary>
        /// Ship that was damaged
        /// </summary>
        public string DamagedShip               { get; set; }

        /// <summary>
        /// Updated infomation of the ships
        /// </summary>
        [JsonIgnore]
        public List<Ship> ShipInfo              { get; set; }

        /// <summary>
        /// Shooted positions and status
        /// </summary>
        public List<ShootPosition> ShootHistory  { get; set; }

    }
}
