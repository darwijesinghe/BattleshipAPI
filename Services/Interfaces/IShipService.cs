using Domain.Models;
using Domain.Response;

namespace Services.Interfaces
{
    public interface IShipService
    {
        /// <summary>
        /// Retrieves a list of ships.
        /// </summary>
        /// <param name="consumer">The consumer application unique value.</param>
        /// <returns>
        /// The <see cref="Result{List{Ship}}"/> object which includes a list of ships if the operation is successful.
        /// </returns>
        Task<Result<List<Ship>>> GetShipList(string consumer);

    }
}
