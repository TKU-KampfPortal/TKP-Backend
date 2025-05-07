using TKP.Server.Application.Models;

namespace TKP.Server.Application.Exceptions
{
    public sealed class InvalidModelException : ApplicationException
    {
        public InvalidModelException(string message, bool transactionRollback = false) : base(message)
        {
            Code = ApiResultErrorCodes.ModelValidation;
            TransactionRollback = transactionRollback;
        }
    }
}
