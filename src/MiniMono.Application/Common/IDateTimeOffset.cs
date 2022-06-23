namespace Wright.Demo.MiniMono.Application.Common;

/// <summary>
/// Abstraction for DateTimeOffset to facilitate dependency injection for
/// testing and ease of substitution.
/// </summary>
public interface IDateTimeOffset
{
	/// <summary>
	/// Gets a <see cref="DateTimeOffset"/> object whose date and time are set to
	/// the current Coordinated Universal Time (UTC) date and time and whose offset is
	/// <see cref="TimeSpan.Zero">Zero</see>.
	/// </summary>
	/// <value>
	/// An object whose date and time are set to
	/// the current Coordinated Universal Time (UTC) date and time and whose offset is
	/// <see cref="TimeSpan.Zero">Zero</see>.
	/// </value>
	/// <remarks>
	/// The <see cref="UtcNow"/> property computes the current Universal Coordinated Time (UTC) based
	/// on the local system's clock time and an offset defined by the local system's time zone.
	/// <para>
	/// The precision of the current UTC time's millisecond component depends on the resolution of the
	/// system clock. On Windows NT 3.5 and later, and Windows Vista operating systems, the clock's
	/// resolution is approximately 10-15 milliseconds.
	/// </para>
	/// </remarks>
	public DateTimeOffset UtcNow { get; }
}