using TKP.Server.Application.Models;

namespace TKP.Server.Application.Exceptions
{
    public sealed class ForbiddenException : ApplicationException
    {
        public ForbiddenException(string message, bool transactionRollback = false) : base(message)
        {
            Code = ApiResultErrorCodes.Conflict;
            TransactionRollback = transactionRollback;
        }
    }
}
