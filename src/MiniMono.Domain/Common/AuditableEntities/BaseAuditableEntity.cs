// <copyright file="BaseAuditableEntity.cs" company="Josh Wright">
// Copyright 2022 Josh Wright. Use of this source code is governed by an MIT-style, license that can be found in the
// LICENSE file or at https://opensource.org/licenses/MIT.
// </copyright>

using System.ComponentModel.DataAnnotations.Schema;

namespace Wright.Demo.MiniMono.Domain.Common.AuditableEntities;

using DomainEvents;

/// <summary>
/// Base entity class for auditable entities.
/// </summary>
public abstract record BaseAuditableEntity : IHasDomainEvent
{
	private readonly List<DomainEvent> _domainEvents = new ();

	/// <summary>
	/// Gets or sets who was the entity created by.
	/// </summary>
	public string? CreatedBy { get; set; }

	/// <summary>
	/// Gets or sets when was the entity created.
	/// </summary>
	public DateTimeOffset Created { get; set; }

	/// <summary>
	/// Gets or sets who last modified the entity.
	/// </summary>
	public string? LastModifiedBy { get; set; }

	/// <summary>
	/// Gets or sets when was the entity last modified.
	/// </summary>
	public DateTimeOffset LastModified { get; set; }

	/// <summary>
	/// Gets or sets the unique identifier for the entity.
	/// </summary>
	public Guid Id { get; set; }

	/// <summary>
	/// Gets a collection of events for the entity.
	/// </summary>
	[NotMapped]
	public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

	/// <summary>
	/// Adds a new domain event for the entity.
	/// </summary>
	/// <param name="domainEvent">
	/// Domain event to raise for the entity.
	/// </param>
	public void AddDomainEvent(DomainEvent domainEvent) => _domainEvents.Add(domainEvent);

	/// <summary>
	/// Removes a particular domain event from the entity.
	/// </summary>
	/// <param name="domainEvent">
	/// Domain event to remove from the entity.
	/// </param>
	public void RemoveDomainEvent(DomainEvent domainEvent) => _domainEvents.Remove(domainEvent);

	/// <summary>
	/// Removes all domain events from the entity.
	/// </summary>
	public void ClearDomainEvents() => _domainEvents.Clear();
}