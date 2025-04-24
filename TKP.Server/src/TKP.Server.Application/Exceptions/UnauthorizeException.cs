using TKP.Server.Application.Models;

namespace TKP.Server.Application.Exceptions
{
    public sealed class UnauthorizeException : ApplicationException
    {
        public UnauthorizeException(string message, bool transactionRollback = false) : base(message)
        {
            Code = ApiResultErrorCodes.Unauthorize;
            TransactionRollback = transactionRollback;
        }
    }
}
