using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using TKP.Server.Application.HelperServices;
using TKP.Server.Core.Entities;

namespace TKP.Server.Infrastructure.Data.Interceptor
{
    public class ContextSaveChangeInterceptor : SaveChangesInterceptor
    {
        private readonly IClaimService _claimService;

        public ContextSaveChangeInterceptor(IClaimService claimService)
        {
            _claimService = claimService;
        }

        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            var dbContext = eventData.Context;


            if (dbContext is not null)
            {

                foreach (var entry in dbContext.ChangeTracker.Entries<BaseEntity>())
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            if (entry.Entity.Id == Guid.Empty)
                                entry.Entity.Id = Guid.NewGuid();

                            if (entry.Entity is BaseEntity baseEntity)
                            {
                                baseEntity.CreatedAt = DateTime.UtcNow;
                                baseEntity.UpdatedAt = baseEntity.CreatedAt;
                            }
                            break;

                        case EntityState.Modified:
                            if (entry.Entity is BaseEntity baseEntityModify)
                            {
                                baseEntityModify.UpdatedAt = DateTime.UtcNow;
                            }
                            break;
                    }
                    var a = 5;
                }
            }
            var saveChangeResult = await base.SavingChangesAsync(eventData, result, cancellationToken);
            return saveChangeResult;
        }
    }
}
