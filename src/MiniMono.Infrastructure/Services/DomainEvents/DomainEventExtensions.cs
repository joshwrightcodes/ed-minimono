// <copyright file="DomainEventExtensions.cs" company="Josh Wright">
// Copyright 2022 Josh Wright. Use of this source code is governed by an MIT-style, license that can be found in the
// LICENSE file or at https://opensource.org/licenses/MIT.
// </copyright>

namespace Wright.Demo.MiniMono.Infrastructure.Services.DomainEvents;

using MediatR;
using Microsoft.EntityFrameworkCore;
using Wright.Demo.MiniMono.Domain.Common.DomainEvents;

public static class DomainEventExtensions
{
	public static async Task DispatchDomainEvents(this IMediator mediator, DbContext context)
	{
		IEnumerable<IHasDomainEvent> entities = context.ChangeTracker
			.Entries<IHasDomainEvent>()
			.Where(e => e.Entity.DomainEvents.Count > 0)
			.Select(e => e.Entity);

		var domainEvents = entities
			.SelectMany(e => e.DomainEvents)
			.ToList();

		entities
			.ToList()
			.ForEach(e => e.ClearDomainEvents());

		foreach (DomainEvent domainEvent in domainEvents)
		{
			await mediator.Publish(domainEvent);
		}
	}
}