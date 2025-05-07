using TKP.Server.Application.Models;

namespace TKP.Server.Application.Exceptions
{
    public sealed class ConflictException : ApplicationException
    {
        public ConflictException(string message, bool transactionRollback = false) : base(message)
        {
            Code = ApiResultErrorCodes.Conflict;
            TransactionRollback = transactionRollback;
        }
    }
}
