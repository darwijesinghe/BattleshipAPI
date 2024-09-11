using Domain.Models;
using Domain.Response;

namespace Services.Interfaces
{
    public interface IShootService
    {
        /// <summary>
        /// Retrieves the result of a shoot based on the provided shoot (row and column) position
        /// </summary>
        /// <param name="row">The row position of the shot</param>
        /// <param name="column">The column position of the shot</param>
        /// <param name="consumer">The consumer application unique value</param>
        /// <returns>
        /// The <see cref="Result{ShootResult}"/> object which includes the shoot result
        /// </returns>
        Task<Result<ShootResult>> GetShootResult(int row, int column, string consumer);
    }
}
