namespace Wright.Demo.MiniMono.Application.Common.Mapping;

using AutoMapper;

/// <summary>
/// Defines an entity that can be mapped from one type to another by AutoMapper
/// using either a default method or an overridden method in the implementing
/// class.
/// </summary>
/// <typeparam name="T">Type AutoMapper will map from.</typeparam>
public interface IMapFrom<T>
{
	/// <summary>
	/// Maps one type to another.
	/// </summary>
	/// <param name="profile">AutoMapper Profile.</param>
	/// <remarks>
	/// Contains a default implementation that uses <see cref="Profile.CreateMap(System.Type, System.Type)"/>.
	/// This can be overridden by implementing <see cref="Mapping(Profile)"/>.
	/// </remarks>
	void Mapping(Profile profile)
		=> profile.CreateMap(typeof(T), GetType());
}