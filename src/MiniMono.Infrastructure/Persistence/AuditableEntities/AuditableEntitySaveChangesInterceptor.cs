using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Wright.Demo.MiniMono.Application.Common;
using Wright.Demo.MiniMono.Application.Common.Identity;
using Wright.Demo.MiniMono.Domain.Common.AuditableEntities;

namespace Wright.Demo.MiniMono.Infrastructure.Persistence.AuditableEntities;

public class AuditableEntitySaveChangesInterceptor : SaveChangesInterceptor
{
	private readonly ICurrentUser _currentUserService;
	private readonly IDateTimeOffset _dateTimeOffset;

	public AuditableEntitySaveChangesInterceptor(
		ICurrentUser currentUserService,
		IDateTimeOffset dateTimeOffset)
	{
		_currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
		_dateTimeOffset = dateTimeOffset ?? throw new ArgumentNullException(nameof(dateTimeOffset));
	}

	public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
	{
		UpdateEntities(eventData.Context);

		return base.SavingChanges(eventData, result);
	}

	public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
	{
		UpdateEntities(eventData.Context);

		return base.SavingChangesAsync(eventData, result, cancellationToken);
	}

	public void UpdateEntities(DbContext? context)
	{
		if (context is null)
		{
			return;
		}

		foreach (EntityEntry<BaseAuditableEntity> entry in context.ChangeTracker.Entries<BaseAuditableEntity>())
		{
			if (entry.State == EntityState.Added)
			{
				entry.Entity.CreatedBy = _currentUserService.UserId;
				entry.Entity.Created = _dateTimeOffset.UtcNow;
				entry.Entity.LastModifiedBy = entry.Entity.CreatedBy;
				entry.Entity.LastModified = entry.Entity.Created;
			}

			if (entry.State is EntityState.Added or EntityState.Modified || entry.HasChangedOwnedEntities())
			{
				entry.Entity.LastModifiedBy = _currentUserService.UserId;
				entry.Entity.LastModified = _dateTimeOffset.UtcNow;
			}
		}
	}
}
