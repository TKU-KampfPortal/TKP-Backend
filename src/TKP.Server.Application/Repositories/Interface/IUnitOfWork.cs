namespace TKP.Server.Application.Repositories.Interface
{
    /// <summary>
    /// Defines a unit of work that encapsulates a set of operations to be committed as a single transaction.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {

        /// <summary>
        /// Executes a specified operation within the context of a transaction.
        /// </summary>
        /// <param name="operation">Operation work with transaction</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        Task ExecuteAsync(Func<Task> operation, CancellationToken cancellationToken = default);

    }
}
