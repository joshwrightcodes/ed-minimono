// <copyright file="IHasDomainEvent.cs" company="Josh Wright">
// Copyright 2022 Josh Wright. Use of this source code is governed by an MIT-style, license that can be found in the
// LICENSE file or at https://opensource.org/licenses/MIT.
// </copyright>

namespace Wright.Demo.MiniMono.Domain.Common.DomainEvents;

using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// Defines Entity that has Domain Events.
/// </summary>
public interface IHasDomainEvent
{
	Guid Id { get; set; }

	[NotMapped]
	IReadOnlyCollection<DomainEvent> DomainEvents { get; }

	void AddDomainEvent(DomainEvent domainEvent);

	void RemoveDomainEvent(DomainEvent domainEvent);

	void ClearDomainEvents();
}