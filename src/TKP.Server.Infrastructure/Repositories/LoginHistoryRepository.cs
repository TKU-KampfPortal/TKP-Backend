using TKP.Server.Application.Repositories.Interface;
using TKP.Server.Domain.Entites;
using TKP.Server.Infrastructure.Data;

namespace TKP.Server.Infrastructure.Repositories
{
    public sealed class LoginHistoryRepository : BaseRepository<LoginHistory>, ILoginHistoryRepository
    {
        public LoginHistoryRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
