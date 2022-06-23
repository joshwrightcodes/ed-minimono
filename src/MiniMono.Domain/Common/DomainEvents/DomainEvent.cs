// <copyright file="DomainEvent.cs" company="Josh Wright">
// Copyright 2022 Josh Wright. Use of this source code is governed by an MIT-style, license that can be found in the
// LICENSE file or at https://opensource.org/licenses/MIT.
// </copyright>

namespace Wright.Demo.MiniMono.Domain.Common.DomainEvents;

using MediatR;

/// <summary>
/// Base class to represent an event in the system.
/// </summary>
public abstract class DomainEvent : INotification
{
}