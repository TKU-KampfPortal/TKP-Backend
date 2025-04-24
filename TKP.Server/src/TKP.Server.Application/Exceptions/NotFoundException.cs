using TKP.Server.Application.Models;

namespace TKP.Server.Application.Exceptions
{
    public sealed class NotFoundException : ApplicationException
    {
        public NotFoundException(string message, bool transactionRollback = false) : base(message)
        {
            Code = ApiResultErrorCodes.NotFound;
            TransactionRollback = transactionRollback;
        }
    }
}
